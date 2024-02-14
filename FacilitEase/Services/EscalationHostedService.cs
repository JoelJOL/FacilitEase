using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace FacilitEase.Services
{
    /// <summary>
    /// Service responsible for escalating tickets based on certain conditions.
    /// </summary>
    public class EscalationHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private System.Threading.Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EscalationHostedService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public EscalationHostedService(IServiceProvider serviceProvider/*, ITicketService ticketService*/)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Starts the asynchronous operation of the hosted service.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the service.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Threading.Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(3));
            return Task.CompletedTask;
        }

        /// <summary>
        /// The method executed by the timer to perform the escalation of tickets.
        /// </summary>
        /// <param name="state">The state object (not used).</param>
        private void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var ticketsToEscalate = (from tickets in dbContext.TBL_TICKET
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
                                         }).ToList();

                foreach (var ticketInfo in ticketsToEscalate)
                {
                    ticketInfo.Ticket.StatusId = 3;
                    if (ticketInfo.Ticket.ControllerId != ticketInfo.Ticket.AssignedTo)
                    {
                        ticketInfo.Ticket.ControllerId = ticketInfo.ControllerManagerId;
                    }
                    else
                    {
                        ticketInfo.Ticket.ControllerId = null;
                    }
                    var trackingEntry = new TBL_TICKET_TRACKING
                    {
                        TicketId = ticketInfo.Ticket.Id,
                        TicketStatusId = ticketInfo.Ticket.StatusId,
                        AssignedTo = ticketInfo.Ticket.AssignedTo,
                        ApproverId = ticketInfo.Ticket.ControllerId,
                        TicketRaisedTimestamp = ticketInfo.Ticket.SubmittedDate,
                        CreatedBy = ticketInfo.Ticket.UserId,
                        UpdatedBy = ticketInfo.Ticket.UserId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };

                    dbContext.TBL_TICKET_TRACKING.Add(trackingEntry);

                    var ticketassign = (from ta in dbContext.TBL_TICKET_ASSIGNMENT
                                        where ta.TicketId == ticketInfo.Ticket.Id
                                        select ta).FirstOrDefault();
                    if (ticketassign != null)
                    {
                        ticketassign.EmployeeStatus = "escalated";
                    }
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Stops the asynchronous operation of the hosted service.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the service.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(System.Threading.Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Disposes the resources used by the hosted service.
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
