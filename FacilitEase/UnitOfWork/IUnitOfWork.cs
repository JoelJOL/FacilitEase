// IUnitOfWork.cs
using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    int Complete();
    Task<int> CompleteAsync();

    void Dispose();
}
