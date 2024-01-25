namespace FacilitEase.Models.ApiModels
{
    public class Join
    {
        public int Id { get; set; }
        public required string TicketName { get; set; }
        public required string TicketDescription { get; set; }
        public required string PriorityName { get; set; }
        public required string StatusName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public required string RaisedEmployeeName { get; set; }
        public required string LocationName { get; set; }

        public required string ManagerName { get; set; }
        public required string  DeptName { get; set; }
        public int? ManagerId { get; set; }
        public required string DocumentLink { get; set; }
        public required int ProjectCode { get; set;}
    }
}
