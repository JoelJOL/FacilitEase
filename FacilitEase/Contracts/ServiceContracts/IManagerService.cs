using FacilitEase.Models.ApiModels;

namespace FacilitEase.Contracts.ServiceContracts
{
    public interface IManagerService
    {
        Task<IEnumerable<ManagerAPI>> GetManagersAsync();
    }
}