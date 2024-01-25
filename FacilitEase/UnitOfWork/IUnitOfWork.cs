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
        IL3AdminRepository Ticket { get; }
        ITicketRepository TicketRepository { get; }
        Task<int> CompleteAsync();
        int Complete();
        void Dispose();
    }
}
