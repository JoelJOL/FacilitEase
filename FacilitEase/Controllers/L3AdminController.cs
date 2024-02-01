using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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



        public class ApiResponse
        {
            public string Message { get; set; }
        }

        [HttpPatch("resolve-ticket/{ticketId}")]
        public IActionResult CloseRaisedTicketStatus([FromRoute] int ticketId)
        {
            _adminService.CloseTicket(ticketId);
            return Ok(new ApiResponse { Message = $"Ticket {ticketId} resolved successfully." });

        }

        [HttpGet("forward-ticket/{ticketId}/{managerId}")]
        public IActionResult ForwardRaisedTicketStatus([FromRoute] int ticketId, [FromRoute] int managerId)
        {
            _adminService.ForwardTicket(ticketId, managerId);
            return Ok(new ApiResponse { Message = $"Ticket {ticketId} status updated successfully." });
        }

        [HttpGet("forward-ticket-to-department/{ticketId}/{deptId}")]
        public IActionResult ForwardRaisedTicketDepartment([FromRoute] int ticketId, [FromRoute] int deptId)
        {
            _adminService.ForwardTicketToDept(ticketId, deptId);
            return Ok(new ApiResponse { Message = $"Ticket {ticketId} status updated successfully." });
        }

        [HttpGet("GetRaisedTicketsByAgent/{agentId}")]
        public IActionResult GetTicketsByAgent(
             int agentId,
             string sortField = null,
             string sortOrder = null,
             int pageIndex = 0,
             int pageSize = 10,
             string searchQuery = null)
        {
            try
            {
                var tickets = _adminService.GetTicketsByAgent(agentId, sortField, sortOrder, pageIndex, pageSize, searchQuery);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetResolvedTicketsByAgent/{agentId}")]
        public IActionResult GetResolvedTicketsByAgent(
         int agentId,
         string sortField = null,
         string sortOrder = null,
         int pageIndex = 0,
         int pageSize = 10,
         string searchQuery = null)
        {
            try
            {
                var tickets = _adminService.GetResolvedTicketsByAgent(agentId, sortField, sortOrder, pageIndex, pageSize, searchQuery);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
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

        [HttpGet("ticket-commenttext/{ticketId}")]
        public ActionResult<string> GetCommentTextByTicketId(int ticketId)
        {
            var commentText = _adminService.GetCommentTextByTicketId(ticketId);

            if (commentText == null)
            {
                return NotFound($"No comment text found for TicketId: {ticketId}");
            }

            return Ok(commentText);
        }

        [HttpPatch("update-comment/{ticketId}")]
        public IActionResult UpdateComment([FromRoute] int ticketId, [FromBody] UpdateCommentDto model)
        {
            try
            {
                _adminService.UpdateCommentTextByTicketId(ticketId, model.NewText);
                return Ok("Comment updated successfully");
            }
            catch (Exception ex)
            {
                // Handle exceptions according to your application's needs
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet("escalted-tickets-by-agent/{agentId}")]
        public IActionResult GetEscalatedTicketsByAgent([FromRoute] int agentId)
        {
            try
            {
                var tickets = _adminService.GetEscalatedTicketsByAgent(agentId);
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
