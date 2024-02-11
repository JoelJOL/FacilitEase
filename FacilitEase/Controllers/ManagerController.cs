using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularDev")]
    public class ManagerController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IEmployeeService _employeeService;
        private readonly IManagerService _managerService;

        public ManagerController(ITicketService ticketService, IEmployeeService employeeService, IManagerService managerService)
        {
            _ticketService = ticketService;
            _employeeService = employeeService;
            _managerService = managerService;
        }

        [HttpGet("GetTicketByManager/{managerId}")]
        public IActionResult GetTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            try
            {
                var tickets = _ticketService.GetTicketByManager(managerId, sortField, sortOrder, pageIndex, pageSize, searchQuery);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetLiveTicketByManager/{managerId}")]
        public IActionResult GetLiveTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            try
            {
                var tickets = _ticketService.GetLiveTicketByManager(managerId, sortField, sortOrder, pageIndex, pageSize, searchQuery);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetApprovalTicket/{managerId}")]
        public IActionResult GetApprovalTicket(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            try
            {
                var tickets = _ticketService.GetApprovalTicket(managerId, sortField, sortOrder, pageIndex, pageSize, searchQuery);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("ViewTicketDetails/{ticketId}")]
        public IActionResult ViewTicketDetails(int ticketId)
        {
            try
            {
                var ticketDetails = _ticketService.ViewTicketDetails(ticketId);
                return Ok(ticketDetails);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPatch("ChangePriority")]
        public IActionResult ChangePriority([FromBody] ChangePriority request)
        {
            try
            {
                _ticketService.ChangePriority(request.TicketId, request.NewPriorityId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPatch("SendForApproval")]
        public IActionResult SendForApproval([FromBody] ChangeController request)
        {
            try
            {
                _ticketService.SendForApproval(request.TicketId, request.CurrentControllerId);
                return Ok("Successfully send for approval to Dept Head");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPatch("TicketDecision")]
        public IActionResult TicketDecision([FromBody] ChangeStatus request)
        {
            try
            {
                _ticketService.TicketDecision(request.TicketId, request.NewStatusId);
                return Ok("Status of ticket changed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{userId}/subordinates")]
        public ActionResult<List<ManagerSubordinateEmployee>> GetSubordinates(int userId)
        {
            try
            {
                var subordinates = _employeeService.GetSubordinates(userId);

                return Ok(subordinates);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManagerAPI>>> GetManagers()
        {
            var managers = await _managerService.GetManagersAsync();
            return Ok(managers);
        }
    }
}