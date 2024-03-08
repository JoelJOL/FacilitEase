namespace FacilitEase.Models.ApiModels
{
    public class CommentSectionDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string ParentId { get; set; }
        public string Text { get; set; }
        public string EmployeeName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
