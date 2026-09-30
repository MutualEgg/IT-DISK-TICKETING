using Ticket.Models;

namespace Ticket.Interface
{
    public interface IRefreshForRepo
    {
        Task AddRefreshToken(RefreshToken token);
        Task <RefreshToken?> GetRefreshTokenByToken(string token);
        Task <RefreshToken?> GetRefreshTokenByUserId(string userid);
    }
}
