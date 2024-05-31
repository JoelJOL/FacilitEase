using FacilitEase.Contracts.ServiceContracts;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Shared
{
    public class Notification
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly INotificationService _notificationService;
        public Notification(IServiceScopeFactory scopeFactory, INotificationService notificationService)
        {
            _scopeFactory = scopeFactory;
            _notificationService = notificationService;
        }

        public async void OnTicketStatusChanged(object sender, EventArgs e)
        {
            var ticket = sender as TBL_TICKET;
            if (ticket != null)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await _notificationService.HandleTicketStatusChangedAsync(ticket, unitOfWork);
                }
            }
        }
    }
}
