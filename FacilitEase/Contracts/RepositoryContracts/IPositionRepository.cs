using FacilitEase.Models.EntityModels;

namespace FacilitEase.Contracts.RepositoryContracts
{
    public interface IPositionRepository : IRepository<TBL_POSITION>
    {
        // Add specific methods for positions if needed
        IEnumerable<TBL_POSITION> GetByName(string positionName);
    }
}