using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Ticket.DTO;
using Ticket.Interface;
using Ticket.Models;

namespace Ticket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController :ControllerBase
    {
        private readonly IMessage _messagerepo;
        private readonly ITicket _ticketrepo;
        public MessageController(IMessage messagerepo,ITicket ticketrepo)
        {
            _messagerepo = messagerepo;
            _ticketrepo = ticketrepo;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostMessage(MessageDTO messagedto)
        {
            //ambil ticket yang mau di kirim pesan
            var ticket = await _ticketrepo.GetTicketById(messagedto.TicketId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)){
                return Unauthorized();}
            if(ticket == null)
            {
                return NotFound(new{message ="ticket not found, try to create a new ticket"});
            }
            //hanya user dan worker yang diassign yang bisa melakukan message dalam tiket tersebut
            if(userId != ticket.UserId && userId != ticket.WorkerId)
            {
                return StatusCode(403,"sorry you cant post message as other pseron");
            }
            bool user;
            //jika yang melakukan postmessage adalah user, maka set user = true, else false
            if(userId == ticket.UserId)
            {
                user = true;
            }
            else
            {
                user = false;
            }
            var message = new Message
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = ticket.Id,
                SenderId = userId,
                //karna tadi kita sudah memeriksa bahwa jika user yang login, maka set user = true, namun kalau 
                //worker yang login, maka user = false.
                //untuk di frontend, kkita akan mengambil informasi ini utnuk menghitung notifikasi
                //tujuannya adalah jika user yang post message maka tidak perlu notifikasi, tpi worker lah yang mendapat notifikasi
                //begitu juga dengan sebaliknya
                IsReadByUser = user,
                IsReadByWorker = !user,
                message = messagedto.message,
                DateIn = DateTime.UtcNow
            };
            await _messagerepo.AddMessage(message);
            await _messagerepo.SaveChanges();
            return Ok(new{message_message = message.message});

        }
    }
}