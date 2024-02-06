namespace FacilitEase.Models.EntityModels
{
    public class TBL_ASSET_TYPE
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? DepartmentId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}