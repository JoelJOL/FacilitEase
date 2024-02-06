namespace FacilitEase.Models.ApiModels
{
 
    public class TrackingDetailsDto
    {
        public int TicketId { get; set; }
        public string StatusName { get; set; }
        public string SubmittedByEmployeeName { get; set; }
        public string AssignedToEmployeeName { get; set; }
        public string ApproverEmployeeName { get; set; }
        public DateTime TrackingCreatedDate { get; set; }
    }

}
