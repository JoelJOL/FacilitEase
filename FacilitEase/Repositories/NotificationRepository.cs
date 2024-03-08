using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace FacilitEase.Repositories
{
    /// <summary>
    /// Repository for managing notifications in the application.
    /// </summary>
    public class NotificationRepository : Repository<TBL_NOTIFICATION>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a list of notifications for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user for whom notifications are to be retrieved.</param>
        /// <returns>An asynchronous operation that represents the task for fetching notifications.</returns>
        public async Task<IEnumerable<TBL_NOTIFICATION>> GetNotificationsByUserIdAsync(int userId)
        {
            return await Context.Set<TBL_NOTIFICATION>()
                .Where(n => n.Receiver == userId)
                .OrderByDescending(n => n.NotificationTimestamp)
                .ToListAsync();
        }
    }
}