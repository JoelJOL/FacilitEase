using FacilitEase.Services;
using System;
using System.Threading.Tasks;
using FacilitEase.Data;
using FacilitEase.Repositories;

public class UnitOfWork : IUnitOfWork
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
            Employees = new EmployeeRepository(_context);
            Tickets = new TicketRepository(_context);
            Department = new DepartmentRepository(_context);
        }

        public IDepartmentRepository Departments { get; set; }
        public ICategoryRepository Category { get; set; }   
        public IPriorityRepository Priority { get; set; }
        public ITicketRepository Ticket { get; set; }
        public IDocumentRepository Document { get; set; }
        public IUserRepository User { get; set; }
        public IStatusRepository Status { get; set; }
        public IEmployeeRepository Employee { get; set; }
        public IEmployeeRepository Employees { get; }
        public ITicketRepository Tickets { get; }
        public IDepartmentRepository Department { get; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}