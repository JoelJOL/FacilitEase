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

        /* [HttpPost]
         public IActionResult AddTicket(TBL_TICKET ticket)
         {
             _adminService.AddTicket(ticket);
             return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
         }*/

        /*[HttpGet]
        public IActionResult GetTickets()
        {
            var tickets = _adminService.GetAllTickets();
            return Ok(tickets);
        }

        [HttpGet("assigned-to-agents")]
        public IActionResult GetTicketsAssignedToAents(int employeeId)
        {
            // Fetch tickets assigned to the current user
            var tickets = _adminService.GetTicketsByAgent(employeeId);

            return Ok(tickets);
        }*/

        /*[HttpGet("{id}")]
        public IActionResult GetTicketById(int id)
        {
            var ticket = _adminService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }*/



        /* [HttpGet("GetTicketsByUserIdId/{employeeId}")]
         public IEnumerable<TBL_TICKET> GetTicketsByEmployeeId(int employeeId)
         {
             return _adminService.GetTicketsByEmployeeId(employeeId);
         }*/

        [HttpGet("ticketdetails-by-agent/{agentId}")]
        public IActionResult GetTicketDetailsByAgent([FromRoute] int agentId)
        {
            try
            {
                var tickets = _adminService.GetTicketDetailsByAgent(agentId);
                if (tickets == null)
                {
                    return NotFound();
                }

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving ticket details");
            }
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


        [HttpGet("resolved-tickets-by-agent/{agentId}")]
        public IActionResult GetResolvedTicketsByAgent([FromRoute] int agentId)
        {
            try
            {
                var tickets = _adminService.GetResolvedTicketsByAgent(agentId);
                if (tickets == null)
                {
                    return NotFound();
                }

                return Ok(tickets);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, "Error retrieving ticket details");
            }
        }

        [HttpGet("latest-tickets-by-agent-desc/{agentId}")]
        public IActionResult GetLatestTicketsByAgent([FromRoute] int agentId)
        {
            try
            {
                var tickets = _adminService.GetLatestTicketsByAgent(agentId);
                if (tickets == null)
                {
                    return NotFound();
                }

                return Ok(tickets);
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, "Error retrieving ticket details");
            }
        }



    }
}
