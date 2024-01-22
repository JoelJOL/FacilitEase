using FacilitEase.Repositories;

namespace FacilitEase.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDepartmentRepository Departments { get; }
        ICategoryRepository Category { get; }
        ITicketRepository Ticket { get; }   
        IPriorityRepository Priority { get; }
        
        int Complete();
        void Dispose();
    }
}
