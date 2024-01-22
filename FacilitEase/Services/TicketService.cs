using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly object employeeDto;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TicketDto> GetTickets()
        {

            var tickets = _unitOfWork.Ticket.GetAll();
            return MapToTicketDtoList(tickets);
        }


        private IEnumerable<TicketDto> MapToTicketDtoList(IEnumerable<TBL_TICKET> tickets)
        {
            return tickets.Select(MapToTicketDto);
        }

        private TicketDto MapToTicketDto(TBL_TICKET tickets)
        {
            return new TicketDto
            {
                Id = tickets.Id,
                TicketDescription = tickets.TicketDescription,
                AssignedTo = tickets.AssignedTo,
                CategoryId = tickets.CategoryId,
                PriorityId = tickets.PriorityId,
                SubmittedDate = tickets.SubmittedDate,
                StatusId = tickets.StatusId,
                UserId = tickets.UserId,
                CreatedBy = tickets.CreatedBy,
                CreatedDate = tickets.CreatedDate,
                UpdatedBy = tickets.UpdatedBy,
                UpdatedDate = tickets.UpdatedDate,
            };
        }
        public void CreateTicket(TicketDto ticketDto)
        {

            var ticketEntity = MapToTBL_TICKET(ticketDto);
            _unitOfWork.Ticket.Add(ticketEntity);
            _unitOfWork.Complete();
        }

        private TBL_TICKET MapToTBL_TICKET(TicketDto ticketDto)
        {

            return new TBL_TICKET
            {
                Id = ticketDto.Id,
                TicketDescription = ticketDto.TicketDescription,
                AssignedTo = ticketDto.AssignedTo,
                CategoryId = ticketDto.CategoryId,
                PriorityId = ticketDto.PriorityId,
                SubmittedDate = ticketDto.SubmittedDate,
                StatusId = ticketDto.StatusId,
                UserId = ticketDto.UserId,
                CreatedBy = ticketDto.CreatedBy,
                CreatedDate = ticketDto.CreatedDate,
                UpdatedBy = ticketDto.UpdatedBy,
                UpdatedDate = ticketDto.UpdatedDate,
            };
        }
    }
}
