namespace FacilitEase.Models.ApiModels
{
    public class CommentResponseDto
    {
        public int id { get; set; }
        public string text { get; set; }
        public int ticketId { get; set; }
        public int? parentId { get; set; }
        public int userId { get; set; }
        public string employeeName { get; set; }
        public DateTime createdAt { get; set; }
    }
}
