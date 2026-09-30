using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Models;

namespace Ticket.DTO
{
    public class TicketAssignDTO
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }
        public string? WorkerId{get;set;}
        public string? TicketId{get;set;}
        public List<bool> Messages {get;set;} = new();
        public DateTime? DateIn {get;set;}
    }
}