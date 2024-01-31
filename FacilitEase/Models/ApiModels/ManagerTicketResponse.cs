namespace FacilitEase.Models.ApiModels
{
    public class ManagerTicketResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalDataCount { get; set; }
    }
}
