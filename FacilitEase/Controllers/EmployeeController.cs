using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly IPriorityService _priorityService;
        private readonly ITicketService _ticketService;
        private readonly IEmployeeService _employeeService;
        private readonly ITicketDetailsService _ticketDetailsService;
        private readonly ICommentService _commentService;

        public EmployeeController(IDepartmentService departmentService,
            ICategoryService categoryService,
            IPriorityService priorityService,
            ITicketService ticketService,
            IEmployeeService employeeService,
            ITicketDetailsService ticketDetailsService,
            ICommentService commentService
            )
        {
            _departmentService = departmentService;
            _categoryService = categoryService;
            _priorityService = priorityService;
            _ticketService = ticketService;
            _employeeService = employeeService;
            _ticketDetailsService = ticketDetailsService;
            _commentService = commentService;
        }

        [HttpGet("departments")]
        public ActionResult<IEnumerable<DepartmentDto>> GetDepartments()
        {
            var departments = _departmentService.GetDepartments();
            return Ok(departments);
        }

        [HttpGet("positions")]
        public ActionResult<IEnumerable<TBL_POSITION>> GetPositions()
        {
            var positions = _employeeService.GetPositions();
            return Ok(positions);
        }

        [HttpGet("locations")]
        public ActionResult<IEnumerable<TBL_LOCATION>> GetLocations()
        {
            var locations = _employeeService.GetLocations();
            return Ok(locations);
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

        [HttpPost("create-with-documents")]
        public IActionResult CreateTicketWithDocuments([FromForm] TicketDto ticketDto, [FromForm] IFormFile file)
        {
            try
            {
                _ticketService.CreateTicketWithDocuments(ticketDto, file);
                return Ok(new { Message = "Ticket created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Error creating ticket: {ex.Message}" });
            }
        }

        [HttpPost("AddEmployees")]
        public IActionResult AddEmployees([FromBody] IEnumerable<EmployeeInputModel> employeeInputs)
        {
            if (employeeInputs == null || !employeeInputs.Any())
            {
                return BadRequest("Employee data is null or empty.");
            }

            try
            {
                _employeeService.AddEmployees(employeeInputs);
                return Ok("Employees added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("facilitiease")]
        public IActionResult GetCategoriesForFacilitiease()
        {
            var categories = _categoryService.GetCategoriesForFacilitiease();

            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found for Facilitiease.");
            }

            return Ok(categories);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                _employeeService.DeleteEmployee(id);
                return Ok($"Employee with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("cancel-request/{ticketId}")]
        public IActionResult RequestToCancelTicket(int ticketId)
        {
            bool success = _ticketService.RequestToCancelTicket(ticketId);

            if (success)
            {
                return Ok(new { Message = "Ticket cancellation request successful." });
            }
            else
            {
                return NotFound(new { Message = "Ticket not found or cancellation request failed." });
            }
        }

        [HttpGet("tickets/{userId}")]
        public IActionResult GetTicketDetailsByUserId(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var ticketDetails = _ticketDetailsService.GetTicketDetailsByUserId(userId, sortField, sortOrder, pageIndex, pageSize, searchQuery);

            if (ticketDetails == null)
            {
                return NotFound("No ticket details found for the specified user ID.");
            }

            return Ok(ticketDetails);
        }

        [HttpGet("ticket/{ticketId}")]
        public IActionResult GetTicketDetailsById(int ticketId)
        {
            var ticketDetails = _ticketDetailsService.GetTicketDetailsById(ticketId);

            if (ticketDetails == null)
            {
                return NotFound($"Ticket with ID {ticketId} not found.");
            }

            return Ok(ticketDetails);
        }

        [HttpGet("GetCategoryByDepartmentId/{departmentId}")]
        public IActionResult GetCategoryByDepartmentId(int departmentId)
        {
            try
            {
                var categories = _categoryService.GetCategoryByDepartmentId(departmentId);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest("Error processing the request. Please try again later.");
            }
        }

        [HttpGet("GetCommentsByTicketId/{ticketId}")]
        public IActionResult GetCommentsByTicketId(int ticketId)
        {
            try
            {
                var comments = _commentService.GetCommentsByTicketId(ticketId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving comments. Please try again later.");
            }
        }

        [HttpGet("get-documents-by-ticket/{ticketId}")]
        public IActionResult GetDocumentsByTicketId(int ticketId)
        {
            try
            {
                var documents = _ticketService.GetDocumentsByTicketId(ticketId);

                if (documents == null || !documents.Any())
                {
                    return NotFound($"No documents found for Ticket ID {ticketId}");
                }

                return Ok(documents);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving documents: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeDetails(int id)
        {
            return Ok(_employeeService.GetEmployeeDetails(id));
        }

        [HttpGet("employeesByProject/{userId}")]
        public ActionResult<IEnumerable<ProjectEmployeeDetails>> GetEmployeesByProject(int userId)
        {
            try
            {
                var employeesByProject = _employeeService.GetEmployeesByProject(userId);

                if (employeesByProject.Count == 0)
                {
                    return NotFound("No employees found for the specified user and project.");
                }

                return Ok(employeesByProject);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}