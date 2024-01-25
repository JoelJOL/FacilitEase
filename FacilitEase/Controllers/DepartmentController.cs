using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowAngularDev")]
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

 
    }
}
