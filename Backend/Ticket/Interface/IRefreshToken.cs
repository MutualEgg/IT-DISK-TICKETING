namespace Ticket.Interface
{
    public interface IRefreshToken
    {
        string CreateAccessToken(string Id, string username, string role);
        string CreateRefreshToken();
    }
}
