namespace FacilitEase.Models.ApiModels
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public string UserRoleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
