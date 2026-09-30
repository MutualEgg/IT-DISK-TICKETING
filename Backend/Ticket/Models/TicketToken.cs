namespace Ticket.Models
{
    public class TicketToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string? WorkerId {get;set;}
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; } = true;
        public string Progress { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }
        public virtual ICollection <ImageURL> ImageURLNavigation { get; set; } = new List<ImageURL>();
        public virtual ICollection <Message> MessageNavigation {get;set;} = new List<Message>();
    }
}
