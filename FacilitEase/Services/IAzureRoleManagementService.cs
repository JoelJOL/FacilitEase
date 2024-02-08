namespace FacilitEase.Services
{
    public interface IAzureRoleManagementService
    {
        Task<dynamic> GetAppRoles(string accessToken);
    }
}
