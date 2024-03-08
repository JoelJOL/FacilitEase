namespace FacilitEase.Models.ApiModels
{
    public class RetrieveCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int TicketId { get; set; }
        public int? ParentId { get; set; }
        public int UserId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RetrieveCommentDto> Replies { get; set; } 

        public RetrieveCommentDto()
        {
            Replies = new List<RetrieveCommentDto>();
        }
    }
}
