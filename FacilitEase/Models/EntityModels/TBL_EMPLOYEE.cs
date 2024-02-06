using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_EMPLOYEE
    {
        [Key]
        public int Id { get; set; }

        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int? ManagerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}