namespace Ticket.Models
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime RefreshTokenExp { get; set; }
    }
}
