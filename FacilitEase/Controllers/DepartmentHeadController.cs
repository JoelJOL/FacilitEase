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
    public class DepartmentHeadController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public DepartmentHeadController(ITicketService ticketService)
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


        [HttpGet("GetApprovalTicket/{departmentHeadId}")]
        public IActionResult DHGetApprovalTicket(int departmentHeadId)
        {
            try
            {
                var tickets = _ticketService.DHGetApprovalTicket(departmentHeadId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("ViewTicketDetails/{ticketId}")]
        public IActionResult DHTicketDetails(int ticketId)
        {
            try
            {
                var ticketDetails = _ticketService.DHTicketDetails(ticketId);
                return Ok(ticketDetails);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
