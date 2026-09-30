using Ticket.DTO;
using Ticket.Models;
namespace Ticket.Interface
{
    public interface IUser
    {
        Task<User> FindUserByUsername(string username);
        Task AddUser(User user);
        Task SaveChanges();
        Task<User?> FindUserById(string id);
        Task<User?> FindUserByEmail(string email);
    }
}
