using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{

    [EnableCors("AllowAngularDev")]
    [ApiController]
    [Route("api/[controller]")]
    public class L3AdminController : ControllerBase
    {
        private readonly IL3AdminService _adminService;

        public L3AdminController(IL3AdminService adminService)
        {
            _adminService = adminService;
        }

        

        [HttpGet("ticketdetail-by-agent/{ticketId}")]
        public IActionResult GetTicketDetailByAgent([FromRoute] int ticketId)
        {
            try
            {
                var ticket = _adminService.GetTicketDetailByAgent(ticketId);
                if (ticket == null)
                {
                    return NotFound();
                }

                return Ok(ticket);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Error retrieving ticket details");
            }
        }

        [HttpPatch("resolve-ticket/{ticketId}")]
        public IActionResult CloseRaisedTicketStatus([FromRoute] int ticketId)
        {
            _adminService.CloseTicket(ticketId);
            return Ok($"Ticket {ticketId} status updated successfully.");
        }

        [HttpGet("forward-ticket/{ticketId}/{managerId}")]
        public IActionResult ForwardRaisedTicketStatus([FromRoute] int ticketId, [FromRoute] int managerId)
        {
            _adminService.ForwardTicket(ticketId, managerId);
            return Ok($"Ticket {ticketId} status updated successfully.");
        }

        [HttpGet("forward-ticket-to-department/{ticketId}/{deptId}")]
        public IActionResult ForwardRaisedTicketDepartment([FromRoute] int ticketId, [FromRoute] int deptId)
        {
            _adminService.ForwardTicketToDept(ticketId, deptId);
            return Ok($"Ticket {ticketId} status updated successfully.");
        }



      



    }
}
