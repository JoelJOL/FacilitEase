using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowAngularDev")]
    [AllowAnonymous]
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

        [HttpPatch("forward-ticket/{ticketId}/{managerId}")]
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

        [HttpPatch("forward-ticket-deptHead/{ticketId}/{employeeId}")]
        public IActionResult ForwardRaisedTicketToDept([FromRoute] int ticketId, [FromRoute] int employeeId)
        {
            try
            {
                _adminService.ForwardTicketDeptHead(ticketId, employeeId);
                _logger.LogInformation($"Ticket {ticketId} status updated successfully. Forwarded to DU Head ID: {employeeId}");
                return Ok(new ApiResponse { Message = $"Ticket {ticketId} status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error forwarding ticket {ticketId} to DU Head ID: {employeeId}");
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
                RetrieveCommentDto createdComment = _adminService.AddComment(commentRequestDto);

                _logger.LogInformation($"Comment posted successfully for Ticket ID: {createdComment.TicketId}");

                // Return the created comment in the desired format
                return Ok(createdComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting comment");
                return StatusCode(500, new { Error = "Internal Server Error", Message = ex.Message });
            }
        }

        [HttpGet("ticket-commenttext/{ticketId}")]
        public ActionResult<List<RetrieveCommentDto>> GetCommentsForTicket(int ticketId)
        {
            try
            {
                var comments = _adminService.GetCommentTextsByTicketId(ticketId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpDelete("delete-comment/{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            try
            {
                // Call the service method to delete the comment
                _adminService.DeleteComment(commentId);

                _logger.LogInformation($"Comment deleted successfully: ID {commentId}");

                // Return a success message
                return Ok(new { Message = "Comment deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment");
                return StatusCode(500, new { Error = "Internal Server Error", Message = ex.Message });
            }
        }

        [HttpPatch("update-comment/{commentId}")]
        public IActionResult UpdateComment(int commentId, [FromBody] UpdateCommentDto updateCommentDto)
        {
            try
            {
                _logger.LogInformation($"Comment udpated successfully for Ticket ID");
                RetrieveCommentDto updatedComment =_adminService.UpdateComment(commentId, updateCommentDto.Text);

                return Ok(updatedComment);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error updating comment");
                return NotFound(new { Error = "Not Found", Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating comment");
                return StatusCode(500, new { Error = "Internal Server Error", Message = ex.Message });
            }
        }

        [HttpGet("TimeSinceLastUpdate/{ticketId}")]
        public IActionResult GetTimeSinceLastUpdate(int ticketId)
        {
            string timeSinceLastUpdate = _adminService.GetTimeSinceLastUpdate(ticketId);

            if (timeSinceLastUpdate != null)
            {
                return Ok(timeSinceLastUpdate);
            }

            return NotFound("None");
        }
    }
}