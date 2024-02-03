using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        public StatusRepository(AppDbContext context) : base(context) { }
    }
}
