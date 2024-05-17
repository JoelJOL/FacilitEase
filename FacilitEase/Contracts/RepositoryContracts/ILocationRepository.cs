using FacilitEase.Models.EntityModels;

namespace FacilitEase.Contracts.RepositoryContracts
{
    public interface ILocationRepository : IRepository<TBL_LOCATION>
    {
        // Add specific methods for locations if needed
        IEnumerable<TBL_LOCATION> GetByName(string locationName);
    }
}