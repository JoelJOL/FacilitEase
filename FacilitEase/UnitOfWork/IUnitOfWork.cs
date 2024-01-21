namespace FacilitEase.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }
        int Complete();
        void Dispose();
    }
}
