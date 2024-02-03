using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [ApiController]
    [Route("api/l2")]
    public class L2AdminController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ITicketService _ticketService;

        public L2AdminController(IEmployeeService employeeService, ITicketService ticketService)
        {
            _employeeService = employeeService;
            _ticketService = ticketService;
        }

        [HttpGet("agents")]
        public IActionResult GetAgents([FromQuery] DepartmentAgentsRequestModel requestModel)
        {
            var agents = _employeeService.GetAgents(requestModel.DepartmentId);
            return Ok(agents);
        }

        [HttpGet("ticketById")]
        public ActionResult<TicketDetails> GetTicketDetails(int desiredTicketId)
        {
            try
            {
                var tickets = _ticketService.GetTicketDetails(desiredTicketId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("unassigned-tickets")]
        public ActionResult<ManagerTicketResponse<UnassignedTicketModel>> GetUnassignedTickets(int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            try
            {
                var unassignedTickets = _ticketService.GetUnassignedTickets(pageIndex, pageSize, sortField, sortOrder, searchQuery);
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
                _ticketService.AssignTicketToAgent(request.TicketId, request.AgentId);
                return Ok(new { Message = "Ticket assigned successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("assigned-tickets")]
        public ActionResult<ManagerTicketResponse<TicketApiModel>> GetAssignedTickets(int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            try
            {
                var assignedTickets = _ticketService.GetAssignedTickets(pageIndex, pageSize, sortField, sortOrder, searchQuery);
                return Ok(assignedTickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("escalated-tickets")]
        public ActionResult<ManagerTicketResponse<TicketApiModel>> GetEscalatedTickets(int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            var escalatedTickets = _ticketService.GetEscalatedTickets(pageIndex, pageSize, sortField, sortOrder, searchQuery);
            return Ok(escalatedTickets);
        }

        [HttpGet("agentsByDepartmentId")]
        public ActionResult<List<AgentDetailsModel>> GetAgentsByDepartment(int departmentId)
        {
            var agents = _employeeService.GetAgentsByDepartment(departmentId);
            return Ok(agents);
        }
    }
}