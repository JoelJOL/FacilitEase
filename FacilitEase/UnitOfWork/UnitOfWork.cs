using FacilitEase.Data;
using FacilitEase.Repositories;

namespace FacilitEase.UnitOfWork
{
    public class IUnitOfwork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUnitOfwork(AppDbContext context)
        {
            _context = context;
            Departments = new DepartmentRepository(_context);
            Category = new CategoryRepository(_context);
            Priority = new PriorityRepository(_context);
            Ticket = new TicketRepository(_context);
            Document = new DocumentRepository(_context);
            User = new UserRepository(_context);
            Employee = new EmployeeRepository(_context);
            Status = new StatusRepository(_context);
        }

        public IDepartmentRepository Departments { get; set; }
        public ICategoryRepository Category { get; set; }   
        public IPriorityRepository Priority { get; set; }
        public ITicketRepository Ticket { get; set; }
        public IDocumentRepository Document { get; set; }
        public IUserRepository User { get; set; }
        public IStatusRepository Status { get; set; }
        public IEmployeeRepository Employee { get; set; }
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
