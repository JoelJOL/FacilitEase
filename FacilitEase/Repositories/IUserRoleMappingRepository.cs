using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public interface IUserRoleMappingRepository : IRepository<TBL_USER_ROLE_MAPPING>
    {
        IEnumerable<int> GetUserIdsByRoleId(int userRoleId);
    }
}