namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// get employee details
    /// </summary>
    public class ManagerSubordinateEmployee
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
    }
}
