// IEmployeeService.cs
using FacilitEase.Models.EntityModels;

public interface IEmployeeService
{
    /* void AddEmployee(TBL_EMPLOYEE employee);*/
    void AddEmployees(IEnumerable<EmployeeInputModel> employeeInputs1, params EmployeeInputModel[] employeeInputs);

    void DeleteEmployee(int id);
}
