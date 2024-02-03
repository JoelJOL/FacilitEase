using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class PriorityRepository : Repository<Priority>, IPriorityRepository
    {
        public PriorityRepository(AppDbContext context) : base(context) { }
    }
}
