using FacilitEase.Data;

namespace FacilitEase.UnitOfWork
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfwork(AppDbContext context)
        {
            _context = context;
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
