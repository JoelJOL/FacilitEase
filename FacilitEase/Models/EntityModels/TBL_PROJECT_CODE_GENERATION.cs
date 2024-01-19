using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{

    public class TBL_PROJECT_CODE_GENERATION
    {
        [Key]
        public int ProjectCode { get; set; }

        [Required]
        public double BudgetAmount { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int UpdatedBy { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

       /* [ForeignKey("ProjectId")]
        public TBL_PROJECT? Project { get; set; }*/
    }
}
