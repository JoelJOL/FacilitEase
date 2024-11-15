namespace FacilitEase.Models.ApiModels
{
    public class UserPermissionData
    {
        public long UserPermissionId { get; set; }
        public int UserId { get; set; }
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
        public int EmployeeId { get; set; } 
        public bool IsActive { get; set; }
    }
}
