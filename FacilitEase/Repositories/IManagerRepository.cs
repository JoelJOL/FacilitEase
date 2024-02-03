using FacilitEase.Models.ApiModels;

namespace FacilitEase.Repositories
{
    public interface IManagerRepository
    {
        Task<IEnumerable<ManagerAPI>> GetManagersAsync();
    }
}