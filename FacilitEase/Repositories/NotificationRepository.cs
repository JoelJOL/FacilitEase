using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace FacilitEase.Repositories
{
    public class NotificationRepository : Repository<TBL_NOTIFICATION>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TBL_NOTIFICATION>> GetNotificationsByUserIdAsync(int userId)
        {
            return await Context.Set<TBL_NOTIFICATION>()
                .Where(n => n.Receiver == userId)
                .OrderByDescending(n => n.NotificationTimestamp)
                .ToListAsync();
        }
    }

}
