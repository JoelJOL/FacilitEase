// TicketController.cs
using Microsoft.AspNetCore.Mvc;
using FacilitEase.Services;
using FacilitEase.Models.ApiModels;
using Microsoft.AspNetCore.Cors;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowAngularDev")]
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // PUT api/tickets/change-status/{id}
        [HttpPut("change-status/{id}")]
        public async Task<IActionResult> ChangeTicketStatus(int id, [FromBody] TicketStatusChangeRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request body");
            }

            var result = await _ticketService.ChangeTicketStatus(id, request);

            if (result)
                return Ok();
            else
                return BadRequest("Failed to change ticket status");
        }
    }
}
