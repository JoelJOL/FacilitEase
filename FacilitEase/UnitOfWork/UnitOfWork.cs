using FacilitEase.Data;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;
using System.Diagnostics;

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
        EmployeeDetailRepository = new EmployeeDetailRepository(_context);
            Status = new StatusRepository(_context);
            Employees = new EmployeeRepository(_context);
            Tickets = new TicketRepository(_context);
            Department = new DepartmentRepository(_context);
            TicketRepository = new TicketRepository(_context);
            Location = new LocationRepository(_context);
            Position = new PositionRepository(_context);
        Notification = new NotificationRepository(_context);


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
    public INotificationRepository Notification { get; set; }
    public IEmployeeDetailRepository EmployeeDetailRepository { get; set; }
    public ITicketRepository Tickets { get; }
    public IDepartmentRepository Department { get; }
    public ILocationRepository Location { get; set; }
    public IPositionRepository Position { get; set; }
    public IEmployeeRepository EmployeeRepository { get; set; }
    public ITicketRepository TicketRepository { get; set; }
    public IL3AdminRepository L3Admin { get; set; }

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

    public void Dispose()
    {
        _context.Dispose();
    }
}