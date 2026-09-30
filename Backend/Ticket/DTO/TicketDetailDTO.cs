using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Ticket.DTO
{
    public class TicketDetailDTO
    {
        public string Title {get;set;}
        public string Description {get;set;}
        public string Category {get;set;}
        public int Priority {get;set;}
        public DateTime DateIn {get;set;}
        public string UserId {get;set;}
        public string? WorkerId{get;set;}
        public List<string> Images {get;set;}
        public List<MessageDTO> Messages {get;set;} = new();
        public string Progress{get;set;}
        public string? WorkerName {get;set;}
    }
}