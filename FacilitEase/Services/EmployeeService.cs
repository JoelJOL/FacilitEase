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

    public void AddEmployee(TBL_EMPLOYEE employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee), "Employee data is null.");
        }

        try
        {
            // Add additional business logic if needed before calling the repository
            _unitOfWork.EmployeeRepository.Add(employee);
            _unitOfWork.Complete();
        }
        catch (Exception ex)
        {
            // Log the exception details or print to console for debugging
            Debug.WriteLine($"Error in AddEmployee: {ex.Message}");
            // Log or print the inner exception details
            Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw; // Re-throw the exception to propagate it up the call stack
        }
    }

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
