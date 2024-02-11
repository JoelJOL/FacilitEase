namespace FacilitEase.Models.ApiModels
{
    public class Asset
    {


            public int Id { get; set; }
            public string AssetName { get; set; }
            public string? WarrantyInfo { get; set; }
            public DateTime? LastMaintenanceDate { get; set; }
            public DateTime? NextMaintenanceDate { get; set; }
            public string AssetType { get; set; }

            public DateTime PurchaseDate { get; set; }  
        
    }

}

