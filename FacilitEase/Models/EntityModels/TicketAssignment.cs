using System.ComponentModel.DataAnnotations.Schema;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_TICKET_ASSIGNMENT")]
    public class TBL_TICKET_ASSIGNMENT
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime TicketAssignedTimestamp { get; set; }
        public string EmployeeStatus { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
