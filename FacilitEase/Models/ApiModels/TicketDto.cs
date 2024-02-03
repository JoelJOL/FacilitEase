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
            public byte[] DocumentLink { get; set; }
    }
}