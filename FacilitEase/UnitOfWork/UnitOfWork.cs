using FacilitEase.Data;
using FacilitEase.Repositories;

namespace FacilitEase.UnitOfWork
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfwork(AppDbContext context)
        {
            _context = context;
            Departments = new DepartmentRepository(_context);
            Category = new CategoryRepository(_context);
            Priority = new PriorityRepository(_context);
            Ticket = new TicketRepository(_context);
        }

        public IDepartmentRepository Departments { get; set; }
        public ICategoryRepository Category { get; set; }   
        public IPriorityRepository Priority { get; set; }
        public ITicketRepository Ticket { get; set; }   
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
