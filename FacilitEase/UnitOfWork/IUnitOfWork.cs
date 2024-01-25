using FacilitEase.Repositories;

namespace FacilitEase.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDepartmentRepository Departments { get; }
        ICategoryRepository Category { get; }
        ITicketRepository Ticket { get; }
        IPriorityRepository Priority { get; }
        IDocumentRepository Document { get; }
        IUserRepository User { get; }
        IStatusRepository Status { get; }
        IEmployeeRepository Employee { get; }

        int Complete();
        void Dispose();
    }
}
