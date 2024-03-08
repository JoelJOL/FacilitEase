using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;
using System.Net.Http.Headers;

namespace FacilitEase.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;
        private readonly IDocumentRepository _documentRepository;
        private readonly IInvoiceService _invoiceService;

        public InvoiceService(AppDbContext context, IDocumentRepository documentRepository)
        {
            _context = context;
            _documentRepository = documentRepository;
        }
        public void UploadInvoice(IFormFile file, int ticketId)
        {
            if (file != null && file.Length > 0)
            {
                var folderName = Path.Combine("Resources", "Invoice");
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                var fullPath = Path.Combine(folderName, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var documentEntity = new TBL_DOCUMENT
                {
                    DocumentLink = fullPath,
                    TicketId = ticketId,
                    CreatedBy = 1,
                    UpdatedBy = 1,
                    Category = "Invoice"
                };

                _documentRepository.Add(documentEntity);
                _context.SaveChanges();
            }
        }

        public IEnumerable<DocumentDto> GetDocumentsByTicketIdAndCategory(int ticketId)
        {
            return _context.TBL_DOCUMENT
                .Where(doc => doc.TicketId == ticketId && doc.Category == "Invoice")
                .Select(doc => new DocumentDto
                {
                    documentLink = doc.DocumentLink.Replace("\\", "/"),
                    
                })
                .ToList();
        }
    }
}
