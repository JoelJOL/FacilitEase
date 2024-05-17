using FacilitEase.Models.EntityModels;

namespace FacilitEase.Contracts.RepositoryContracts
{
    public interface IUserRoleMappingRepository : IRepository<TBL_USER_ROLE_MAPPING>
    {
        IEnumerable<int> GetUserIdsByRoleId(int userRoleId);
    }
}