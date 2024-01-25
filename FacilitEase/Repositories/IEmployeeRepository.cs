using FacilitEase.Models.EntityModels;
﻿using FacilitEase.Models.ApiModels;

namespace FacilitEase.Repositories
{
    public interface IEmployeeRepository : IRepository<ManagerSubordinateEmployee, IRepository<TBL_EMPLOYEE>>
    {

    }
}
