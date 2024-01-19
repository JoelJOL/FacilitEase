namespace FacilitEase.Models.EntityModels
{
    public class TBL_USER_ROLE_MAPPING
    {
        public int Id { get; set; }
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
