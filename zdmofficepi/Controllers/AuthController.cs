using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using zdmofficepi.DataAccess;
using zdmofficepi.Models;
using zdmofficepi.Utils;

namespace zdmofficepi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDBContext _context;
        UnitofWork UnitofWork;
        PasswordUtils PasswordUtils;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            UnitofWork = new UnitofWork(context);
            PasswordUtils = new PasswordUtils();

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            return Ok("OK");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DBTest")]
        public IActionResult DBTest()
        {
            return Ok($"Aktif Kullanıcı Sayısı = {UnitofWork.UserRepositroy.GetAll().Count}");
        }

        //Admin Kullanıcının Oluşturulması
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = UnitofWork.UserRepositroy.GetAll().FirstOrDefault(u=>u.Username==model.Username);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Bu Kullanıcı Adı Daha Önce Alındı" });
            var userGuid = Guid.NewGuid().ToString();
            var RoleGuid = Guid.NewGuid().ToString();
            var salt = PasswordUtils.CreateSalt(30);
            UserModel user = new UserModel()
            {
                Id = 0,
                Username = model.Username,
                Uuid = userGuid,
                Email = model.Email,
                IsActive = false,
                Createduser = "System",
                Createdtime = DateTime.Now,
                PasswordHash = PasswordUtils.GenerateHash(model.Password, salt),
                PhoneNumber = "",
                Salt = salt
            };
            UnitofWork.UserRepositroy.Add(user);
            UnitofWork.Complate();
            return Ok(new ResponseModel { Status = "Success", Message = "Kullanıcı Başarı ile Oluşturuldu" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = UnitofWork.UserRepositroy.GetRecords<UserModel>(u => u.IsActive).ToList().FirstOrDefault(u => u.Username == model.Username);
            if ((user == null))
            {
                return NotFound(new ResponseModel { Status = "Error", Message = "Kullanıcı Bulunamadı" });
            }
            if (!CheckPassword(user, model.Password))
            {
                return Unauthorized(new ResponseModel { Status = "Error", Message = "Kullanıcı Adı veya Şifre Hatalı" });
            }
            var authClaims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name,user.Username),
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            Response.Cookies.Append("X-Access-Token", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Username", user.Username, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                User = user.Username
            });
        }

        [Authorize]
        [HttpGet]
        [Route("GetActiveUser")]
        public async Task<IActionResult> GetActiveUser()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(userId);
        }

        private bool CheckPassword(UserModel user, string password)
        {
            return PasswordUtils.AreEqual(password, user.PasswordHash, UnitofWork.UserRepositroy.GetAll().FirstOrDefault(u=>u.Username == user.Username).Salt);
        }

    }
}
