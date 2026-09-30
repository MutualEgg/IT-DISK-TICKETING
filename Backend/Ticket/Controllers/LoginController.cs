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
    public class LoginController :ControllerBase
    {
        private readonly IPassword _passreppo;
        private readonly IUser _userreppo;
        private readonly IRefreshToken _jwt;
        private readonly IRefreshForRepo _refreshreppo;
        public LoginController(IPassword passreppo, IUser userreppo,IRefreshToken jwt,IRefreshForRepo refreshrepo)
        {
            _passreppo = passreppo;
            _userreppo = userreppo;
            _jwt = jwt;
            _refreshreppo = refreshrepo;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var user = await _userreppo.FindUserByEmail(request.email);
            if(user == null)
            {
                return BadRequest(new{message ="sorry please type in the correct email and password"});
            }
            var password = await _passreppo.GetPassword(user.Id);
            if(password == null)
            {
                return BadRequest(new{message ="sorry please type in the correct email and password"});
            }
            //cek hashing password di db dnegna password yang diinput user
            bool verified = BCrypt.Net.BCrypt.Verify(request.password,password.HashedPassword);
            if (!verified)
            {
                return BadRequest(new{message ="sorry please type in the correct email and password"});
            }
            //membuat accesstoken(exp 15 mins) dan refreshtoken(exp 7 hari)
            //setiap kali user login akun, maka kita akan mereplace refreshtoken user dengan yang baru tanpa membuat new row di database
            var accesstoken = _jwt.CreateAccessToken(user.Id, user.Username, user.Role);
            var refreshtoken = _jwt.CreateRefreshToken();
            //memasukkan accesstoken kedalam cookie agar bisa disimapn cookie
            Response.Cookies.Append("accessToken", accesstoken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });
            //memasukkan refresh token kedalam cookie agar bisa disimapn cookie
            Response.Cookies.Append("refreshToken", refreshtoken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            //kita ambil refreshtoken yang lama berdasarkan id user
            var existing = await _refreshreppo.GetRefreshTokenByUserId(user.Id);
            //harusnya pasti exist karna ini login
            if (existing == null)
            {
                existing = new RefreshToken   // sesuaikan nama class & namespace-mu
                {
                    UserId = user.Id,
                    Token = refreshtoken,
                    RefreshTokenExp = DateTime.UtcNow.AddDays(7)
                };
                await _refreshreppo.AddRefreshToken(existing);   // sesuaikan nama method
            }
            else
            {
                //replace refreshtoken yang lama dan update expnya
                existing.Token = refreshtoken;
                existing.RefreshTokenExp = DateTime.UtcNow.AddDays(7);
            }
            //save to db
            await _userreppo.SaveChanges();
            return Ok(new
            {
                username = user.Username,
                accesscode = accesstoken,
                refrehcode=refreshtoken,
                message="success",
                Role = user.Role
            });
        }
        [HttpGet("getuserid")]
        [Authorize]
        public IActionResult GetUserId()
        {
            //ambil iduser untuk digunakan di next endpoint
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized(new
                {
                    message= "try to relog"
                });
            }
            return Ok(new{id=userId});
        }

    }
}