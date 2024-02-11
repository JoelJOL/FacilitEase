namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// Represents a model for joining data related to a ticket in the API response.
    /// </summary>
    public class TicketDetailDataDto
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string TicketDescription { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string EmployeeName { get; set; }
        public string LocationName { get; set; }

        public string ManagerName { get; set; }
        public string DeptName { get; set; }
        public int? ManagerId { get; set; }
  
        public required string DocumentLink { get; set; }
        public required int ProjectCode { get; set;}
        public string Notes { get; set; }
        public string LastUpdate { get; set; }
    }
}