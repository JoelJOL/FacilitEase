using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.ApiModels
{
    public class AdminReportTickets
    {
        [Key]
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string EmployeeName { get; set; }
        public string Location { get; set; }
        public string AssignedTo { get; set; }
        public string SubmittedDate { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}
