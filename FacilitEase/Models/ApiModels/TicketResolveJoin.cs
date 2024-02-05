namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// // Represents a model for joining resolved ticket data in the API response. This is used in the data table fro resolved
    /// ticketsgit
    /// </summary>
    public class TicketResolveJoin
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime ResolvedDate { get; set; }
        public string Priority { get; set; }
    }
}