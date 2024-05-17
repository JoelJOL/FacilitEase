using FacilitEase.Models.ApiModels;

namespace FacilitEase.Contracts.ServiceContracts
{
    public interface IPriorityService
    {
        IEnumerable<PriorityDto> GetPriority();
    }
}