namespace FacilitEase.Models.ApiModels
{
    public class DepartmentHeadTicketResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalDataCount { get; set; }
    }
}