using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.ApiModels
{
    public class AssignRole
    {
        [Required]
        public int EmpId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}