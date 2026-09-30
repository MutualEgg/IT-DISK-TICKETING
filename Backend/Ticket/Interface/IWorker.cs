using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Models;

namespace Ticket.Interface
{
    public interface IWorker
    {
        Task<List<User>?> GetWorkers(string specialty);
    }
}