using System.ComponentModel.DataAnnotations.Schema;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_ASSET_EMPLOYEE_MAPPING")]
    public class AssetEmployeeMapping
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; }
        public int? TicketId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
