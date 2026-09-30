using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Ticket.Models;

namespace Ticket.Data
{
    public class TicketDbContext :DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
        {
        }
        public DbSet <User> Users { get; set; }
        public DbSet <Password> Passwords { get; set; }
        public DbSet <TicketToken> Tickets { get; set; }
        public DbSet <ImageURL> ImageURLS { get; set; }
        public DbSet <RefreshToken> RefreshTokens { get; set; }
        public DbSet<Message> Messages {get;set;}
    }
}
