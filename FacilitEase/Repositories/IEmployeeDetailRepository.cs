using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;

public interface IEmployeeDetailRepository : IRepository<TBL_EMPLOYEE_DETAIL>
{
    void AddRange(IEnumerable<TBL_EMPLOYEE_DETAIL> employeeDetails);
}