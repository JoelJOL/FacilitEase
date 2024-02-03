using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_PROJECT_CODE_GENERATION")]
    public class ProjectCodeGeneration
    {
        [Key]
        public int ProjectCode { get; set; }
        public double BudgetAmount { get; set; }
        public int ProjectId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
