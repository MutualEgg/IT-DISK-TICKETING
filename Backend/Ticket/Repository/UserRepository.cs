using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Interface;
using Ticket.Models;
namespace Ticket.Repository
{
    public class UserRepository : IUser
    {
        private readonly TicketDbContext _db;
        public UserRepository(TicketDbContext db)
        {
            _db = db;
        }

        public async Task AddUser(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public async Task<User?> FindUserById(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<User?> FindUserByUsername(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(x=>x.Username == username);
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
        public async Task<User?> FindUserByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x=>x.Email == email);
        }
    }
}
