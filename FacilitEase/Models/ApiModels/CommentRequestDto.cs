namespace FacilitEase.Models.ApiModels
{
    public class CommentRequestDto
    {
        public string Text { get; set; }
        public int TicketId { get; set; }
        public int? ParentId { get; set; }
        public int UserId { get; set; }
    }
}