using MBCA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MBCA_API.Controllers
{
    [Route("mbca-api/v1/[controller]")]
    [ApiController]
    public class UsersController : ExtControllerBase
    {
        private readonly MbcaContext dbc;
        private readonly IConfiguration conf;

        public UsersController(MbcaContext dbc, IConfiguration conf)
        {
            this.dbc = dbc;
            this.conf = conf;
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDTO input)
        {
            var user = dbc.Users.Include(e => e.Role).FirstOrDefault(e => e.Username == input.usernameOrEmail || e.Email == input.usernameOrEmail);
            if (user == null) return err("Credentials invalid.", 401);
            if (isHashValid(input.password, user.Password)) return err("Credentials invalid.", 401);
            return json(new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email,
                role = user.Role.Name,
                isActive = user.IsActivated,
                token = GenToken(user.Id, user.Role.Name),
            }, "Login successful");
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterDTO input)
        {
            var pw = input.password;
            if (pw.Length < 8) return err("Password length must be 8 characters or more.");
            var hasLetter = pw.Any(Char.IsLetter);
            var hasDigit = pw.Any(Char.IsDigit);
            var hasSymbol = pw.Any(c => !Char.IsLetterOrDigit(c));
            if (!hasLetter || !hasSymbol || !hasDigit) return err("Password must have combination of letters, digits and symbols.");
            if (dbc.Users.Any(e => e.Username == input.username)) return err("Username has been taken.");
            if (dbc.Users.Any(e => e.Email == input.email)) return err("Email has been taken.");
            if (dbc.Users.Any(e => e.PhoneNumber == input.phoneNumber)) return err("Phone number has been taken.");
            var user = new User
            {
                Username = input.username,
                Email = input.email,
                PhoneNumber = input.phoneNumber,
                FullName = input.fullName,
                Password = hash(input.password),
                IsActivated = false,
                RoleId = 1
            };
            dbc.Users.Add(user);
            dbc.SaveChanges();
            dbc.Otps.Add(new Otp
            {
                UserId = user.Id,
                Code = randStr(6),
                ValidUntil = DateTime.Now.AddMinutes(2).ToBinary()
            });
            dbc.SaveChanges();
            return msg("User registered successfully");
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult Me()
        {
            var user = dbc.Users.Include(e => e.Role).FirstOrDefault(e => e.Id == getUserId());
            if (user == null) return err("User not found", 404);
            return json(new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email,
                fullName = user.FullName,
                phoneNumber = user.PhoneNumber,
                isActive = user.IsActivated,
                role = user.Role.Name,
            }, "User profile fetched successfully");
        }

        



        protected string GenToken(int id, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"])), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(conf["Jwt:Issuer"], conf["Jwt:Audience"], claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        } 
    }

    public class LoginDTO 
    {
        [Required] public string usernameOrEmail { get; set; } = "";
        [Required] public string password { get; set; } = "";
    }

    public class RegisterDTO
    {
        [Required] public string username { get; set; } = "";
        [Required] public string fullName { get; set; } = "";
        [Required] public string phoneNumber { get; set; } = "";
        [Required][EmailAddress] public string email { get; set; } = "";
        [Required] public string password { get; set; } = "";
    }
}
