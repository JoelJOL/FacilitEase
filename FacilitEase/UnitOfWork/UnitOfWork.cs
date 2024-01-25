using FacilitEase.Data;
using FacilitEase.Repositories;

namespace FacilitEase.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
      
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Ticket = new L3AdminRepository(_context);
            Department = new DepartmentRepository(_context);
            Employee = new EmployeeRepository(_context);
        }

        public IL3AdminRepository Ticket { get; }
        public IDepartmentRepository Department { get; }
        public IEmployeeRepository Employee { get; }
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
