using Ticket.Models;

namespace Ticket.Interface
{
    public interface IPassword
    {
        Task AddPassword(Password password);
        Task<Password?> GetPassword(string userid);
    }

}
