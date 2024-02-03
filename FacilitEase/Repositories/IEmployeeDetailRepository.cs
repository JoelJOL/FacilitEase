using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using System.Collections.Generic;

public interface IEmployeeDetailRepository : IRepository<EmployeeDetail>
{
    void AddRange(IEnumerable<EmployeeDetail> employeeDetails);
}
