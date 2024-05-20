using FacilitEase.Contracts.RepositoryContracts;
using FacilitEase.Models.EntityModels;

public interface IEmployeeDetailRepository : IRepository<TBL_EMPLOYEE_DETAIL>
{
    void AddRange(IEnumerable<TBL_EMPLOYEE_DETAIL> employeeDetails);
}