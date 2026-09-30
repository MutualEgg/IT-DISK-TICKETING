namespace Ticket.Models
{
    public class User
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; } = true;
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public virtual RefreshToken? Refreshtoken { get; set; }
        public string? Specialty {get;set;}
        public bool IsAvailable{get;set;} = true;
    }
}
