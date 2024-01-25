using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class PriorityService : IPriorityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriorityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PriorityDto> GetPriority()
        {
            var priorities = _unitOfWork.Priority.GetAll();
            return MapToPriorityDtoList(priorities);
        }

        private IEnumerable<PriorityDto> MapToPriorityDtoList(IEnumerable<TBL_PRIORITY> priorities)
        {
            return priorities.Select(MapToPriorityDto);
        }

        private PriorityDto MapToPriorityDto(TBL_PRIORITY priority)
        {
            return new PriorityDto
            {
                Id = priority.Id,
                PriorityName = priority.PriorityName,
                CreatedBy = priority.CreatedBy,
                CreatedDate = priority.CreatedDate,
                UpdatedBy = priority.UpdatedBy,
                UpdatedDate = priority.UpdatedDate,
            };
        }
    }
}
