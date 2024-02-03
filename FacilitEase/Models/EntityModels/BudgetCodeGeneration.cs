using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_BUDGET_CODE_GENERATION")]
    public class BudgetCodeGeneration
    {
        [Key]
        public int Id { get; set; }
        public int BudgetCode { get; set; }
        public float BudgetAmount { get; set; }
        public int DepartmentId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
