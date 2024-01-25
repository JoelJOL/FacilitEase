using FacilitEase.Models.ApiModels;
using FacilitEase.Repositories;
using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    public IEmployeeRepository Employees { get; }
    public ITicketRepository Tickets { get; }
    IL3AdminRepository Ticket { get; }
    IDepartmentRepository Department { get; }
    int Complete();
    Task<int> CompleteAsync();
    void Dispose();
}
