namespace FacilitEase.Models.EntityModels
{
    public class TBL_PROJECT_EMPLOYEE_MAPPING
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
