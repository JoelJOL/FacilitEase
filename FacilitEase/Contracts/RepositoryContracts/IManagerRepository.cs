using FacilitEase.Models.ApiModels;

namespace FacilitEase.Contracts.RepositoryContracts
{
    public interface IManagerRepository
    {
        Task<IEnumerable<ManagerAPI>> GetManagersAsync();
    }
}