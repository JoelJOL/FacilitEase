
using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

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
            _timer = new System.Threading.Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try
                {
                    var ControllerManagerIds = (from controllerEmployee in dbContext.TBL_EMPLOYEE
                                                select new { controllerEmployee.Id, controllerEmployee.ManagerId }).ToList();

                    var ticketsToEscalate = from tickets in dbContext.TBL_TICKET
                                            let category = dbContext.TBL_CATEGORY.FirstOrDefault(cat => cat.Id == tickets.CategoryId)
                                            let categoryId = category != null ? category.DepartmentId : (int?)null
                                            where (tickets.StatusId == 1 || tickets.StatusId == 2 || tickets.StatusId == 6)
                                                  && (categoryId != null
                                                      && dbContext.TBL_SLA.Any(sla => sla.DepartmentId == categoryId.Value
                                                                                   && DateTime.Now > DateTime.Now.AddMinutes(sla.Time))
                                                      )
                                            select tickets;

                    foreach (var ticketInfo in ticketsToEscalate)
                    {
                        ticketInfo.StatusId = 3;

                        var controllerManager = ControllerManagerIds.FirstOrDefault(c => c.Id == ticketInfo.ControllerId);
                        if (controllerManager != null)
                        {
                            if (ticketInfo.ControllerId != ticketInfo.AssignedTo)
                            {
                                ticketInfo.ControllerId = controllerManager.ManagerId;
                            }
                            else
                            {
                                ticketInfo.ControllerId = controllerManager.ManagerId;
                                ticketInfo.AssignedTo = controllerManager.ManagerId;
                            }
                        }
                    }

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                // Separate queries for each join
                // Separate queries for each join
                /*var ticketsQuery = from tickets in dbContext.TBL_TICKET
                                   where (tickets.StatusId == 1 || tickets.StatusId == 2 || tickets.StatusId == 6)
                                   select tickets;

                var categoriesQuery = from tickets in ticketsQuery
                                      join categories in dbContext.TBL_CATEGORY
                                      on tickets.CategoryId equals categories.Id into categoryJoin
                                      from category in categoryJoin.DefaultIfEmpty()
                                      select category;


                var departmentsQuery = from departments in dbContext.TBL_DEPARTMENT
                                       join categories in categoriesQuery on departments.Id equals categories.DepartmentId
                                       select departments;

                var slaQuery = from sla in dbContext.TBL_SLA
                               join departments in departmentsQuery on sla.DepartmentId equals departments.Id
                               join categories in categoriesQuery on departments.Id equals categories.DepartmentId // Joining with categoriesQuery
                               select new { SLA = sla, Category = categories }; // Including category information

                var controllerEmployeeQuery = from controllerEmployee in dbContext.TBL_EMPLOYEE
                                              join tickets in ticketsQuery on controllerEmployee.Id equals tickets.ControllerId
                                              select controllerEmployee;

                // Main query combining all the above queries
                var ticketsToEscalateQuery = from tickets in ticketsQuery
                                             join slaInfo in slaQuery on tickets.CategoryId equals slaInfo.Category.Id
                                             join controllerEmployee in controllerEmployeeQuery on tickets.ControllerId equals controllerEmployee.Id
                                             where DateTime.Now > tickets.CreatedDate.AddMinutes(slaInfo.SLA.Time)
                                             select new
                                             {
                                                 Ticket = tickets,
                                                 ControllerManagerId = controllerEmployee.ManagerId,
                                             };

                // Materialize the query to a list for easier debugging
                var ticketsToEscalateList = ticketsToEscalateQuery.ToList();

                // Get the count of elements
                int countOfElements = ticketsToEscalateList.Count;

                // Now you can inspect the count or the elements themselves for debugging


                // Now you can inspect the count or the elements themselves for debugging*/

                try
                {
                    /*var ticketsToEscalate = from tickets in dbContext.TBL_TICKET
                                            join categories in dbContext.TBL_CATEGORY on tickets.CategoryId equals categories.Id
                                            join departments in dbContext.TBL_DEPARTMENT on categories.DepartmentId equals departments.Id
                                            join sla in dbContext.TBL_SLA on departments.Id equals sla.DepartmentId
                                            where (tickets.StatusId == 1 || tickets.StatusId == 2 || tickets.StatusId == 6)
                                            where DateTime.Now < tickets.SubmittedDate.AddMinutes(sla.Time)
                                            select tickets;*/
                    var ticketsToEscalate = from tickets in dbContext.TBL_TICKET
                                            let category = dbContext.TBL_CATEGORY.FirstOrDefault(cat => cat.Id == tickets.CategoryId)
                                            let categoryId = category != null ? category.DepartmentId : (int?)null
                                            where (tickets.StatusId == 1 || tickets.StatusId == 2 || tickets.StatusId == 6)
                                                  && (categoryId != null
                                                      && dbContext.TBL_SLA.Any(sla => sla.DepartmentId == categoryId.Value
                                                                                   && DateTime.Now > DateTime.Now.AddMinutes(sla.Time))
                                                      )
                                            select tickets;






                    foreach (var ticketInfo in ticketsToEscalate)
                    {
                        ticketInfo.StatusId = 3;
                        var ControllerManagerId = (from controllerEmployee in dbContext.TBL_EMPLOYEE
                                                   where ticketInfo.ControllerId == controllerEmployee.Id
                                                   select controllerEmployee.ManagerId).FirstOrDefault();
                    if (ticketInfo.ControllerId != ticketInfo.AssignedTo)
                        {
                            ticketInfo.ControllerId = ControllerManagerId;
                        }
                        else
                        {
                            ticketInfo.ControllerId = ControllerManagerId;
                            ticketInfo.AssignedTo = ControllerManagerId;
                        }

                        /*var ticketassign = dbContext.TBL_TICKET_ASSIGNMENT.FirstOrDefault(ta => ta.Id == ticketInfo.Ticket.Id);
                        if (ticketassign != null)
                        {
                            ticketassign.EmployeeStatus = "escalated";
                        }*/
                    }

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

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
