using FacilitEase.Contracts.RepositoryContracts;
using FacilitEase.Models.EntityModels;

public interface IEmployeeRepository : IRepository<TBL_EMPLOYEE>
{
    void AddRange(IEnumerable<TBL_EMPLOYEE> employees);
}