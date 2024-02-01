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

        /// <summary>
        /// To get all the priorities for a ticket
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PriorityDto> GetPriority()
        {
            var priorities = _unitOfWork.Priority.GetAll();
            return MapToPriorityDtoList(priorities);
        }

        /// <summary>
        /// The function takes a collection of TBL_PRIORITY entities and maps each entity to a PriorityDto using the MapToPriorityDto function. 
        /// It returns an IEnumerable<PriorityDto> containing the mapped PriorityDto objects.
        /// </summary>
        /// <param name="priorities"></param>
        /// <returns></returns>
        private IEnumerable<PriorityDto> MapToPriorityDtoList(IEnumerable<TBL_PRIORITY> priorities)
        {
            return priorities.Select(MapToPriorityDto);
        }


        /// <summary>
        /// This function takes a single TBL_PRIORITY entity and maps its properties to a new PriorityDto object.
        /// It is used by the MapToPriorityDtoList function to map individual TBL_PRIORITY entities to PriorityDto objects.
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
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
