using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_USER_PERMISSION
    {
        [Key]
        public long Id { get; set; }

        public long PermissionId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
