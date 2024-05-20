namespace FacilitEase.Contracts.ServiceContracts
{
    public interface INotificationService
    {
        Task MonitorTicketChanges(CancellationToken cancellationToken);
    }
}