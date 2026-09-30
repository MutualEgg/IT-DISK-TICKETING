using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace Ticket.DTO
{
    public class TicketDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(30, ErrorMessage = "Title cannot exceed 30 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [Range(1,4)]
        public int Priority { get; set; }
        public string? WorkerId{get;set;}
        public string? TicketId{get;set;}
    }
}