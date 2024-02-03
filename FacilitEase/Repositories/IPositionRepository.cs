using FacilitEase.Models.EntityModels;
using System.Collections.Generic;

namespace FacilitEase.Repositories
{
    public interface IPositionRepository : IRepository<Position>
    {
        // Add specific methods for positions if needed
        IEnumerable<Position> GetByName(string positionName);
    }
}
