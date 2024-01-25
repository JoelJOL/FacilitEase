namespace FacilitEase.Models.ApiModels
{
    public class TicketJoin
    {
        public int Id { get; set; }
        public required string TicketName { get; set; }
        public required string PriorityName { get; set; }
        public required string StatusName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public required string EmployeeName { get; set; }
    }
}
