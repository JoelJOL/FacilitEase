// EmployeeService.cs
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;
using System;
using System.Diagnostics;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddEmployees(IEnumerable<EmployeeInputModel> employeeInputs, params EmployeeInputModel[] additionalEmployeeInputs)
    {
        if (employeeInputs == null || !employeeInputs.Any())
        {
            throw new ArgumentException("Employee input data is null or empty.", nameof(employeeInputs));
        }

        try
        {
            // Combine the two collections if needed
            var allEmployeeInputs = employeeInputs.Concat(additionalEmployeeInputs);

            // Map the input models to your entity models
            var employeeEntities = allEmployeeInputs.Select(employeeInput => new TBL_EMPLOYEE
            {
                EmployeeCode = employeeInput.EmployeeCode,
                FirstName = employeeInput.FirstName,
                LastName = employeeInput.LastName,
                DOB = employeeInput.DOB,
                Email = employeeInput.Email,
                Gender = employeeInput.Gender,
                ManagerId = employeeInput.ManagerId,
                // Map other properties as needed
            }).ToList();

            // Add additional business logic if needed before calling the repository
            _unitOfWork.EmployeeRepository.AddRange(employeeEntities);
            _unitOfWork.Complete();
        }
        catch (Exception ex)
        {
            // Log the exception details or print to console for debugging
            Debug.WriteLine($"Error in AddEmployees: {ex.Message}");
            // Log or print the inner exception details
            Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw; // Re-throw the exception to propagate it up the call stack
        }
    }

    /*  public void AddEmployee(EmployeeInputModel employeeInput)
      {
          AddEmployees(new List<EmployeeInputModel> { employeeInput });
      }*/


    public void DeleteEmployee(int id)
    {
        try
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            // Add additional business logic if needed before calling the repository
            _unitOfWork.EmployeeRepository.Delete(employee);
            _unitOfWork.Complete();
        }
        catch (Exception ex)
        {
            // Log the exception details or print to console for debugging
            Console.WriteLine($"Error in DeleteEmployee: {ex.Message}");
            // Log or print the inner exception details
            Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw; // Re-throw the exception to propagate it up the call stack
        }
    }



}
