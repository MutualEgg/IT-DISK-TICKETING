using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.DTO;
using Ticket.Models;

namespace Ticket.Interface
{
    public interface ITicket
    {
        Task AddTicket(TicketToken ticket);   
        Task SaveChanges();
        Task<List<TicketToken>> GetTicketHistory(string id,string? cat);
        Task<TicketToken?> GetTicketById(string id);
        Task<List<TicketToken>?> GetAllTicketForAdmin(string cat);
        Task<List<TicketToken>?> GetTicketHistoryAdmin(string cat);
        Task<List<TicketToken>?> GetTicketByWorkerId(string id);
        Task<List<TicketToken>?> GetTicketHistoryWorker(string id);
        Task <List<TicketToken>?> GetTicketHistoryUser(string id,string? cat);
    }
}