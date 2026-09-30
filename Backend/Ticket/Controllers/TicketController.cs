using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class TicketController :ControllerBase
    {
        private readonly ITicket _ticketreppo;
        private readonly IUser _userrepo;

        public TicketController(ITicket ticketreppo,IUser userrepo)
        {
            _ticketreppo=ticketreppo;
            _userrepo = userrepo;
        }
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateTicket(TicketDTO ticket)
        {
            //self-explanatory
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized(new
                {
                    message="failed, try to relog or smth"
                });
            }
            var ticketId = Guid.NewGuid().ToString();
            TicketToken newticket = new TicketToken
            {
                Id = ticketId,
                UserId = userIdString,
                Title = ticket.Title,
                Description = ticket.Description,
                Category = ticket.Category,
                Priority = ticket.Priority,
                IsActive = true,
                Progress = "pending",
                DateIn = DateTime.UtcNow
            };
            await _ticketreppo.AddTicket(newticket);
            await _ticketreppo.SaveChanges();
            return Ok(new
            {
                message="succesful",
                ticketid = newticket.Id
            });

        }
        [HttpGet("loadhistory/{cat?}")]
        [Authorize]
        public async Task<IActionResult> GetTicketHistory(string? cat)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    message="failed, try to relog or smth"
                });
            }
            //ambil ticket yang progressnya != finished
            //ambil ticket yang sudah finished, kalau cat == null, maka ambil semua ticket. tpi kalau category memiliki isi seperti "Network" Maka hanya ambil ticket yang bercategory Network yang sudah finished
            List<TicketToken> tickets = await _ticketreppo.GetTicketHistory(userId,cat);
            //kita pisah ticketnya menjadi ongoing dan pending(ongoing artinya sedang dikerjakan worker, pending artinnya belum diassign admin)
            var ongoingresult = tickets
            .Where(t=>t.Progress =="ongoing")
            .Select(t => new
            {
                    id = t.Id,
                    title = t.Title,
                    description = t.Description,
                    category = t.Category,
                    priority = t.Priority,
                    progress = t.Progress,
                    dateIn = t.DateIn,
                    //diini versinya adalah kita juga menampilkan image, tpi maybe in next update
                    images = t.ImageURLNavigation
                        .Where(i => i.IsActive)
                        .Select(i => i.URL)
                        .ToList(),
                    messages = t.MessageNavigation.Select(x=>x.IsReadByUser).ToList()
                }).OrderBy(x=>x.dateIn)
                .ToList();    
            var pendingresult = tickets
            .Where(t=>t.Progress =="pending")
            .Select(t => new
            {
                    id = t.Id,
                    title = t.Title,
                    description = t.Description,
                    category = t.Category,
                    priority = t.Priority,
                    progress = t.Progress,
                    dateIn = t.DateIn,
                    images = t.ImageURLNavigation
                        .Where(i => i.IsActive)
                        .Select(i => i.URL)
                        .ToList(),
                    messages = t.MessageNavigation.Select(x=>x.IsReadByUser).ToList()
                }).OrderBy(x=>x.dateIn)
                .ToList();    
                return Ok(new { ongoing_ticket_list = ongoingresult,
                                pending_ticket_list = pendingresult});
        }
        [HttpGet("userhistory/{cat?}")]
        [Authorize]
        public async Task<IActionResult> GetUserFinishedTicket(string? cat)
        {
            //ambil ticket yang sudah finished, kalau cat == null, maka ambil semua ticket. tpi kalau category memiliki isi seperti "Network" Maka hanya ambil ticket yang bercategory Network yang sudah finished
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            List<TicketToken> TicketTokens = await _ticketreppo.GetTicketHistoryUser(userId,cat);
            List<TicketAssignDTO> ticketassign = TicketTokens.Select(t=>new TicketAssignDTO
            {
                Title = t.Title,
                Description = t.Description,
                Category = t.Category,
                Priority = t.Priority,
                WorkerId = t.WorkerId,
                TicketId = t.Id,
                DateIn = t.DateIn
            }).ToList();
            return Ok(new
            {
                ticket_detail = ticketassign
            });
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTicketDetail(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            TicketToken ticket = await _ticketreppo.GetTicketById(id);
            if(ticket == null)
            {
                return NotFound(new { message = $"Ticket with id {id} not found" });
            }
            if(ticket.UserId != userId && ticket.WorkerId != userId && userRole !="Admin")
            {
                return StatusCode(403, new { message = "forbidden to see other ticket" });
            }
            var workerusername = "";
            if(ticket.WorkerId!= null)
            {
                var worker = await _userrepo.FindUserById(ticket.WorkerId);
                workerusername = worker?.Username ?? "";
            }
            //kita kembalikan ticket dengan detail + messagesnya
            var newticket = new TicketDetailDTO
            {
                Title = ticket.Title,
                Description = ticket.Description,
                Category = ticket.Category,
                Priority = ticket.Priority,
                DateIn = ticket.DateIn,
                UserId  = ticket.UserId,
                WorkerId = ticket.WorkerId,
                Progress = ticket.Progress,
                WorkerName = workerusername,
                Messages = ticket.MessageNavigation.Select(m=> new MessageDTO
                {                    
                    SenderId = m.SenderId,
                    TicketId = ticket.Id,
                    IsReadByUser = m.IsReadByUser,
                    IsReadByWorker = m.IsReadByWorker,
                    message = m.message,
                    DateIn = m.DateIn
                }).OrderBy(x=>x.DateIn)
                .ToList()
            };
            return Ok(new {ticket_detail = newticket});
        }
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTicket(string id)
        {
            //soft delete dengan cara isactive = false
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            TicketToken ticket = await _ticketreppo.GetTicketById(id);
            if(userId != ticket.UserId)
            {
                return StatusCode(403,"you cant delete other ticket");
            }
            ticket.IsActive = false;
            await _ticketreppo.SaveChanges();
            return Ok();
        }
        [HttpGet("alltickets/{cat?}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllTicketForAdmin(string? cat)
        {
            //khusus admin, berarti kita ambil semua ticket tnapa id. sedangkan kalau endpoint user
            //kita ambil semua ticket berdasarkan id user aja
            var tickets = await _ticketreppo.GetAllTicketForAdmin(cat);
            var tickets_pending  = tickets.Where(t=>t.Progress =="pending")
            .Select(t=> new 
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Category = t.Category,
                Priority = t.Priority,
                DateIn = t.DateIn
            }).ToList();
            var tickets_ongoing = tickets.Where(t=>t.Progress=="ongoing")
            .Select(t=> new 
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Category = t.Category,
                Priority = t.Priority,
                DateIn = t.DateIn
            }).ToList();
            return Ok(new{ticket_pending = tickets_pending,
                            ticket_ongoing = tickets_ongoing});
        }
        [HttpPost("assignticket")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AssignTicket(TicketAssignDTO request)
        {
            //kita isi worker id di ticker user
            var ticket = await _ticketreppo.GetTicketById(request.TicketId);
            var userid =  await _userrepo.FindUserById(request.WorkerId);
            //userid.IsAvailable = false;
            if(ticket == null)
            {
                return NotFound(new{message="error not found"});
            }
            ticket.Title = request.Title;
            ticket.Description = request.Description;
            ticket.Category = request.Category;
            ticket.Progress ="ongoing";
            ticket.Priority = request.Priority;
            ticket.WorkerId = request.WorkerId;
            await _ticketreppo.SaveChanges();
            return Ok(new{Message="update successfuly"});
        }
        [HttpDelete("admindelete/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteTicketForAdmin(string id)
        {

            //self explanatory
            var ticket = await _ticketreppo.GetTicketById(id);
            if(ticket == null)
            {
                return BadRequest(new{Message="sorry theres an error, try to contact the company"});
            }
            ticket.IsActive = false;
            await _ticketreppo.SaveChanges();
            return Ok(new{Message="success"});
        }
        [HttpGet("adminhistory/{cat?}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AdminTicketHistory(string?cat)
        {
            //cek history yang progress == finished
            List<TicketToken> histtickets = await _ticketreppo.GetTicketHistoryAdmin(cat);
            List<TicketAssignDTO> ticket_hist = histtickets
            .Select(t=> new TicketAssignDTO
            {
                Title = t.Title,
                Description = t.Description,
                Category = t.Category,
                Priority = t.Priority,
                WorkerId = t.WorkerId,
                TicketId = t.Id,
                DateIn = t.DateIn
            }).ToList();
            return Ok(new{ticket_history = ticket_hist});
        }
        [HttpPut("read/{id}")]
        [Authorize]
        public async Task<IActionResult> CheckAsRead(string id)
        {
            //setiap kali kita open ticket detail, kita check asread messagenya
            //jika user yang open ticketdetail maka isreadbyuser = true sedangkan worker = tetap false/tetap true
            //jika worker yang open maka isreadbyworker = false, dan isreadbyuser tetap false atau tetap true berdasarkan past
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TicketToken ticket = await _ticketreppo.GetTicketById(id);
            bool user;
            if(userId == ticket.UserId)
            {
                user = true;
            }
            else
            {
                user = false;
            }
            foreach(var message in ticket.MessageNavigation)
            {
                if(message.IsReadByUser == false)
                {
                    message.IsReadByUser = user;
                }
                if(message.IsReadByWorker == false)
                {
                    message.IsReadByWorker = !user;
                }
                
            }
            await _ticketreppo.SaveChanges();
            return Ok(new
            {
                Message="done"
            });
        }
    }
}