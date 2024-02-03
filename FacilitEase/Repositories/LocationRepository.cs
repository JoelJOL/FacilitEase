using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FacilitEase.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext context) : base(context)
        {
        }

        // Implement specific methods for locations if needed
        public IEnumerable<Location> GetByName(string locationName)
        {
            return Context.Set<Location>().Where(l => l.LocationName == locationName).ToList();
        }
    }
}
