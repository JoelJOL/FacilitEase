namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// To get the comments for a ticket
    /// </summary>
    public class CommentDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
    }
}