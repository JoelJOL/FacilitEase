using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class NotificationRepository : Repository<TBL_NOTIFICATION>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        // Implement any additional methods specific to TBL_NOTIFICATION here
    }
}