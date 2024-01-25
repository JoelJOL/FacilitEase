using FacilitEase.Repositories;
using FacilitEase.Models.ApiModels;
using FacilitEase.Repositories;
using System;
using System.Threading.Tasks;


namespace FacilitEase.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository Departments { get; }
        ICategoryRepository Category { get; }
        ITicketRepository Ticket { get; }
        IPriorityRepository Priority { get; }
        IDocumentRepository Document { get; }
        IUserRepository User { get; }
        IStatusRepository Status { get; }
        IEmployeeRepository Employee { get; }
        IL3AdminRepository Ticket { get; }
        Task<int> CompleteAsync();
        int Complete();
        void Dispose();
    }
}
