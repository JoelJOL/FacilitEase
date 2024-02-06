using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public interface IUserRepository : IRepository<TBL_USER>
    {
        object GetAllQ();
    }
}
