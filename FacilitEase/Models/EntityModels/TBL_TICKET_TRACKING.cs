namespace FacilitEase.Models.EntityModels
{
    public class TBL_TICKET_TRACKING
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int TicketStatusId { get; set; }
        public DateTime TicketRaisedTimestamp { get; set; }
        public int? AssignedTo { get; set; }
        public int? ApproverId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}