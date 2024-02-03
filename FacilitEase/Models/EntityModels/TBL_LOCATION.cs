namespace FacilitEase.Models.EntityModels
{
    public class TBL_LOCATION
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}