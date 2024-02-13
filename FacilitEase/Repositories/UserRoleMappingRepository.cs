using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class UserRoleMappingRepository :Repository<TBL_USER_ROLE_MAPPING>, IUserRoleMappingRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRoleMappingRepository(AppDbContext dbContext) : base(dbContext) {
            
            
            _dbContext = dbContext;
        }




        public IEnumerable<int> GetUserIdsByRoleId(int userRoleId)
        {
            // Assuming there is a UserRoleId property in TBL_USER_ROLE_MAPPING
            return Context.Set<TBL_USER_ROLE_MAPPING>()
                .Where(mapping => mapping.UserRoleId == userRoleId)
                .Select(mapping => mapping.UserId)
                .ToList();
        }
    }

}
