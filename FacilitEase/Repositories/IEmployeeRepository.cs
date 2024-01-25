using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using System.Collections.Generic;

public interface IEmployeeRepository : IRepository<TBL_EMPLOYEE>,IRepository<ManagerSubordinateEmployee
{
    void AddRange(IEnumerable<TBL_EMPLOYEE> employees);

}
