using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Interface;
using Ticket.Models;

namespace Ticket.Repository
{
    public class PasswordRepository : IPassword
    {
        private readonly TicketDbContext _db;

        public PasswordRepository(TicketDbContext db)
        {
            _db = db;
        }

        public async Task AddPassword(Password password)
        {
            await _db.Passwords.AddAsync(password);
        }
        public async Task<Password?> GetPassword(string userid)
        {
            return await _db.Passwords.FirstOrDefaultAsync(x=>x.UserId == userid);
        }
    }
}
