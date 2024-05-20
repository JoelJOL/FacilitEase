using FacilitEase.Contracts.RepositoryContracts;
using FacilitEase.Contracts.ServiceContracts;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        public async Task<IEnumerable<ManagerAPI>> GetManagersAsync()
        {
            return await _managerRepository.GetManagersAsync();
        }
    }
}