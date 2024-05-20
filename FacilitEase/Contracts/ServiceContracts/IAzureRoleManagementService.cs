namespace FacilitEase.Contracts.ServiceContracts
{
    public interface IAzureRoleManagementService
    {
        Task<dynamic> GetAppRoles(string accessToken);
    }
}