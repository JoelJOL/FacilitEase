using System.Collections.Generic;
using System.Threading.Tasks;
using FacilitEase.Models.ApiModels;
using FacilitEase.Repositories;

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
