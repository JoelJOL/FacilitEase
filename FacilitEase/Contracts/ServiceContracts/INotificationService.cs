using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Contracts.ServiceContracts
{
    public interface INotificationService
    {
        Task MonitorTicketChanges(CancellationToken cancellationToken);
        Task HandleTicketStatusChangedAsync(TBL_TICKET ticket, IUnitOfWork unitOfWork);
        void OnTicketStatusChanged(object sender, EventArgs e);
    }
}