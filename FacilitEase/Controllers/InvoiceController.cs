using FacilitEase.Services;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [Route("api")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("upload-invoice/{ticketId}")]
        public IActionResult UploadInvoice([FromForm] IFormFile file, int ticketId)
        {
            _invoiceService.UploadInvoice(file, ticketId);
            return Ok("Invoice uploaded successfully");
        }

        [HttpGet("invoices/{ticketId}")]
        public IActionResult GetInvoicesByTicketId(int ticketId)
        {
            var invoices = _invoiceService.GetDocumentByTicketIdAndCategory(ticketId);
            return Ok(invoices);
        }
    }
}
