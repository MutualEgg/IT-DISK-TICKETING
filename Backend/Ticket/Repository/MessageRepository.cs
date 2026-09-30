using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Data;
using Ticket.Interface;
using Ticket.Models;

namespace Ticket.Repository
{
    public class MessageRepository : IMessage
    {
        private readonly TicketDbContext _db;
        public MessageRepository (TicketDbContext db)
        {
            _db = db;
        }

        public async Task AddMessage(Message message)
        {
            await _db.Messages.AddAsync(message);
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}