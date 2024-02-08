using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FacilitEase.Controllers
{

    [EnableCors("AllowAngularDev")]
    [ApiController]
    [Route("api/[controller]")]
    public class L3AdminController : ControllerBase
    {
        private readonly IL3AdminService _adminService;
        private readonly ITicketService _ticketService;
        private readonly ILogger<L3AdminController> _logger;

        public L3AdminController(IL3AdminService adminService, ILogger<L3AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public class ApiResponse
        {
            public string Message { get; set; }
        }

        [HttpGet("tracking-details/{ticketId}")]
        public IActionResult GetTicketDetails(int ticketId)
        {
            try
            {
                var trackingDetails = _adminService.GetTicketDetails(ticketId);
                return Ok(trackingDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("resolve-ticket/{ticketId}")]
        public IActionResult CloseRaisedTicketStatus([FromRoute] int ticketId)
        {
            try
            {
                _adminService.CloseTicket(ticketId);
                _logger.LogInformation($"Ticket {ticketId} resolved successfully.");
                return Ok(new ApiResponse { Message = $"Ticket {ticketId} resolved successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resolving ticket {ticketId}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPatch("AcceptTicketCancellation/{ticketId}")]
        public IActionResult AcceptTicketCancellation([FromRoute] int ticketId)
        {
            try
            {
                _adminService.AcceptTicketCancellation(ticketId);
                _logger.LogInformation($"Ticket {ticketId} Cancellation Request Accepted.");
                return Ok(new ApiResponse { Message = $"Ticket {ticketId} Cancellation Request Accepted." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error accepting cancellation request for ticket {ticketId}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPatch("DenyTicketCancellation/{ticketId}")]
        public IActionResult DenyTicketCancellation([FromRoute] int ticketId)
        {
            try
            {
                _adminService.DenyTicketCancellation(ticketId);
                _logger.LogInformation($"Ticket {ticketId} Cancellation Request Denied.");
                return Ok(new ApiResponse { Message = $"Ticket {ticketId} Cancellation Request Denied." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error denying cancellation request for ticket {ticketId}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("forward-ticket/{ticketId}/{managerId}")]
        public IActionResult ForwardRaisedTicketStatus([FromRoute] int ticketId, [FromRoute] int managerId)
        {
            try
            {
                _adminService.ForwardTicket(ticketId, managerId);
                _logger.LogInformation($"Ticket {ticketId} status updated successfully. Forwarded to Manager ID: {managerId}");
                return Ok(new ApiResponse { Message = $"Ticket {ticketId} status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error forwarding ticket {ticketId} to Manager ID: {managerId}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("forward-ticket-to-department/{ticketId}/{deptId}")]
        public IActionResult ForwardRaisedTicketDepartment([FromRoute] int ticketId, [FromRoute] int deptId)
        {
            try
            {
                _adminService.ForwardTicketToDept(ticketId, deptId);
                _logger.LogInformation($"Ticket {ticketId} status updated successfully. Forwarded to Department ID: {deptId}");
                return Ok(new ApiResponse { Message = $"Ticket {ticketId} status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error forwarding ticket {ticketId} to Department ID: {deptId}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
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
                _logger.LogInformation($"Retrieved tickets for Agent ID: {agentId}");
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tickets for Agent ID: {agentId}");
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
                _logger.LogInformation($"Retrieved resolved tickets for Agent ID: {agentId}");
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving resolved tickets for Agent ID: {agentId}");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



        [HttpGet("GetOnHoldTicketsByAgent/{agentId}")]
        public IActionResult GetOnHoldTicketsByAgent(
     int agentId,
     string sortField = null,
     string sortOrder = null,
     int pageIndex = 0,
     int pageSize = 10,
     string searchQuery = null)
        {
            try
            {
                var tickets = _adminService.GetOnHoldTicketsByAgent(agentId, sortField, sortOrder, pageIndex, pageSize, searchQuery);
                _logger.LogInformation($"Retrieved On-Hold tickets for Agent ID: {agentId}");
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving On-Hold tickets for Agent ID: {agentId}");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetCancelRequestTicketsByAgent/{agentId}")]
        public IActionResult GetCancelRequestTicketsByAgent(
            int agentId,
            string sortField = null,
            string sortOrder = null,
            int pageIndex = 0,
            int pageSize = 10,
            string searchQuery = null)
        {
            try
            {
                var tickets = _adminService.GetCancelRequestTicketsByAgent(agentId, sortField, sortOrder, pageIndex, pageSize, searchQuery);
                _logger.LogInformation($"Retrieved Cancel Request tickets for Agent ID: {agentId}");
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving Cancel Request tickets for Agent ID: {agentId}");
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
                    _logger.LogInformation($"Ticket details not found for Ticket ID: {ticketId}");
                    return NotFound();
                }

                _logger.LogInformation($"Retrieved ticket details for Ticket ID: {ticketId}");
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving ticket details for Ticket ID: {ticketId}");
                return StatusCode(500, "Error retrieving ticket details");
            }
        }

        [HttpPost]
        public IActionResult PostComment([FromBody] CommentRequestDto commentRequestDto)
        {
            try
            {
                // Create a new Comment object with predefined values and provided text and TicketId
                TBL_COMMENT comment = new TBL_COMMENT
                {
                    TicketId = commentRequestDto.TicketId,
                    Text = commentRequestDto.Text,
                    Sender = 1, // Predefined value, replace with actual value
                    Receiver = 2, // Predefined value, replace with actual value
                    Category = "Note", // Predefined value, replace with actual value
                    CreatedBy = 1, // Predefined value, replace with actual value
                    UpdatedBy = 1, // Predefined value, replace with actual value
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                // Call the service method to save the comment
                _adminService.AddComment(comment);

                _logger.LogInformation($"Comment posted successfully for Ticket ID: {comment.TicketId}");
                return Ok(new { Message = "Comment posted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting comment");
                return StatusCode(500, new { Error = "Internal Server Error", Message = ex.Message });
            }
        }



        [HttpGet("ticket-commenttext/{ticketId}")]
        public ActionResult<string> GetCommentTextByTicketId(int ticketId)
        {
            try
            {
                var commentText = _adminService.GetCommentTextByTicketId(ticketId);

                if (commentText == null)
                {
                    _logger.LogInformation($"No comment text found for TicketId: {ticketId}");
                    return NotFound($"No comment text found for TicketId: {ticketId}");
                }

                _logger.LogInformation($"Retrieved comment text for TicketId: {ticketId}");
                return Ok(commentText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving comment text for TicketId: {ticketId}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPatch("update-comment/{ticketId}")]
        public IActionResult UpdateComment([FromRoute] int ticketId, [FromBody] UpdateCommentDto model)
        {
            try
            {
                _adminService.UpdateCommentTextByTicketId(ticketId, model.NewText);
                _logger.LogInformation($"Comment updated successfully for TicketId: {ticketId}");
                return Ok("Comment updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating comment for TicketId: {ticketId}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("TimeSinceLastUpdate/{ticketId}")]
        public IActionResult GetTimeSinceLastUpdate(int ticketId)
        {
            string timeSinceLastUpdate = _ticketService.GetTimeSinceLastUpdate(ticketId);

            if (timeSinceLastUpdate != null)
            {
                return Ok(timeSinceLastUpdate);
            }

            return NotFound("None");
        }



    }
}