using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IPriorityService
    {
        IEnumerable<PriorityDto> GetPriority();
    }
}