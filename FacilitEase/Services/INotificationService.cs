namespace FacilitEase.Services
{
    public interface INotificationService
    {
        Task MonitorTicketChanges(CancellationToken cancellationToken);
    }
}