namespace FacilitEase.Models.EntityModels
{
    public class TBL_EMPLOYEE_DETAIL
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