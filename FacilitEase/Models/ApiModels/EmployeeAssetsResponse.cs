namespace FacilitEase.Models.ApiModels
{
    public class EmployeeAssetsResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalDataCount { get; set; }
    }
}
