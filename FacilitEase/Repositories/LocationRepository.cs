using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class LocationRepository : Repository<TBL_LOCATION>, ILocationRepository
    {
        public LocationRepository(AppDbContext context) : base(context)
        {
        }

        // Implement specific methods for locations if needed
        public IEnumerable<TBL_LOCATION> GetByName(string locationName)
        {
            return Context.Set<TBL_LOCATION>().Where(l => l.LocationName == locationName).ToList();
        }
    }
}