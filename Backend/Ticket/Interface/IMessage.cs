using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Models;

namespace Ticket.Interface
{
    public interface IMessage
    {
        Task AddMessage(Message message);
        Task SaveChanges();
    }
}