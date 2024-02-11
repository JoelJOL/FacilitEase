using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public ReportService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public Report GetReportData(int id)
        {
            Report report = new Report();
            report.Resolved = (from ta in _context.TBL_TICKET_ASSIGNMENT
                               join u in _context.TBL_USER on ta.EmployeeId equals u.EmployeeId
                               where u.Id == id && ta.EmployeeStatus == "resolved"
                               select ta).Count();
            report.Escalated = (from ta in _context.TBL_TICKET_ASSIGNMENT
                                join u in _context.TBL_USER on ta.EmployeeId equals u.EmployeeId
                                where u.Id == id && ta.EmployeeStatus == "escalated"
                                select ta).Count();
            report.Total = report.Resolved + report.Escalated;

            return report;
        }

        public ChartData GetChartData(int id)
        {
            var chartData = new ChartData
            {
                January = new int[] { 0, 0, 0 },
                February = new int[] { 0, 0, 0 },
                March = new int[] { 0, 0, 0 },
                April = new int[] { 0, 0, 0 },
                May = new int[] { 0, 0, 0 },
                June = new int[] { 0, 0, 0 },
                July = new int[] { 0, 0, 0 },
                August = new int[] { 0, 0, 0 },
                September = new int[] { 0, 0, 0 },
                October = new int[] { 0, 0, 0 },
                November = new int[] { 0, 0, 0 },
                December = new int[] { 0, 0, 0 }
            };
            var ticketCountsByMonth = from ta in _context.TBL_TICKET_ASSIGNMENT
                                      join u in _context.TBL_USER on ta.EmployeeId equals u.EmployeeId
                                      where u.Id == id
                                      group ta by ta.TicketAssignedTimestamp.Month into groupResult
                                      select new
                                      {
                                          Month = groupResult.Key,
                                          ResolvedCount = groupResult.Count(ta => ta.EmployeeStatus == "resolved"),
                                          EscalatedCount = groupResult.Count(ta => ta.EmployeeStatus == "escalated")
                                      };
            foreach (var entry in ticketCountsByMonth)
            {
                switch (entry.Month)
                {
                    case 1: chartData.January = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 2: chartData.February = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 3: chartData.March = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 4: chartData.April = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 5: chartData.May = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 6: chartData.June = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 7: chartData.July = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 8: chartData.August = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 9: chartData.September = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 10: chartData.October = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 11: chartData.November = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                    case 12: chartData.December = new int[] { entry.ResolvedCount, entry.EscalatedCount }; break;
                }
            }
            return chartData;
        }

        public ProfileData GetProfileData(int id)
        {
            ProfileData profileData = new ProfileData();
            profileData.EmpId = (from u in _context.TBL_USER
                                 where u.Id == id
                                 select u.EmployeeId).FirstOrDefault();
            profileData.EmployeeFirstName = (from p in _context.TBL_EMPLOYEE
                                             join u in _context.TBL_USER on p.Id equals u.EmployeeId
                                             where u.EmployeeId == id
                                             select p.FirstName).FirstOrDefault()?.ToString() ?? "";

            profileData.EmployeeLastName = (from p in _context.TBL_EMPLOYEE
                                            join u in _context.TBL_USER on p.Id equals u.EmployeeId
                                            where u.EmployeeId == id
                                            select p.LastName).FirstOrDefault()?.ToString() ?? "";

            profileData.Username = (from u in _context.TBL_USER
                                    where u.Id == id
                                    select u.Email).FirstOrDefault()?.ToString() ?? "";

            profileData.JobTitle = (from p in _context.TBL_POSITION
                                    join ed in _context.TBL_EMPLOYEE_DETAIL on p.Id equals ed.PositionId
                                    join u in _context.TBL_USER on ed.EmployeeId equals u.EmployeeId
                                    where u.EmployeeId == id
                                    select p.PositionName).FirstOrDefault()?.ToString() ?? "";
            return profileData;
        }

        public WeekReport GetWeeklyData(int id)
        {
            WeekReport weekReport = new WeekReport();

            DateTime currentDateTime = DateTime.Now;
            DateTime startDateOfWeek = currentDateTime.AddDays(-(int)currentDateTime.DayOfWeek);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            weekReport = (from ta in _context.TBL_TICKET_ASSIGNMENT
                          join u in _context.TBL_USER on ta.EmployeeId equals u.EmployeeId
                          where u.EmployeeId == id
                          group ta by 1 into g
                          select new WeekReport
                          {
                              DailyTickets = g.Count(ta => ta.TicketAssignedTimestamp.Date == currentDateTime),
                              DailyResolved = g.Count(ta => ta.TicketAssignedTimestamp.Date == currentDateTime && ta.EmployeeStatus == "resolved"),
                              DailyUnresolved = g.Count(ta => ta.TicketAssignedTimestamp.Date == currentDateTime && ta.EmployeeStatus == "unresolved"),
                              DailyEscalated = g.Count(ta => ta.TicketAssignedTimestamp.Date == currentDateTime && ta.EmployeeStatus == "escalated"),
                              WeeklyTickets = g.Count(ta => ta.TicketAssignedTimestamp.Date >= startDateOfWeek && ta.TicketAssignedTimestamp.Date <= endDateOfWeek),
                              WeeklyResolved = g.Count(ta => ta.TicketAssignedTimestamp.Date >= startDateOfWeek && ta.TicketAssignedTimestamp.Date <= endDateOfWeek && ta.EmployeeStatus == "resolved"),
                              WeeklyUnresolved = g.Count(ta => ta.TicketAssignedTimestamp.Date >= startDateOfWeek && ta.TicketAssignedTimestamp.Date <= endDateOfWeek && ta.EmployeeStatus == "unresolved"),
                              WeeklyEscalated = g.Count(ta => ta.TicketAssignedTimestamp.Date >= startDateOfWeek && ta.TicketAssignedTimestamp.Date <= endDateOfWeek && ta.EmployeeStatus == "escalated")
                          }).FirstOrDefault();
            return weekReport;
        }
    }
}