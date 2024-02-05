using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using System.Collections.Generic;

public interface IEmployeeRepository : IRepository<TBL_EMPLOYEE>
{
    void AddRange(IEnumerable<TBL_EMPLOYEE> employees);

}
