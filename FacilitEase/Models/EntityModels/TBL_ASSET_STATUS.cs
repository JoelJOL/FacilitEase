namespace FacilitEase.Models.EntityModels
{
    public class TBL_ASSET_STATUS
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
