using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IL1AdminService
    {
        /// <summary>
        /// To get the suggestions of details of employees that are having similar names to the string contained in text
        /// </summary>
        /// <param name="text">Search parameter that is entered in the searchbar</param>
        /// <returns>All the details of employees that are having similar names to string in text</returns>
        IEnumerable<ProfileData> GetSuggestions(string text);
        /// <summary>
        /// To get all the roles of an employee that are available to him
        /// </summary>
        /// <param name="id">User id of the user whose assignable roles must be fetched</param>
        /// <returns>All assignable roles of a user</returns>
        IEnumerable<string> GetRoles(int id);
        /// <summary>
        /// Assigning a role to an employee
        /// </summary>
        /// <param name="assignRole">An apiModel that consists of the employeeId and the role name that must be assigned</param>
        void AssignRole(AssignRole assignrole);

        // public EmployeeTicketResponse<L1AdminTicketView> GetAllTickets(string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        EmployeeTicketResponse<L1AdminTicketView> GetAllTickets( string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        ManagerTicketResponse<TicketApiModel> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);
    }
}