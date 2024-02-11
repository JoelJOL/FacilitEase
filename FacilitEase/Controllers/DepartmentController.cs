using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowAngularDev")]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _deptService;

        public DepartmentController(IDepartmentService deptService)
        {
            _deptService = deptService;
        }


    
        [HttpGet("getAllExceptUserDepartment/{userId}")]
        public IActionResult GetAllDepartmentsExceptUserDepartment(int userId)
        {
            try
            {
                var departments = _deptService.GetAllDepartmentsExceptUserDepartment(userId);
                return Ok(departments);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("categories-by-department/{departmentId}")]
        public IActionResult GetCategoriesByDepartmentId(int departmentId)
        {
            var categories = _deptService.GetCategoriesByDepartmentId(departmentId);

            if (categories == null || categories.Count == 0)
            {
                return NotFound("No categories found for the specified department.");
            }

            return Ok(categories);
        }



    }
}