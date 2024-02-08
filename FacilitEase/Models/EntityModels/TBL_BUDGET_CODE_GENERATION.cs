using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_BUDGET_CODE_GENERATION
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