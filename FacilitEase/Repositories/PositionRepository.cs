using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FacilitEase.Repositories
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(AppDbContext context) : base(context)
        {
        }

        // Implement specific methods for positions if needed
        public IEnumerable<Position> GetByName(string positionName)
        {
            return Context.Set<Position>().Where(p => p.PositionName == positionName).ToList();
        }
    }
}
