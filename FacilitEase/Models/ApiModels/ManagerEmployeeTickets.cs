using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.ApiModels
{
    public class ManagerEmployeeTickets
    {
        [Key]
        public int Id { get; set; }
        public string TicketName { get; set; }

        public string EmployeeName { get; set; }
        public string AssignedTo { get; set; }

        public DateTime SubmittedDate { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }


    }
}
