using System.ComponentModel.DataAnnotations.Schema;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_ASSET")]
    public class Asset
    {
    
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string WarrantyInfo { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public int? LocationId { get; set; }
        public int? TypeId { get; set; }
        public int? StatusId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
