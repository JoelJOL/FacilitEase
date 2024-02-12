namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// // Represents a model for joining resolved ticket data in the API response. This is used in the data table fro resolved
    /// ticketsgit
    /// </summary>
    public class ResolvedTicketDto
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string EmployeeName { get; set; }
        public string SubmittedDate { get; set; }
        public string ResolvedDate { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
    }
}