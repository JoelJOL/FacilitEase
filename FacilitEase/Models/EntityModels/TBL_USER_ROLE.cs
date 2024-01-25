namespace FacilitEase.Models.EntityModels
{
    public class TBL_USER_ROLE
    {
        public int Id { get; set; }
        public string UserRoleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
