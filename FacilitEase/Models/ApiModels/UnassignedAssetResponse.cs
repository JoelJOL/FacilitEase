namespace FacilitEase.Models.ApiModels
{
    public class UnassignedAssetResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalDataCount { get; set; }
    }
}
