using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class L3AdminRepository : Repository<TBL_TICKET>, IL3AdminRepository
    {
        public L3AdminRepository(AppDbContext context) : base(context)
        {
        }
    }
}