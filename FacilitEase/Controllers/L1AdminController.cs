/*using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowAngularDev")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly IPriorityService _priorityService;
        private readonly ITicketService _ticketService;
        public EmployeeController(IDepartmentService departmentService,
            ICategoryService categoryService,
            IPriorityService priorityService,
            ITicketService ticketService)
        {
            _departmentService = departmentService;
            _categoryService = categoryService;
            _priorityService = priorityService;
            _ticketService = ticketService;
        }

        [HttpGet("departments")]
        public ActionResult<IEnumerable<DepartmentDto>> GetDepartments()
        {
            var departments = _departmentService.GetDepartments();
            return Ok(departments);
        }

        [HttpPost("departments")]
        public IActionResult CreateDepartment([FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null)
            {
                return BadRequest("DepartmentDto cannot be null");
            }

            _departmentService.CreateDepartment(departmentDto);

            return CreatedAtAction(nameof(GetDepartments), new { }, departmentDto);
        }

        [HttpGet("categories")]
        public ActionResult<IEnumerable<CategoryDto>> GetCategory()
        {
            var categories = _categoryService.GetCategory();
            return Ok(categories);
        }

        [HttpGet("priorities")]
        public ActionResult<IEnumerable<PriorityDto>> GetPriority()
        {
            var priority = _priorityService.GetPriority();
            return Ok(priority);
        }

        [HttpPost("raiseticket")]
        public IActionResult CreateTicket([FromBody] TicketDto ticketApiModel)
        {
            if (ticketApiModel == null)
            {
                return BadRequest("Invalid ticket data");
            }

            _ticketService.CreateTicketWithDocuments(ticketApiModel);

            return Ok("Ticket created successfully");
        }
    }

    *//*[HttpGet("user/{userId}")]
    public IActionResult GetTicketDetailsByUserId(int userId)
    {
        var ticketDetails = _ticketDetailsService.GetTicketDetailsByUserId(userId);

        if (ticketDetails == null || !ticketDetails.Any())
        {
            return NotFound("No ticket details found for the specified user ID.");
        }

        return Ok(ticketDetails);
    }*//*
}
*/

using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowAngularDev")]
    [ApiController]
    [Route("api/[controller]")]
    public class L1AdminController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly IPriorityService _priorityService;
        private readonly ITicketService _ticketService;
        private readonly ISLAService _slaService;

        public L1AdminController(IDepartmentService departmentService,
            ICategoryService categoryService,
            IPriorityService priorityService,
            ITicketService ticketService,
            ISLAService sLAService)
        {
            _departmentService = departmentService;
            _categoryService = categoryService;
            _priorityService = priorityService;
            _ticketService = ticketService;
            _slaService = sLAService;
        }

        [HttpGet("departments")]
        public ActionResult<IEnumerable<DepartmentDto>> GetDepartments()
        {
            var departments = _departmentService.GetDepartments();
            return Ok(departments);
        }

        [HttpPost("departments")]
        public IActionResult CreateDepartment([FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null)
            {
                return BadRequest("DepartmentDto cannot be null");
            }

            _departmentService.CreateDepartment(departmentDto);

            return CreatedAtAction(nameof(GetDepartments), new { }, departmentDto);
        }

        [HttpPatch("EditSLA")]
        public IActionResult EditSla([FromBody] EditSLAInfo request)
        {
            try
            {
                _slaService.EditSLA(request.DepartmentId, request.PriorityId, request.Time);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("SLAInfo/{userId}")]
        public ActionResult GetSlaInfo(int userId)
        {
            var slaInfo = _slaService.GetSLAInfo(userId);
            return Ok(slaInfo);
        }
    }
}