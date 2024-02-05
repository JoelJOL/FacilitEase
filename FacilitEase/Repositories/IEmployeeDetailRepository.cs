using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using System.Collections.Generic;

public interface IEmployeeDetailRepository : IRepository<TBL_EMPLOYEE_DETAIL>
{
    void AddRange(IEnumerable<TBL_EMPLOYEE_DETAIL> employeeDetails);
}
