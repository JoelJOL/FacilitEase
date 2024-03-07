namespace FacilitEase.Models.ApiModels
{
    public class AssetHistory
    {
        public int Id { get; set; }
        public string AssignedToEmployeeName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}