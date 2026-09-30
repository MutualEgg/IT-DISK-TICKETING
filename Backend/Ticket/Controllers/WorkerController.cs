using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket.DTO;
using Ticket.Interface;
using Ticket.Models;

namespace Ticket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController :ControllerBase
    {
        private readonly IWorker _workrepo;
        private readonly ITicket _ticketrepo;
        private readonly IUser _userrepo;
        public WorkerController(IWorker workrepo,ITicket ticketrepo,IUser userrepo)
        {
            _workrepo = workrepo;
            _ticketrepo = ticketrepo;
            _userrepo = userrepo;
        }
        [HttpGet("{cat}")]
        [Authorize(Roles ="Admin,Worker")]
        public async Task<IActionResult> GetWorkers(string cat)
        {
            //mengambil worker berdasarkan specialtynya untuk di display di problem ticket dengan specialty itu
            var workers = await _workrepo.GetWorkers(cat);
            var cat_worker = workers.Select(t=> new
            {
                Id = t.Id,
                name = t.Username
            }).ToList();
            return Ok(new
            {
            worker_list = cat_worker
            });
        }
        [HttpGet("task")]
        [Authorize(Roles ="Worker,Admin")]
        public async Task<IActionResult> GetTask()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new
                {
                    Message="string is empty"
                });
            }
            //menampilkan task worker dari ticket yang di assign
            List<TicketToken?> ticket = await _ticketrepo.GetTicketByWorkerId(userId);
            var tickets = ticket.Select(t=>new
            {
                Title = t.Title,
                Description = t.Description,
                Category = t.Category,
                Priority = t.Priority,
                WorkerId = t.WorkerId,
                TicketId = t.Id,
                Messages = t.MessageNavigation.Select(c=>c.IsReadByWorker).ToList()
            }).ToList();
            return Ok(new
            {
                task_tickets = tickets
            });
        }
        [HttpPut("finish/{id}")]
        [Authorize(Roles ="Worker")]
        public async Task<IActionResult> FinishATask(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TicketToken ticket = await _ticketrepo.GetTicketById(id);
            if (ticket == null)
            {
                return BadRequest(new
                {
                    Message = "Ticket not found"
                });
            }
            User worker = await _userrepo.FindUserById(userId);

            if(worker == null)
            {
                return BadRequest(new
                {
                    Message ="sorry theres no worker"
                });
            }
            //versi kedua adalah ketika admin assign ticket ke worker, maka worker.isavailable = false
            //ini akan membuat admin untuk tidak bisa memberikan ticket ke worker itu lagi
            worker.IsAvailable = true;
            //ketika sudah selesai, worker.isavalaible = true
            if(ticket == null)
            {
                return BadRequest(new
                {
                    Message="error"
                });
            }
            ticket.Progress = "finished";
            ticket.DateOut = DateTime.UtcNow;
            await _ticketrepo.SaveChanges();
            return Ok(new
            {
                ticket=ticket.Progress
            });
        }
        [HttpGet("tickethistory")]
        [Authorize(Roles ="Worker")]
        public async Task<IActionResult> GetTicketHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            //ambil ticket only yang progressnya finished
            List<TicketToken> tickettokens = await _ticketrepo.GetTicketHistoryWorker(userId);
            List<TicketAssignDTO> tickets = tickettokens.Select(t=>new TicketAssignDTO
            {
                Title = t.Title,
                Description = t.Description,
                Category = t.Category,
                Priority = t.Priority,
                TicketId = t.Id,
                WorkerId = t.WorkerId,
                DateIn = t.DateIn
            }).ToList();
            return Ok(new
            {
                tickethist = tickets
            });
        }
    }
}