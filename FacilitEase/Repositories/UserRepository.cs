using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
    }
}
