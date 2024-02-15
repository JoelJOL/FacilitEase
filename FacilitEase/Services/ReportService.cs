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

        /// <summary>
        /// To get the number of resolved, escalated and total tickets of an admin for a year
        /// </summary>
        /// <param name="id">The user id of the user whose data is required</param>
        /// <returns>An object of Report ApiModel</returns>
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

        /// <summary>
        /// To group the number of resolved and escalated tickets of an admin with respect to months of an year
        /// </summary>
        /// <param name="id">User id of the user whose</param>
        /// <returns>Object of the ApiModel chardata that consists of ticket count of each status sorted by the month</returns>
        public ChartData GetChartData(int id)
        {
            //ApiModel to store data
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
            //Get the resolved and unresolved number of tickets from TicketAssignment table and group them usign a key
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

            //Adding the data into the above api model with respect the months
            //The arrayentered here consists of numbers which represets [number of resolved tickets, number of escalated tickets]
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

        /// <summary>
        /// To get the EmployeeId, FirstName, LastName, JobTitle and Username of an employee
        /// </summary>
        /// <param name="id">User id of the user whose data is required</param>
        /// <returns>Object of ProfileData apiModel</returns>
        public ProfileData GetProfileData(int id)
        {
            //ProfileData is an apiModel to get the data from database
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

        /// <summary>
        /// To get the number of resolved, unresolved and escalated tickets of an admin in a week
        /// </summary>
        /// <param name="id">Iser id of the user whose data is required</param>
        /// <returns>Object of WeekReport ApiModel that consists of weekly ticket count of userunder each status</returns>
        public WeekReport GetWeeklyData(int id)
        {
            //WeekReport is an ApiModel to store data from database
            WeekReport weekReport = new WeekReport();

            //To get the todays date, start date of current week and ending date of the current week
            DateTime currentDateTime = DateTime.Now;
            DateTime startDateOfWeek = currentDateTime.AddDays(-(int)currentDateTime.DayOfWeek);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            weekReport = (from ta in _context.TBL_TICKET_ASSIGNMENT
                          join u in _context.TBL_USER on ta.EmployeeId equals u.EmployeeId
                          where u.EmployeeId == id
                          group ta by 1 into g
                          select new WeekReport
                          {
                              DailyTickets = g.Count(ta => ta.TicketAssignedTimestamp.Date == currentDateTime.Date),
                              DailyResolved = g.Count(ta => ta.TicketAssignedTimestamp.Date == currentDateTime.Date && ta.EmployeeStatus == "resolved"),
                              DailyUnresolved = g.Count(ta => ta.TicketAssignedTimestamp.Date == currentDateTime.Date && ta.EmployeeStatus == "unresolved"),
                              DailyEscalated = g.Count(ta => ta.TicketAssignedTimestamp.Date == currentDateTime.Date && ta.EmployeeStatus == "escalated"),
                              WeeklyTickets = g.Count(ta => ta.TicketAssignedTimestamp.Date >= startDateOfWeek.Date && ta.TicketAssignedTimestamp.Date <= endDateOfWeek.Date),//Total tickets that are between startdate and enddate of the current week
                              WeeklyResolved = g.Count(ta => ta.TicketAssignedTimestamp.Date >= startDateOfWeek.Date && ta.TicketAssignedTimestamp.Date <= endDateOfWeek.Date && ta.EmployeeStatus == "resolved"),//Resolved tickets that are between startdate and enddate of the current week
                              WeeklyUnresolved = g.Count(ta => ta.TicketAssignedTimestamp.Date >= startDateOfWeek.Date && ta.TicketAssignedTimestamp.Date <= endDateOfWeek.Date && ta.EmployeeStatus == "unresolved"),//Unresolved tickets that are between startdate and enddate of the current week
                              WeeklyEscalated = g.Count(ta => ta.TicketAssignedTimestamp.Date >= startDateOfWeek.Date && ta.TicketAssignedTimestamp.Date <= endDateOfWeek.Date && ta.EmployeeStatus == "escalated")//Escalated tickets that are between startdate and enddate of the current week
                          }).FirstOrDefault();
            return weekReport;
        }

        /// <summary>
        /// To get the data required to display the report data that is categorised by ticket category and sorted into months for each status of resolved, unresolved and escalated
        /// </summary>
        /// <param name="id">the user id of the user</param>
        /// <returns>Dtaa for report of categories</returns>
        public CategoryReportData CategoryReport(int id)
        {
            //creating object of Api model categoryReportData
            var categoryReportData = new CategoryReportData
            {
                January = new CategoryReportMonthData[] { },
                February = new CategoryReportMonthData[] { },
                March = new CategoryReportMonthData[] { },
                April = new CategoryReportMonthData[] { },
                May = new CategoryReportMonthData[] { },
                June = new CategoryReportMonthData[] { },
                July = new CategoryReportMonthData[] { },
                August = new CategoryReportMonthData[] { },
                September = new CategoryReportMonthData[] { },
                October = new CategoryReportMonthData[] { },
                November = new CategoryReportMonthData[] { },
                December = new CategoryReportMonthData[] { }
            };

            //Selecting the data based on category for each month and counting the number of resolved, unresolved and escalated tickets of that category in each month
            var ticketCountsByMonthByCategory = from ta in _context.TBL_TICKET_ASSIGNMENT
                                                join u in _context.TBL_USER on ta.EmployeeId equals u.EmployeeId
                                                join ed in _context.TBL_EMPLOYEE_DETAIL on u.Id equals ed.Id
                                                join t in _context.TBL_TICKET on ta.TicketId equals t.Id
                                                join c in _context.TBL_CATEGORY on t.CategoryId equals c.Id
                                                where u.Id == id && ed.DepartmentId == c.DepartmentId
                                                group new { ta, c } by new { ta.TicketAssignedTimestamp.Month, c.CategoryName } into groupResult
                                                select new
                                                {
                                                    Month = groupResult.Key.Month,
                                                    CategoryName = groupResult.Key.CategoryName,
                                                    ResolvedCount = groupResult.Sum(x => x.ta.EmployeeStatus == "resolved" ? 1 : 0),
                                                    UnresolvedCount = groupResult.Sum(x => x.ta.EmployeeStatus == "unresolved" ? 1 : 0),
                                                    EscalatedCount = groupResult.Sum(x => x.ta.EmployeeStatus == "escalated" ? 1 : 0)
                                                };
            foreach (var entry in ticketCountsByMonthByCategory)
            {
                var categoryReportMonthData = new CategoryReportMonthData
                {
                    CategoryName = entry.CategoryName,
                    ResolvedCount = entry.ResolvedCount,
                    UnresolvedCount = entry.UnresolvedCount,
                    EscalatedCount = entry.EscalatedCount
                };

                CategoryReportMonthData[] monthArray;
                switch (entry.Month)
                {
                    case 1: monthArray = categoryReportData.January; break;
                    case 2: monthArray = categoryReportData.February; break;
                    case 3: monthArray = categoryReportData.March; break;
                    case 4: monthArray = categoryReportData.April; break;
                    case 5: monthArray = categoryReportData.May; break;
                    case 6: monthArray = categoryReportData.June; break;
                    case 7: monthArray = categoryReportData.July; break;
                    case 8: monthArray = categoryReportData.August; break;
                    case 9: monthArray = categoryReportData.September; break;
                    case 10: monthArray = categoryReportData.October; break;
                    case 11: monthArray = categoryReportData.November; break;
                    case 12: monthArray = categoryReportData.December; break;
                    default: monthArray = null; break;
                }

                // Append the new CategoryReportMonthData object to the existing array or create a new list
                if (monthArray != null)
                {
                    var monthList = monthArray.ToList();
                    monthList.Insert(0, categoryReportMonthData);
                    monthArray = monthList.ToArray();

                    // Assign the updated array back to the corresponding property
                    switch (entry.Month)
                    {
                        case 1: categoryReportData.January = monthArray; break;
                        case 2: categoryReportData.February = monthArray; break;
                        case 3: categoryReportData.March = monthArray; break;
                        case 4: categoryReportData.April = monthArray; break;
                        case 5: categoryReportData.May = monthArray; break;
                        case 6: categoryReportData.June = monthArray; break;
                        case 7: categoryReportData.July = monthArray; break;
                        case 8: categoryReportData.August = monthArray; break;
                        case 9: categoryReportData.September = monthArray; break;
                        case 10: categoryReportData.October = monthArray; break;
                        case 11: categoryReportData.November = monthArray; break;
                        case 12: categoryReportData.December = monthArray; break;
                    }
                }
            }

            return categoryReportData;
        }
    }
}