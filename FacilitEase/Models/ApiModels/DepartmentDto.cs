namespace FacilitEase.Models.ApiModels
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string DeptName { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
