using FacilitEase.Contracts.ServiceContracts;
using FacilitEase.Models.ApiModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [ApiController]
    [Route("api/l2")]
    [EnableCors("AllowAngularDev")]
    public class L2AdminController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ITicketService _ticketService;
        private readonly ISLAService _slaService;

        public L2AdminController(IEmployeeService employeeService, ITicketService ticketService, ISLAService slaService)
        {
            _employeeService = employeeService;
            _ticketService = ticketService;
            _slaService = slaService;
        }

        [HttpGet("agents/{userId}")]
        public ActionResult<IEnumerable<AgentApiModel>> GetAgents(int userId)
        {
            try
            {
                var agents = _employeeService.GetAgents(userId);
                return Ok(agents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("ticket-details/{ticketId}")]
        public ActionResult<TicketDetails> GetTicketDetails(int ticketId)
        {
            try
            {
                var tickets = _ticketService.GetTicketDetails(ticketId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("unassigned-tickets/{userId}")]
        public ActionResult<ManagerTicketResponse<UnassignedTicketModel>> GetUnassignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            try
            {
                var unassignedTickets = _ticketService.GetUnassignedTickets(userId, pageIndex, pageSize, sortField, sortOrder, searchQuery);
                return Ok(unassignedTickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("assign-ticket")]
        public ActionResult AssignTicketToAgent(AssignTicket request)
        {
            try
            {
                Console.WriteLine($"TicketId: {request.TicketId}, AgentId: {request.AgentId}");
                _ticketService.AssignTicketToAgent(request.UserId, request.TicketId, request.AgentId);
                return Ok(new { Message = "Ticket assigned successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("assigned-tickets/{userId}")]
        public ActionResult<ManagerTicketResponse<TicketApiModel>> GetAssignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            try
            {
                var assignedTickets = _ticketService.GetAssignedTickets(userId, pageIndex, pageSize, sortField, sortOrder, searchQuery);
                return Ok(assignedTickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("escalated-tickets/{userId}")]
        public ActionResult<ManagerTicketResponse<TicketApiModel>> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            var escalatedTickets = _ticketService.GetEscalatedTickets(userId, pageIndex, pageSize, sortField, sortOrder, searchQuery);
            return Ok(escalatedTickets);
        }

        [HttpGet("agents-details/{userId}")]
        public ActionResult<List<AgentDetailsModel>> GetAgentsByDepartment(int userId)
        {
            var agents = _employeeService.GetAgentsByDepartment(userId);
            return Ok(agents);
        }

        [HttpGet("sla-info/{TicketId}")]
        public ActionResult GetSlaInfo(int TicketId)
        {
            var slaTicketInfo = _slaService.GetTicketSLA(TicketId);
            return Ok(slaTicketInfo);
        }

        [HttpPost("edit-sla")]
        public IActionResult EditTicketSla([FromBody] EditTicketSLAInfo request)
        {
            try
            {
                _slaService.EditTicketSLA(request.TicketId, request.Time);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}