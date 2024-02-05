using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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
            report.Unresolved = (from ta in _context.TBL_TICKET_ASSIGNMENT
                                 where ta.EmployeeId == id && ta.EmployeeStatus == "unresolved"
                                 select ta).Count();
            report.Resolved = (from ta in _context.TBL_TICKET_ASSIGNMENT
                               where ta.EmployeeId == id && ta.EmployeeStatus == "resolved"
                               select ta).Count();
            report.Escalated = (from ta in _context.TBL_TICKET_ASSIGNMENT
                                where ta.EmployeeId == id && ta.EmployeeStatus == "escalated"
                                select ta).Count();
            report.Total = report.Unresolved + report.Resolved + report.Escalated;

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

            var ticketCountsByMonth = _context.TBL_TICKET_ASSIGNMENT
                .Where(ta => ta.EmployeeId == id)
                .GroupBy(ta => ta.TicketAssignedTimestamp.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    ResolvedCount = group.Count(ta => ta.EmployeeStatus == "resolved"),
                    UnresolvedCount = group.Count(ta => ta.EmployeeStatus == "unresolved"),
                    EscalatedCount = group.Count(ta => ta.EmployeeStatus == "escalated")
                })
                 .ToList();

            foreach (var entry in ticketCountsByMonth)
            {

                switch (entry.Month)
                {
                    case 1: chartData.January = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 2: chartData.February = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 3: chartData.March = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 4: chartData.April = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 5: chartData.May = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 6: chartData.June = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 7: chartData.July = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 8: chartData.August = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 9: chartData.September = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 10: chartData.October = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 11: chartData.November = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                    case 12: chartData.December = new int[] { entry.ResolvedCount, entry.UnresolvedCount, entry.EscalatedCount }; break;
                }
            }
            return chartData;

        }
        public ProfileData GetProfileData(int id)
        {
            ProfileData profileData = new ProfileData();
            profileData.EmpId=(from u in _context.TBL_USER
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
                                    select u.Email).FirstOrDefault()?.ToString()??"";

            profileData.JobTitle= (from p in _context.TBL_POSITION
                                join ed in _context.TBL_EMPLOYEE_DETAIL on p.Id equals ed.PositionId
                                join u in _context.TBL_USER on ed.EmployeeId equals u.EmployeeId
                                where u.EmployeeId == id
                                select p.PositionName).FirstOrDefault()?.ToString()??"";
            return profileData;
        }
    }
}
