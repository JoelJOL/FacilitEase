namespace FacilitEase.Models.ApiModels
{
    public class Asset
    {
        
            public int AssetId { get; set; }
            public string AssetName { get; set; }
            public string WarrantyInfo { get; set; }
            public DateTime? LastMaintenanceDate { get; set; }
            public DateTime? NextMaintenanceDate { get; set; }
            public int LocationId { get; set; }
            public string LocationName { get; set; }
            public int TypeId { get; set; }
            public string TypeName { get; set; }
            public int StatusId { get; set; }
            public string StatusName { get; set; }
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string Status { get; set; }
            public int? TicketId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
        }

    }

