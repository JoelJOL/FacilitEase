namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// To post a new ticket
    /// </summary>
    public class TicketDto
    {
        public string TicketName { get; set; }
        public string TicketDescription { get; set; }
        public int PriorityId { get; set; }
        public int CategoryId { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        public string DocumentLink { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int StatusId { get; set; }
        public int AssignedTo { get; set; }
    }
}