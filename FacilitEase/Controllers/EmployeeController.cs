

using FacilitEase.Models.ApiModels;
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
}
