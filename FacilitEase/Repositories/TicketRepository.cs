using FacilitEase.Models.ApiModels;
using System.Reflection.Metadata;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
﻿using FacilitEase.Data;
using FacilitEase.Models.EntityModels;


namespace FacilitEase.Repositories
{
    public class TicketRepository : Repository<TicketApiModel>, ITicketRepository,Repository<TBL_TICKET>
    {
        private readonly AppDbContext _context;
        public TicketRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}


