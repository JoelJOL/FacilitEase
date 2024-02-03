using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class PriorityRepository : Repository<TBL_PRIORITY>, IPriorityRepository
    {
        public PriorityRepository(AppDbContext context) : base(context)
        {
        }
    }
}