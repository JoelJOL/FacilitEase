// IUnitOfWork.cs
using FacilitEase.Repositories;
using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    int Complete();
    Task<int> CompleteAsync();

    ITicketRepository TicketRepository { get; }

    void Dispose();
}
