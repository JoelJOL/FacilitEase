using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using System.Collections.Generic;

public interface IEmployeeRepository : IRepository<Employee>
{
    void AddRange(IEnumerable<Employee> employees);

}
