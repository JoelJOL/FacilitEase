using System;

namespace FacilitEase.UnitOfWork
{
    public interface UnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Employees = new EmployeeRepository(_context);
            City = new CityRepository(_context);
            Designation = new DesignationRepository(_context);

            // Initialize other repositories.
        }

        public IEmployeeRepository Employees { get; }
        public ICityRepository City { get; }

        public IDesignationRepository Designation { get; }

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
