namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// Represents a model for joining data related to a ticket in the API response.
    /// </summary>
    public class Join
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string TicketDescription { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string RaisedEmployeeName { get; set; }
        public string LocationName { get; set; }

        public string ManagerName { get; set; }
        public string DeptName { get; set; }
        public int? ManagerId { get; set; }
        public string DocumentLink { get; set; }
        public int ProjectCode { get; set; }
    }
}