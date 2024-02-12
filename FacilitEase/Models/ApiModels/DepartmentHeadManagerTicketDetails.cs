namespace FacilitEase.Models.ApiModels
{
    public class DepartmentHeadManagerTicketDetails
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string EmployeeName { get; set; }
        public string AssignedTo { get; set; }
        public string SubmittedDate { get; set; }
        public string priorityName { get; set; }
        public string statusName { get; set; }
        
        public string ForwardedBy { get; set; }

        public string Notes { get; set; }
        public string LastUpdate { get; set; }
        public string? TicketDescription { get; set; }
        public string? DocumentLink { get; set; }
    }
}