using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowLocalhost")]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController: ControllerBase
    {
        private readonly IDepartmentService _deptService;

        public DepartmentController(IDepartmentService deptService)
        {
            _deptService = deptService;
        }


        [HttpGet]
        public IActionResult GetTickets()
        {
            var depts = _deptService.GetAllDepartments();
            return Ok(depts);
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
