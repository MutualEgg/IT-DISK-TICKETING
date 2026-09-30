using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Interface;
using Ticket.Models;

namespace Ticket.Repository
{
    public class TicketRepository : ITicket
    {
        private readonly TicketDbContext _db;
        public TicketRepository(TicketDbContext db)
        {
            _db = db;
        }
        public async Task AddTicket(TicketToken ticket)
        {
            await _db.Tickets.AddAsync(ticket);
        }
        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
        public async Task<List<TicketToken>> GetTicketHistory(string id,string? cat)
        {
            if (string.IsNullOrEmpty(cat))
            {
                return await _db.Tickets.Include(x=>x.ImageURLNavigation).Include(x=>x.MessageNavigation)
                .Where(x=>x.UserId == id && x.IsActive).ToListAsync();
            }
            return await _db.Tickets.Include(x=>x.ImageURLNavigation).Include(x=>x.MessageNavigation)
            .Where(x=>x.UserId == id && x.IsActive && x.Category.StartsWith(cat)).ToListAsync();
            
        }
        public async Task<TicketToken?> GetTicketById(string id)
        {
            return await _db.Tickets
            .Include(x=>x.MessageNavigation)
            .FirstOrDefaultAsync(x=>x.Id == id && x.IsActive);
        }
        public async Task<List<TicketToken>?> GetAllTicketForAdmin(string? cat)
        {
            if (string.IsNullOrEmpty(cat))
            {
                return await _db.Tickets.Where(x=>x.IsActive && x.Progress !="finished").OrderByDescending(c=>c.Priority).ThenBy(x=>x.DateIn).ToListAsync();
            }
            return await _db.Tickets.Where(x=>x.IsActive && x.Progress !="finished" && x.Category.StartsWith(cat)).OrderByDescending(x=>x.Priority).ThenBy(x=>x.DateIn).ToListAsync();
        }
        public async Task<List<TicketToken>> GetTicketHistoryAdmin(string? cat)
        {
            if (string.IsNullOrEmpty(cat))
            {
                return await _db.Tickets.Where(x=>x.IsActive && x.Progress =="finished").OrderBy(x=>x.DateOut).ToListAsync();
            }
            return await _db.Tickets.Where(x=>x.IsActive && x.Progress == "finished" && x.Category.StartsWith(cat)).OrderBy(x=>x.DateOut).ToListAsync();
        }
        public async Task<List<TicketToken>?> GetTicketByWorkerId(string id)
        {
            return await _db.Tickets.Where(x=>x.IsActive && x.Progress !="finished" && x.WorkerId == id).Include(x=>x.MessageNavigation)
            .OrderBy(x=>x.Priority).ThenBy(x=>x.DateIn).ToListAsync();
        }
        public async Task<List<TicketToken>?> GetTicketHistoryWorker(string id)
        {
            return await _db.Tickets.Where(x=>x.IsActive && x.WorkerId == id && x.Progress=="finished").OrderBy(x=>x.DateOut).ToListAsync();
        }
        public async Task<List<TicketToken>?> GetTicketHistoryUser(string id,string? cat)
        {
            if (string.IsNullOrEmpty(cat))
            {
                return await _db.Tickets.Where(x=>x.IsActive && x.Progress == "finished" && x.UserId == id).OrderBy(x=>x.DateOut).ToListAsync();
            }
            return await _db.Tickets.Where(x=>x.Category.StartsWith(cat) && x.IsActive && x.Progress == "finished" && x.UserId == id).OrderBy(x=>x.DateOut).ToListAsync();
        }
    }
}