using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using Ticket.Data;
using Ticket.DTO;
using Ticket.Interface;
using Ticket.Models;
namespace Ticket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController :ControllerBase
    {
        private readonly IUser _userreppo;
        private readonly IPassword _passreppo;
        private readonly IRefreshForRepo _refreshreppo;
        private readonly IRefreshToken _jwt;
        public RegisterController(IUser userreppo, IRefreshToken jwt,IPassword passreppo, IRefreshForRepo refreshreppo)
        {
            _userreppo = userreppo;
            _jwt = jwt;
            _passreppo = passreppo;
            _refreshreppo = refreshreppo;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            //kalau username sudah ada, maka suruh user ganti username
            User existuser = await _userreppo.FindUserByUsername(request.username);
            if (existuser != null)
            {
                return BadRequest(new {message ="the username already exist"});
            }

            User newuser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.username,
                Email = request.email,
                IsActive = true,
                DateIn = DateTime.Now,
                Role ="User"
            };
            //membuat accesstoken dan refreshtoken
            var accesstoken = _jwt.CreateAccessToken(newuser.Id, newuser.Username, newuser.Role);
            var refreshtoken = _jwt.CreateRefreshToken();
            //hashpassword dan simpan kedalam db
            var hashedpassword = BCrypt.Net.BCrypt.HashPassword(request.password);
            Password pass = new Password
            {
                Id = Guid.NewGuid().ToString(),
                UserId = newuser.Id,
                HashedPassword = hashedpassword,
                IsActive = true,
                DateIn = DateTime.UtcNow
            };
            //simpan refreshtoken dalam db
            RefreshToken token = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId= newuser.Id,
                Token = refreshtoken,
                RefreshTokenExp = DateTime.UtcNow.AddDays(7)
            };
            //set accesstoken dan refreshtoken ke cookie agar kita bisa memanggilnya dari cookie
            Response.Cookies.Append("accessToken", accesstoken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            Response.Cookies.Append("refreshToken", refreshtoken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            newuser.Refreshtoken = token;
            await _passreppo.AddPassword(pass);
            await _refreshreppo.AddRefreshToken(token);
            await _userreppo.AddUser(newuser);
            await _userreppo.SaveChanges();
            return Ok(new
            {
                accessToken = accesstoken,
                refreshToken = refreshtoken,
                message ="successfully register your user account"
            });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            //ambil refreshcode yang disimpan di cookie
            //kita bisa ambil refreshtoken pake metode lain yaitu ambil dari db
            var token = Request.Cookies["refreshToken"];
            if (token == null)
                return Unauthorized(new
                {
                    message =  "error, try to relog"
                });
            //ambil entityrefreshtoken dalam db lalu ganti dengan refreshtoken yang baru
            var refreshtoken = await _refreshreppo.GetRefreshTokenByToken(token);
            if (refreshtoken == null)
                return Unauthorized();
            if (refreshtoken.RefreshTokenExp <= DateTime.UtcNow)
            {
                //refreshtoken juga expired, maka slah satu caranya adalah relog untuk mendapatkan new accesstoken dan refreshtoken
                return Unauthorized(new
                {
                    message =  "error, try to relog"
                });
            }
            //ambil entity user supaya kita bisa buat accesstoken yang berisi data user
            var user = await _userreppo.FindUserById(refreshtoken.UserId);
            var accesstoken = _jwt.CreateAccessToken(user.Id, user.Username, user.Role);
            var newrefreshtoken = _jwt.CreateRefreshToken();

            Response.Cookies.Append("accessToken", accesstoken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            Response.Cookies.Append("refreshToken", newrefreshtoken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            
            refreshtoken.Token = newrefreshtoken;
            user.Refreshtoken = refreshtoken;
            await _userreppo.SaveChanges();
            return Ok(new
            {
                message ="success on refresh",
                accescode = accesstoken,
                newtoken = newrefreshtoken
            });
        }
    }
}
