// EmployeeController.cs
using FacilitEase.Models.EntityModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
[EnableCors("AllowAngularDev")]
[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    // POST api/employee
    // POST api/employee
    [HttpPost]
    public IActionResult AddEmployee([FromBody] EmployeeInputModel employeeInput)
    {
        if (employeeInput == null)
        {
            return BadRequest("Employee data is null.");
        }

        try
        {
            
            // Map the input model to your entity model before passing it to the service
            var employeeEntity = new TBL_EMPLOYEE
            {
                EmployeeCode = employeeInput.EmployeeCode,
                FirstName = employeeInput.FirstName,
                LastName = employeeInput.LastName,
                DOB = employeeInput.DOB,
                Email = employeeInput.Email,
                Gender = employeeInput.Gender,
                ManagerId = employeeInput.ManagerId,
                // Map other properties as needed
            };

            _employeeService.AddEmployee(employeeEntity);
            return Ok("Employee added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE api/employee/5
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
}
