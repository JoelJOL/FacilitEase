using FacilitEase.Data;
using FacilitEase.Repositories;
using FacilitEase.Services;

namespace FacilitEase.UnitOfWork
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfwork(AppDbContext context)
        {
            _context = context;
            Tickets = new TicketRepository(_context);
            Employees = new EmployeeRepository(_context);
        }
        public ITicketRepository Tickets { get; }

        public IEmployeeRepository Employees { get; }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
