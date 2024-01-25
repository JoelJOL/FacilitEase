namespace FacilitEase.Models.ApiModels
{
    public class TicketApiModel
    {
        public int TicketId { get; set; }
        public string TicketName { get; set; }
        public string TicketDescription { get; set; }
        public string RaisedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime RaisedDateTime { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}
