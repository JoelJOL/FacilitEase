using FacilitEase.Repositories;

namespace FacilitEase.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository Departments { get; }
        ICategoryRepository Category { get; }
        ITicketRepository Ticket { get; }
        IPriorityRepository Priority { get; }
        IDocumentRepository Document { get; }
        IUserRepository User { get; }
        IStatusRepository Status { get; }
        IEmployeeRepository Employee { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IL3AdminRepository L3Admin { get; }
        ITicketRepository TicketRepository { get; }
        IEmployeeDetailRepository EmployeeDetailRepository { get; }

        INotificationRepository Notification { get; set; }

       ILocationRepository Location { get; }
        IPositionRepository Position { get; }
        Task<int> CompleteAsync();
        int Complete();
        void Dispose();
    }
}
