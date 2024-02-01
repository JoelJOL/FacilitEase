namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// retrieve detailed information about agents
    /// </summary>
    public class AgentDetailsModel
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
}
