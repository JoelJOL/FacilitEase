using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IReportService
    {
        /// <summary>
        /// To get the number of resolved, escalated and total tickets of an admin for a year
        /// </summary>
        /// <param name="id">The user id of the user whose data is required</param>
        /// <returns>An object of Report ApiModel</returns>
        Report GetReportDataYearTicketStatus(int id);

        /// <summary>
        /// To group the number of resolved and escalated tickets of an admin with respect to months of an year
        /// </summary>
        /// <param name="id">User id of the user whose</param>
        /// <returns>Object of the ApiModel chardata that consists of ticket count of each status sorted by the month</returns>
        ChartData GetBarChartData(int id);

        /// <summary>
        /// To get the EmployeeId, FirstName, LastName, JobTitle and Username of an employee
        /// </summary>
        /// <param name="id">User id of the user whose data is required</param>
        /// <returns>Object of ProfileData apiModel</returns>
        ProfileData GetProfileData(int id);

        /// <summary>
        /// To get the number of resolved, unresolved and escalated tickets of an admin in a week
        /// </summary>
        /// <param name="id">Iser id of the user whose data is required</param>
        /// <returns>Object of WeekReport ApiModel that consists of weekly ticket count of userunder each status</returns>
        WeekReport GetDailyAndWeeklyData(int id);

        /// <summary>
        /// To get the data required to display the report data that is categorised by ticket category and sorted into months for each status of resolved, unresolved and escalated
        /// </summary>
        /// <param name="id">the user id of the user</param>
        /// <returns>Dtaa for report of categories</returns>
        CategoryReportData GetReportDataByCategory(int id);
        ManagerTicketResponse<AdminReportTickets> GetTicketsByAdmin(int adminId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
    }
}