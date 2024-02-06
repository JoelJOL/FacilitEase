namespace FacilitEase.Models.ApiModels
{
    public class AgentTicketResponse<T>
    {
        /// <summary>
        /// Represents a generic response structure for API endpoints returning paginated data.
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        public int TotalDataCount { get; set; }
    }
}