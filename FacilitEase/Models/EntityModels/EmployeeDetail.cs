using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_EMPLOYEE_DETAIL")]
    public class EmployeeDetail
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int LocationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
