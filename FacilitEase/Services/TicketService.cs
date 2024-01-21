using FacilitEase.Models.ApiModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public void ChangePriority(int ticketId, int newPriorityId)
        {
            var ticket = _unitOfWork.Tickets.GetById(ticketId);
            if (ticket == null)
            {
                throw new InvalidOperationException($"Ticket with ID {ticketId} not found.");
            }
            else
            {
                ticket.PriorityId = newPriorityId;
            }
            _unitOfWork.Complete();
        }

        public IEnumerable<ManagerEmployeeTickets> GetApprovalTicket(int managerId)
        {
            return _unitOfWork.Tickets.GetApprovalTicket(managerId);

        }

        public IEnumerable<ManagerEmployeeTickets> GetTicketByManager(int managerId)
        {
            return _unitOfWork.Tickets.GetTicketByManager(managerId);
        }

        public void SendForApproval(int ticketId, int managerId)
        {
            var ticket = _unitOfWork.Tickets.GetById(ticketId);
            if (ticket == null)
            {
                throw new InvalidOperationException($"Ticket with ID {ticketId} not found.");
            } 
            else
            {
                var manager = _unitOfWork.Employees.GetById(managerId);
                if (manager?.ManagerId != null)
                {
                    ticket.ControllerId = manager.ManagerId;
                }
                else
                {
                    throw new InvalidOperationException("ManagerId is null or invalid.");
                }
            }
            _unitOfWork.Complete();
            

        }

        public void TicketDecision(int ticketId, int statusId)
        {
            var ticket = _unitOfWork.Tickets.GetById(ticketId);
            if (ticket == null)
            {
                throw new InvalidOperationException($"Ticket with ID {ticketId} not found.");
            }
            else 
            { 
                ticket.StatusId = statusId;
            }
            _unitOfWork.Complete();


        }

        public ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId)
        {
            return _unitOfWork.Tickets.ViewTicketDetails(ticketId);
        }
    }
}
