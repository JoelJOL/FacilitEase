using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class PositionRepository : Repository<TBL_POSITION>, IPositionRepository
    {
        public PositionRepository(AppDbContext context) : base(context)
        {
        }

        // Implement specific methods for positions if needed
        public IEnumerable<TBL_POSITION> GetByName(string positionName)
        {
            return Context.Set<TBL_POSITION>().Where(p => p.PositionName == positionName).ToList();
        }
    }
}