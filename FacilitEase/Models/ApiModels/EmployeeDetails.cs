namespace FacilitEase.Models.ApiModels
{
    public class EmployeeDetails
    {
        public string EmployeeName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
        public string[] Roles { get; set; }
    }
}