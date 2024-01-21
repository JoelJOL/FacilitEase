using FacilitEase.Repositories;

namespace FacilitEase.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ITicketRepository Tickets { get; }
        public IEmployeeRepository Employees { get; }
        int Complete();
        void Dispose();
    }
}
