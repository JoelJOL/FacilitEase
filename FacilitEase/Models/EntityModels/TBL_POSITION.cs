namespace FacilitEase.Models.EntityModels
{
    public class TBL_POSITION
    {
        public int Id { get; set; }
        public string PositionName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set;}

        
    }
}
