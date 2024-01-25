using FacilitEase.Repositories;

namespace FacilitEase.UnitOfWork
{
    public interface IUnitOfWork
    {
        IL3AdminRepository Ticket { get; }

        IDepartmentRepository Department { get; }
        int Complete();
        void Dispose();
    }
}
