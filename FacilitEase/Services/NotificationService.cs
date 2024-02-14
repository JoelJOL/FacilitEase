using FacilitEase.Hubs;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace FacilitEase.Services
{
    public class NotificationService : INotificationService, IHostedService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        /*  private readonly AppDbContext _context;*/

        public NotificationService(IHubContext<NotificationHub> hubContext, IServiceScopeFactory scopeFactory)
        {
            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
     {
         using (var scope = _scopeFactory.CreateScope())
         {
             await MonitorTicketChanges(cancellationToken);
         }
     }, TaskCreationOptions.LongRunning);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //  cleanup logic if needed
            return Task.CompletedTask;
        }

        public async Task MonitorTicketChanges(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                // Get the initial state of the tickets
                var initialTickets = unitOfWork.Ticket.GetAll().ToDictionary(t => t.Id, t => new { t.StatusId, t.ControllerId });

                while (!cancellationToken.IsCancellationRequested)
                {
                    // Create a new scope for the current iteration
                    using (var innerScope = _scopeFactory.CreateScope())
                    {
                        var innerUnitOfWork = innerScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        // Get the current state of the tickets
                        var currentTickets = innerUnitOfWork.Ticket.GetAll().ToDictionary(t => t.Id, t => new { t.StatusId, t.ControllerId });

                        try
                        {
                            // Find the tickets that have changed
                            var changedTickets = currentTickets.Where(ct =>
                            {
                                if (!initialTickets.ContainsKey(ct.Key))
                                {
                                    // This is a new ticket, handle it separately
                                    // For example, send a notification or add it to a separate list
                                    return true;
                                }
                                var initialValue = initialTickets[ct.Key];
                                return ct.Value.StatusId != initialValue.StatusId ||
                                       (ct.Value.ControllerId == null && initialValue.ControllerId != null) ||
                                       (ct.Value.ControllerId != null && !ct.Value.ControllerId.Equals(initialValue.ControllerId));
                            }).ToList();

                            foreach (var changedTicket in changedTickets)
                            {
                                // Determine the messages to send based on the new status and controller
                                var messages = GetMessagesForStatusAndController(changedTicket.Value.StatusId, changedTicket.Value.ControllerId, changedTicket.Key);

                                // Send a notification to the user and controller
                                foreach (var message in messages)
                                {
                                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", new { UserId = message.UserId, Text = message.Text });

                                    // Create a new notification in the database
                                    var notification = new TBL_NOTIFICATION
                                    {
                                        Content = $"TicketId: {changedTicket.Key}, {message.Text}",
                                        TicketId = changedTicket.Key,
                                        Receiver = message.UserId,
                                        NotificationTimestamp = DateTime.Now
                                    };
                                    unitOfWork.Notification.Add(notification);
                                    // Save the changes to the database
                                    await unitOfWork.CompleteAsync();
                                }
                            }
                        }
                        catch (Exception ex) { Debug.WriteLine(ex); }

                        // Update the initial state to the current state
                        initialTickets = currentTickets;
                        Debug.WriteLine("Inside the loop");

                        // Wait for a while before checking again
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                }
            }
        }


        private List<Message> GetMessagesForStatusAndController(int? statusId, int? controllerId, int ticketId)
        {
            IUnitOfWork unitOfWork;
            List<TBL_USER> users = null;
            List<TBL_EMPLOYEE> employees = null;

            using (var scope = _scopeFactory.CreateScope())
            {
                unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                try
                {
                    users = unitOfWork.User.GetAll().ToList();
                    employees = unitOfWork.Employee.GetAll().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                var messages = new List<Message>();
                var ticket = unitOfWork.Ticket.GetById(ticketId);

                // Get the userId for the controllerId and AssignedTo
                var controllerUserId = users.FirstOrDefault(u => u.EmployeeId == controllerId)?.Id;
                var controllerName = employees.FirstOrDefault(e => e.Id == controllerId)?.FirstName.ToString();
                var assignedToUserId = users.FirstOrDefault(u => u.EmployeeId == ticket.AssignedTo)?.Id;
                var employeeId = users.FirstOrDefault(u => u.Id == ticket.UserId)?.EmployeeId;
                var managerId = employees.FirstOrDefault(e => e.Id == employeeId)?.ManagerId;
                var employeeFirstName = employees.FirstOrDefault(e => e.Id == employeeId)?.FirstName.ToString();

                if (statusId == 1)
                {
                    // Handle the case for a new ticket here
                    var l2AdminUserIds = unitOfWork.UserRoleMapping.GetUserIdsByRoleId(4);
                    foreach (var l2AdminUserId in l2AdminUserIds)
                    {
                        messages.Add(new Message { UserId = l2AdminUserId, Text = $"New ticket is generated by {employeeFirstName}" });
                    }
                    // Send a notification to the manager
                    if (managerId.HasValue)
                    {
                        messages.Add(new Message { UserId = managerId.Value, Text = $"New ticket created by your employee  {employeeFirstName}" });
                    }
                }
                else if (statusId.HasValue && (controllerUserId.HasValue || statusId == 5))
                {
                    // Handle other cases here
                    switch (statusId)
                    {
                        case 2: // In progress
                            messages.Add(new Message { UserId = (int)ticket.UserId, Text = $"Ticket assigned by {controllerName}" });
                            messages.Add(new Message { UserId = controllerUserId.Value, Text = "Ticket assigned" });
                            break;

                        case 3: // Escalated
                            messages.Add(new Message { UserId = (int)ticket.UserId, Text = $"Ticket escalated to {controllerName}" });
                            messages.Add(new Message { UserId = controllerUserId.Value, Text = "Ticket escalated" });
                            break;

                        case 4: // Resolved
                            messages.Add(new Message { UserId = (int)ticket.UserId, Text = $"Ticket resolved by {controllerName}" });
                            break;

                        case 5: // Cancelled
                            messages.Add(new Message { UserId = (int)ticket.UserId, Text = $"Ticket cancelled by {controllerName}" });
                            if (assignedToUserId.HasValue)
                            {
                                messages.Add(new Message { UserId = assignedToUserId.Value, Text = "Ticket cancelled" });
                            }
                            break;

                        case 6: // On hold
                            messages.Add(new Message { UserId = (int)ticket.UserId, Text = $"Ticket sent for approval to {controllerName}" });
                            messages.Add(new Message { UserId = controllerUserId.Value, Text = "New Ticket" });
                            break;

                        case 7: // Cancel requested
                            messages.Add(new Message { UserId = controllerUserId.Value, Text = $"Cancel request received from {controllerName}" });
                            break;

                        default:
                            messages.Add(new Message { UserId = controllerUserId.Value, Text = "Unknown status" });
                            break;
                    }
                }
                return messages;
            }
        }
    }

        public class Message
    {
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}