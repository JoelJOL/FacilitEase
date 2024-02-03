﻿using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class EmployeeDetailRepository : Repository<EmployeeDetail>, IEmployeeDetailRepository
    {
        public EmployeeDetailRepository(AppDbContext context) : base(context)
        {
        }

        public void AddRange(IEnumerable<EmployeeDetail> employeeDetails)
        {
            // This implementation will call the AddRange method from the base class (generic repository)
            base.AddRange(employeeDetails);
        }
    }
}
