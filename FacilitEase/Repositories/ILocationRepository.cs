using FacilitEase.Models.EntityModels;
using System.Collections.Generic;

namespace FacilitEase.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {
        // Add specific methods for locations if needed
        IEnumerable<Location> GetByName(string locationName);
    }
}
