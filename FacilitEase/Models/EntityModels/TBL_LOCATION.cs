using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{

    public class TBL_LOCATION
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string LocationName { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public int UpdatedBy { get; set; }
    }
}
