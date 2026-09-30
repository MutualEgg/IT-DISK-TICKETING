using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using Ticket.Data;
using Ticket.Interface;
using Ticket.Models;

namespace Ticket.Repository
{
    public class RefreshTokenRepository : IRefreshForRepo
    {
        private readonly TicketDbContext _db;
        public RefreshTokenRepository(TicketDbContext db)
        {
            _db = db;
        }
        public async Task AddRefreshToken(RefreshToken token)
        {
           await _db.RefreshTokens.AddAsync(token);
        }
        public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(x=>x.Token == token);
        }

        public async Task<RefreshToken?> GetRefreshTokenByUserId(string userid)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(x=>x.UserId == userid);
        }
    }
}
