namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// // Represents a model for joining resolved ticket data in the API response. This is used in the data table fro resolved
    /// ticketsgit 
    /// </summary>
    public class TicketResolveJoin
    {
        public int Id { get; set; }
        public required string TicketName { get; set; }
        public required string EmployeeName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime ResolvedDate { get; set; }
        public required string PriorityName { get; set; }
    }
}
