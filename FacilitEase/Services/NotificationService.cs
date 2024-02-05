using FacilitEase.Hubs;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;
using Microsoft.AspNetCore.SignalR;

namespace FacilitEase.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IHubContext<NotificationHub> hubContext, IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
        }

        public async Task MonitorTicketChanges()
        {
            // Get the initial state of the tickets
            var initialTickets = _unitOfWork.Ticket.GetAll().ToDictionary(t => t.Id, t => new { t.StatusId, t.ControllerId });

            while (true)
            {
                // Get the current state of the tickets
                var currentTickets = _unitOfWork.Ticket.GetAll().ToDictionary(t => t.Id, t => new { t.StatusId, t.ControllerId });

                // Find the tickets that have changed
                var changedTickets = currentTickets.Where(ct => !Equals(ct.Value, initialTickets[ct.Key])).ToList();

                foreach (var changedTicket in changedTickets)
                {
                    // Determine the messages to send based on the new status and controller
                    var messages = GetMessagesForStatusAndController(changedTicket.Value.StatusId, changedTicket.Value.ControllerId, changedTicket.Key);

                    // Send a notification to the user and controller
                    foreach (var message in messages)
                    {
                        await _hubContext.Clients.User(message.UserId.ToString()).SendAsync("ReceiveNotification", message.Text);

                        // Create a new notification in the database
                        var notification = new TBL_NOTIFICATION
                        {
                            Content = $"TicketId: {changedTicket.Key}, {message.Text}",
                            TicketId = changedTicket.Key,
                            Sender = null,
                            Receiver = message.UserId,
                            NotificationTimestamp = DateTime.Now
                        };
                        _unitOfWork.Notification.Add(notification);
                    }
                }

                // Save the changes to the database
                await _unitOfWork.CompleteAsync();

                // Update the initial state to the current state
                initialTickets = currentTickets;

                // Wait for a while before checking again
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private List<Message> GetMessagesForStatusAndController(int? statusId, int? controllerId, int ticketId)
        {
            var messages = new List<Message>();
            var ticket = _unitOfWork.Ticket.GetById(ticketId);

            if (statusId.HasValue && controllerId.HasValue)
            {
                switch (statusId)
                {
                    case 1: // Open
                        messages.Add(new Message { UserId = (int)controllerId, Text = "New ticket is generated" });
                        break;
                    case 2: // In progress
                        messages.Add(new Message { UserId = (int)ticket.UserId, Text = "Ticket in progress" });
                        messages.Add(new Message { UserId = (int)controllerId, Text = "Ticket assigned" });
                        break;
                    case 3: // Escalated
                        messages.Add(new Message { UserId = (int)ticket.UserId, Text = "Ticket escalated" });
                        messages.Add(new Message { UserId = (int)controllerId, Text = "Ticket escalated" });
                        break;
                    case 4: // Resolved
                        messages.Add(new Message { UserId = (int)ticket.UserId, Text = "Ticket resolved" });
                        break;
                    case 5: // Cancelled
                        messages.Add(new Message { UserId = (int)ticket.UserId, Text = "Ticket cancelled" });
                        messages.Add(new Message { UserId = (int)ticket.AssignedTo, Text = "Ticket cancelled" });
                        break;
                    case 6: // On hold
                        messages.Add(new Message { UserId = (int)ticket.UserId, Text = "Ticket sent for approval" });
                        messages.Add(new Message { UserId = (int)controllerId, Text = "New Ticket" });
                        break;
                    case 7: // Cancel requested
                        messages.Add(new Message { UserId = (int)controllerId, Text = "Cancel request received" });
                        break;
                    default:
                        messages.Add(new Message { UserId = (int)controllerId, Text = "Unknown status" });
                        break;
                }
            }
            return messages;

        }
    }

    public class Message
    {
        public int UserId { get; set; }
        public string Text { get; set; }
    }


}
