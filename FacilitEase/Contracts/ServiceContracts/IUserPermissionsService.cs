using FacilitEase.Models.ApiModels;

namespace FacilitEase.Contracts.ServiceContracts
{
    public interface IUserPermissionsService
    {
        IEnumerable<UserPermissionData> GetUserPermissions(int userId);

        bool UpdatePermission(int userPermissionId, bool isActive);
    }
}
