// TicketService.cs
using System;
using System.Threading.Tasks;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;


public class TicketService : ITicketService
{
    private readonly IRepository<TBL_TICKET> _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketService(IRepository<TBL_TICKET> ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request)
    {
        try
        {
            var ticket = _unitOfWork.TicketRepository.GetById(ticketId);

            if (ticket == null)
                return false;

            var newStatusId = request.IsApproved ? 3 : 2; // Set status based on IsApproved flag

            ticket.StatusId = newStatusId;

            /*_ticketRepository.Update(ticket);*/
            _unitOfWork.TicketRepository.Update(ticket);
            _unitOfWork.Complete();

            return true;
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately
            return false;
        }
    }
    // Implement other methods if needed
}
