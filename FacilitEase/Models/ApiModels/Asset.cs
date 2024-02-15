namespace FacilitEase.Models.ApiModels
{
    public class Asset
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string? WarrantyInfo { get; set; }
        public string? LastMaintenanceDate { get; set; }
        public string? NextMaintenanceDate { get; set; }
        public string AssetType { get; set; }
        public string? PurchaseDate { get; set; }
    }
}