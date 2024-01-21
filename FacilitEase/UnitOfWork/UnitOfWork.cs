// UnitOfWork.cs
using FacilitEase.Data;
using FacilitEase.Repositories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FacilitEase.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            EmployeeRepository = new EmployeeRepository(_context);
        }

        public IEmployeeRepository EmployeeRepository { get; set; }

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
}
