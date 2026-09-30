using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Interface;
using Ticket.Models;

namespace Ticket.Repository
{
    public class WorkerRepository : IWorker
    {
        private readonly TicketDbContext _db;
        public WorkerRepository(TicketDbContext db)
        {
            _db = db;
        }
        public async Task<List<User>?> GetWorkers(string specialty)
        {
            return await _db.Users.Where(x=>x.IsActive && x.Role == "Worker" && x.Specialty == specialty && x.IsAvailable).ToListAsync();
        }
    }
}