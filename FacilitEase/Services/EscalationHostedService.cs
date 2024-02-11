using FacilitEase.Data;

namespace FacilitEase.Services
{
    public class EscalationHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private System.Threading.Timer _timer;
        private readonly ITicketService _ticketService;

        public EscalationHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Threading.Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5000));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var ticketsToEscalate = from tickets in dbContext.TBL_TICKET
                                        join categories in dbContext.TBL_CATEGORY on tickets.CategoryId equals categories.Id
                                        join departments in dbContext.TBL_DEPARTMENT on categories.DepartmentId equals departments.Id
                                        join sla in dbContext.TBL_SLA on departments.Id equals sla.DepartmentId
                                        join controllerEmployee in dbContext.TBL_EMPLOYEE
                                            on tickets.ControllerId equals controllerEmployee.Id into controllerEmployees
                                        from controllerEmployee in controllerEmployees.DefaultIfEmpty()
                                        where sla.PriorityId == tickets.PriorityId
                                              && (tickets.StatusId == 1 || tickets.StatusId == 2 || tickets.StatusId == 6)
                                              && DateTime.UtcNow > tickets.SubmittedDate.AddMinutes(sla.Time)
                                        select new
                                        {
                                            Ticket = tickets,
                                            ControllerManagerId = controllerEmployee != null ? controllerEmployee.ManagerId : null
                                        };

                foreach (var ticketInfo in ticketsToEscalate)
                {
                    ticketInfo.Ticket.StatusId = 3;
                    if (ticketInfo.Ticket.ControllerId != ticketInfo.Ticket.AssignedTo)
                    {
                        ticketInfo.Ticket.ControllerId = ticketInfo.ControllerManagerId;
                    }
                    else
                    {
                        ticketInfo.Ticket.ControllerId = ticketInfo.ControllerManagerId;
                        ticketInfo.Ticket.AssignedTo = ticketInfo.ControllerManagerId;
                    }
                    /* var ticketassign = (from ta in dbContext.TBL_TICKET_ASSIGNMENT
                                         where ta.Id == ticketInfo.Ticket.Id
                                         select ta).FirstOrDefault();
                     if (ticketassign != null)
                     {
                         ticketassign.EmployeeStatus = "escalated";
                     }*/
                }

                dbContext.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(System.Threading.Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}