namespace FacilitEase.Models.ApiModels
{
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
