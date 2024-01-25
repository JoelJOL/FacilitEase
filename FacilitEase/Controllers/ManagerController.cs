using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FacilitEase.Controllers
{
    [ApiController]
    [Route("api/managers")]
    [EnableCors("AllowAngularDev")]
    public class ManagerController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public ManagerController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // Your existing actions...

        [HttpGet("{managerId}/subordinates")]
        public ActionResult<List<ManagerSubordinateEmployee>> GetSubordinateEmployees(int managerId)
        {
            try
            {
                var subordinates = _employeeService.GetSubordinates(managerId);
               

                return Ok(subordinates);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        
    }
}
