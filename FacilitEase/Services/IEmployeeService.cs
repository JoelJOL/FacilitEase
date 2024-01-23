// IEmployeeService.cs
using FacilitEase.Models.EntityModels;

public interface IEmployeeService
{
   /* void AddEmployee(TBL_EMPLOYEE employee);*/
    void AddEmployee(EmployeeInputModel employeeInput);
    void DeleteEmployee(int id);
}
