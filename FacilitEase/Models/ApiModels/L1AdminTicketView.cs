namespace FacilitEase.Models.ApiModels
{
    public class L1AdminTicketView
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string RaisedBy { get; set; }
        public string Priority { get; set; }

        public string Status { get; set; }
       
        public string Location { get; set; }
        public string Department { get; set; }
        public string SubmittedDate { get; set; }
       
        public string AssignedTo { get; set; }
    }
}
