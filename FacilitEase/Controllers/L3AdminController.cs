using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacilitEase.Controllers
{

    [EnableCors("AllowLocalhost")]
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

        [HttpPatch("AcceptTicketCancellation/{ticketId}")]
        public IActionResult AcceptTicketCancellation([FromRoute] int ticketId)
        {
            _adminService.AcceptTicketCancellation(ticketId);
            return Ok(new ApiResponse { Message = $"Ticket {ticketId} Cancellation Request Accepted." });

        }

        [HttpPatch("DenyTicketCancellation/{ticketId}")]
        public IActionResult DenyTicketCancellation([FromRoute] int ticketId)
        {
            _adminService.DenyTicketCancellation(ticketId);
            return Ok(new ApiResponse { Message = $"Ticket {ticketId} Cancellation Request Denied." });

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
                return Ok(tickets);
            }
            catch (Exception ex)
            {
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

        [HttpPost]
        public IActionResult PostComment([FromBody] CommentRequestDto commentRequestDto)
        {
            try
            {
                // Create a new Comment object with predefined values and provided text and TicketId
                Comment comment = new Comment
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

                return Ok(new { Message = "Comment posted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Internal Server Error", Message = ex.Message });
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

        [HttpDelete("delete-comment/{ticketId}")]
        public async Task<IActionResult> DeleteComment(int ticketId)
        {
            var success = await _adminService.DeleteCommentAsync(ticketId);

            if (success)
            {
                return NoContent(); // Successfully deleted
            }
            else
            {
                return NotFound(new { error = "Comment not found" });
            }
        }

    }
}
