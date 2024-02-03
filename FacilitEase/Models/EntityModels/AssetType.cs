using System.ComponentModel.DataAnnotations.Schema;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_ASSET_TYPE")]
    public class AssetType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? DepartmentId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
