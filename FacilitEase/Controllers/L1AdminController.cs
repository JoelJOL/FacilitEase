using FacilitEase.Contracts.ServiceContracts;
using FacilitEase.Models.ApiModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowAngularDev")]
    [ApiController]
    [Route("api")]
    public class L1AdminController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly IPriorityService _priorityService;
        private readonly ITicketService _ticketService;
        private readonly ISLAService _slaService;
        private readonly IL1AdminService _l1AdminService;

        public L1AdminController(IDepartmentService departmentService,
            ICategoryService categoryService,
            IPriorityService priorityService,
            ITicketService ticketService,
            ISLAService sLAService,
            IL1AdminService l1AdminService)
        {
            _departmentService = departmentService;
            _categoryService = categoryService;
            _priorityService = priorityService;
            _ticketService = ticketService;
            _slaService = sLAService;
            _l1AdminService = l1AdminService;
        }

        [HttpPost("l1/edit-sla")]
        public IActionResult EditSla([FromBody] EditSLAInfo request)
        {
            try
            {
                _slaService.EditSLA(request.DepartmentId, request.CategoryId, request.Time);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("l1/escalated-tickets/{userId}")]
        public ActionResult<ManagerTicketResponse<TicketApiModel>> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            try
            {
                var assignedTickets = _ticketService.GetEscalatedTickets(userId, pageIndex, pageSize, sortField, sortOrder, searchQuery);
                return Ok(assignedTickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("l1/tickets")]
        public ActionResult<EmployeeTicketResponse<L1AdminTicketView>> GetAllTickets(string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            try
            {
                var tickets = _l1AdminService.GetAllTickets(sortField, sortOrder, pageIndex, pageSize, searchQuery);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("l1/sla-info/{userId}")]
        public ActionResult GetSlaInfo(int userId)
        {
            var slaInfo = _slaService.GetSLAInfo(userId);
            return Ok(slaInfo);
        }
    }
}