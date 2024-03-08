namespace FacilitEase.Models.EntityModels
{
    public class TBL_COMMENT
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Text { get; set; }
        public int? ParentId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}