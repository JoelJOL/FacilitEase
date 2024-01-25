namespace FacilitEase.UnitOfWork
{
    public interface IUnitOfWork
    {
        int Complete();
        void Dispose();
    }
}
