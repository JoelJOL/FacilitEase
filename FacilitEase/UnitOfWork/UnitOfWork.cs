using FacilitEase.Services;
using System;
using System.Threading.Tasks;
using FacilitEase.Data;
using FacilitEase.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Employees = new EmployeeRepository(_context);
        Tickets = new TicketRepository(_context);
    }

    public IEmployeeRepository Employees { get; }
    public ITicketRepository Tickets { get; }

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