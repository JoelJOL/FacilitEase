namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// Represents a model for joining data related to a ticket in the API response. This api model is used 
    /// in the data table view
    /// </summary>
    public class TicketJoin
    {
        public int Id { get; set; }
        public required string TicketName { get; set; }
        public required string EmployeeName { get; set; }
        public required string PriorityName { get; set; }
        public required string StatusName { get; set; }
        public DateTime SubmittedDate { get; set; }




    }
}
