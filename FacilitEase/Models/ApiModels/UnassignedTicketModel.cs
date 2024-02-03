namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// Unassigned ticket details
    /// </summary>
    public class UnassignedTicketModel
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string RaisedBy { get; set; }
        public DateTime RaisedDateTime { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}