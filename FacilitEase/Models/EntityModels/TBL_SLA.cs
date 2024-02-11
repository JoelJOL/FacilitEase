using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_SLA
    {
        [Key]
        public int Id { get; set; }

        public int? PriorityId { get; set; }
        public int? DepartmentId { get; set; }
        public int Time { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}