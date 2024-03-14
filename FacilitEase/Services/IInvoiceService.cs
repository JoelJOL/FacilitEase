using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IInvoiceService
    {
        void UploadInvoice(IFormFile file, int ticketId);
        IEnumerable<DocumentDto> GetDocumentByTicketIdAndCategory(int ticketId);
    }
}
