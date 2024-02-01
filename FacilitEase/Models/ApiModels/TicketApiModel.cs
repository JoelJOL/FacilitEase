namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// to get the basic details of a ticket
    /// </summary>
    public class TicketApiModel
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string RaisedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime RaisedDateTime { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}
