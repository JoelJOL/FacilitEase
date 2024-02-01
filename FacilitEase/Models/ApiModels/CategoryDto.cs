namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// To get the categories
    /// </summary>
    public class CategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int DepartmentId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
