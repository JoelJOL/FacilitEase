using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class StatusRepository : Repository<TBL_STATUS>, IStatusRepository
    {
        public StatusRepository(AppDbContext context) : base(context) { }
    }
}
