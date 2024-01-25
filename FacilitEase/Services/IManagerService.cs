using System.Collections.Generic;
using System.Threading.Tasks;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IManagerService
    {
        Task<IEnumerable<ManagerAPI>> GetManagersAsync();
    }
}
