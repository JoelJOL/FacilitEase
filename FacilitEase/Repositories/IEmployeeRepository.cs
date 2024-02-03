using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;

public interface IEmployeeRepository : IRepository<TBL_EMPLOYEE>
{
    void AddRange(IEnumerable<TBL_EMPLOYEE> employees);
}