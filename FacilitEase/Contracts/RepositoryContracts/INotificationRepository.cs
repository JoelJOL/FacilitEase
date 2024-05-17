using FacilitEase.Models.EntityModels;

namespace FacilitEase.Contracts.RepositoryContracts
{
    public interface INotificationRepository : IRepository<TBL_NOTIFICATION>
    {
        Task<IEnumerable<TBL_NOTIFICATION>> GetNotificationsByUserIdAsync(int userId);
    }
}