using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;

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
            ICommentService commentService)
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

        /*[HttpPost("raiseticket")]
        public IActionResult CreateTicket([FromBody] TicketDto ticketApiModel)
        {
            if (ticketApiModel == null)
            {
                return BadRequest("Invalid ticket data");
            }

            _ticketService.CreateTicketWithDocuments(ticketApiModel);

            return Ok("Ticket created successfully");
        }*/
        /* [HttpPost("upload")]
         public IActionResult Upload()
         {
             try
             {
                 var file = Request.Form.Files[0];
                 var folderName = Path.Combine("Resources", "Images");
                 var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                 if (file.Length > 0 )
                 {
                     var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                     var fullPath = Path.Combine(pathToSave, fileName);
                     var dbPath = Path.Combine(folderName, fileName);

                     using (var stream = new FileStream(fullPath, FileMode.Create))
                     {
                         file.CopyTo(stream);
                     }

                     return Ok( new {dbPath});
                 }
                 else { return BadRequest(); }
             }
             catch (Exception ex)
             {
                 return BadRequest(ex.Message);
             }
         }*/

        [HttpPost("create-with-documents")]
        public IActionResult CreateTicketWithDocuments([FromForm] TicketDto ticketDto, [FromForm] IFormFile file)
        {
            try
            {
                // Call the service method to create the ticket with documents
                _ticketService.CreateTicketWithDocuments(ticketDto, file);

                // Return a success response if the ticket creation is successful
                return Ok(new { Message = "Ticket created successfully." });
            }
            catch (Exception ex)
            {
                // Return a bad request response if an exception occurs during the process
                return BadRequest(new { Message = $"Error creating ticket: {ex.Message}" });
            }
        }
        private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images");
        [HttpGet("{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine(_imagesPath, imageName);

            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg"); // Adjust the content type based on your image format
            }
            else
            {
                return NotFound();
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
            bool success = _ticketDetailsService.RequestToCancelTicket(ticketId);

            if (success)
            {
                return Ok(new { Message = "Ticket cancellation request successful." });
            }
            else
            {
                return NotFound(new { Message = "Ticket not found or cancellation request failed." });
            }
        }

        [HttpGet("myTickets/{userId}")]
        public IActionResult GetTicketDetailsByUserId(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var ticketDetails = _ticketDetailsService.GetTicketDetailsByUserId(userId, sortField, sortOrder, pageIndex, pageSize, searchQuery);

            if (ticketDetails == null)
            {
                return NotFound("No ticket details found for the specified user ID.");
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

        [HttpGet("{id}")]
        public IActionResult GetEmployeeDetails(int id)
        {
            return Ok(_employeeService.GetEmployeeDetails(id));
        }
    }
}