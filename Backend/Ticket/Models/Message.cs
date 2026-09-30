using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Models
{
    public class Message
    {
        public string Id {get;set;}
        public string SenderId {get;set;}
        public string TicketId {get;set;}
        public bool IsReadByWorker {get;set;} = false;
        public bool IsReadByUser {get;set;} = false;
        public string message {get;set;}
        public DateTime DateIn {get;set;}
        public virtual TicketToken Ticket {get;set;}
        
    }
}