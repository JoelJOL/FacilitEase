namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// Represents a model for joining data related to a ticket in the API response. This api model is used
    /// in the data table view
    /// </summary>
    public class TicketJoin
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string EmployeeName { get; set; }
        public string SubmittedDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
    }
}