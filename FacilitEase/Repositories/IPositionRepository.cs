using FacilitEase.Models.EntityModels;
using System.Collections.Generic;

namespace FacilitEase.Repositories
{
    public interface IPositionRepository : IRepository<TBL_POSITION>
    {
        // Add specific methods for positions if needed
        IEnumerable<TBL_POSITION> GetByName(string positionName);
    }
}
