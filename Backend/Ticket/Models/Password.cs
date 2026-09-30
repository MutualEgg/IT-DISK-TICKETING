namespace Ticket.Models
{
    public class Password
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string? HashedPassword { get; set; }
        public bool? IsActive {get;set;}
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
    }
}
