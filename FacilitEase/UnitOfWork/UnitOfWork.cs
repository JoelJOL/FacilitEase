// UnitOfWork.cs
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
        TicketRepository = new TicketRepository(_context);
    }
    public ITicketRepository TicketRepository { get; set; }
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
