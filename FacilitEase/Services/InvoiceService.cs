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

                // Check if a document with the same category and ticketId already exists
                var existingDocument = _context.TBL_DOCUMENT
                    .SingleOrDefault(doc => doc.TicketId == ticketId && doc.Category == "Invoice");

                if (existingDocument != null)
                {
                    // If exists, delete the existing file and update the database
                    File.Delete(existingDocument.DocumentLink);
                    existingDocument.DocumentLink = fullPath;
                    existingDocument.UpdatedBy = 1;
                    existingDocument.UpdatedDate = DateTime.Now;
                }
                else
                {
                    // If doesn't exist, create a new document entity
                    var documentEntity = new TBL_DOCUMENT
                    {
                        DocumentLink = fullPath,
                        TicketId = ticketId,
                        CreatedBy = 1,
                        UpdatedBy = 1,
                        Category = "Invoice"
                    };

                    _documentRepository.Add(documentEntity);
                }

                _context.SaveChanges();
            }
        }

        public IEnumerable<DocumentDto> GetDocumentByTicketIdAndCategory(int ticketId)

        {
            var documents = _context.TBL_DOCUMENT
                .Where(d => d.TicketId == ticketId && d.Category == "Invoice")
                .Select(d => new DocumentDto
                {
                    documentLink = d.DocumentLink.Replace("\\", "/")
                })
                .ToList();

            return documents;
        }
    }
}
