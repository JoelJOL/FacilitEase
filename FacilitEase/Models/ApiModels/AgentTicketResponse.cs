namespace FacilitEase.Models.ApiModels
{
    public class AgentTicketResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalDataCount { get; set; }
    }
}
