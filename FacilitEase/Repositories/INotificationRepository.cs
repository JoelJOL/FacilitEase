using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public interface INotificationRepository : IRepository<TBL_NOTIFICATION>
    {
        Task<IEnumerable<TBL_NOTIFICATION>> GetNotificationsByUserIdAsync(int userId);
    }

}
