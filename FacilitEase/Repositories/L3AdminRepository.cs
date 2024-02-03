using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace FacilitEase.Repositories
{
    public class L3AdminRepository : Repository<Ticket>, IL3AdminRepository
    {
        public L3AdminRepository(AppDbContext context) : base(context) { }
       

    }
}
