namespace FacilitEase.Models.ApiModels
{
    //to get the details of a specific ticket
    public class TicketDetails
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string TicketDescription { get; set; }
        public string StatusName { get; set; }
        public string PriorityName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public string AssignedTo { get; set; }
        public string ManagerName { get; set; }
        public int? ManagerId { get; set; }
        public string LocationName { get; set; }
        public string DeptName { get; set; }
        public string DocumentLink { get; set; }
        public string Notes { get; set; }
        public string LastUpdate { get; set; }
    }
}