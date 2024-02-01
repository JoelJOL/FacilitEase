namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// to assign agents to tickets
    /// </summary>
    public class AssignTicket
    {
        public int TicketId { get; set; }
        public int AgentId { get; set; }
    }
}
