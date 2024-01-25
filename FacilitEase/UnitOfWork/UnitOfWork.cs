using FacilitEase.Services;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using FacilitEase.Data;
using FacilitEase.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            EmployeeRepository = new EmployeeRepository(_context);
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
            TicketRepository = new TicketRepository(_context);
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
        public IEmployeeRepository EmployeeRepository { get; set; }
        public ITicketRepository TicketRepository { get; set; }

        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception details or print to console for debugging
                Debug.WriteLine($"Error in Complete: {ex.Message}");
                // Log or print the inner exception details
                Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception details or print to console for debugging
                Console.WriteLine($"Error in CompleteAsync: {ex.Message}");
                // Log or print the inner exception details
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
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