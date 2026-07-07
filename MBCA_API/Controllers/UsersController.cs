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
using System.Text.RegularExpressions;

namespace MBCA_API.Controllers
{
    [Route("mbca-api/v1/[controller]")]
    [ApiController]
    public class UsersController : ExtControllerBase
    {
        private readonly MBCAContext dbc;
        private readonly IConfiguration conf;

        public UsersController(MBCAContext dbc, IConfiguration conf)
        {
            this.dbc = dbc;
            this.conf = conf;
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDTO inp)
        {
            var user = dbc.Users.Include(e => e.role).FirstOrDefault(e => e.username == inp.usernameOrEmail || e.email == inp.usernameOrEmail);
            if (user == null) return err("Credentials not valid");
            if (!isHashValid(inp.password, user.password)) return err("Credentials not valid");
            return json(new
            {
                user.id,
                user.username,
                user.email,
                role = user.role.name,
                token = generateToken(user.id, user.role.name, user.isActivated),
                user.isActivated
            }, "Login successful");
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterDTO inp)
        {
            var pw = inp.password;
            if (pw.Length < 8) return err("Password length must be 8 characters or more");
            var hasLetter = pw.Any(Char.IsLetter);
            var hasDigit = pw.Any(Char.IsDigit);
            var hasSymbol = pw.Any(c => !Char.IsLetterOrDigit(c));
            if (!hasLetter || !hasDigit || !hasSymbol) return err("Password length must be 8 characters or more");
            if (!Regex.IsMatch(inp.phoneNumber, @"\+?\d{10,}")) return err("Phone Number not valid");
            if (dbc.Users.Any(e => e.username == inp.username)) return err("Username has been taken");
            if (dbc.Users.Any(e => e.email == inp.email)) return err("Email has been taken");
            if (dbc.Users.Any(e => e.phoneNumber == inp.phoneNumber)) return err("Phone Number has been taken");
            dbc.Users.Add(new User
            {
                username = inp.username,
                password = hash(inp.password),
                email = inp.email,
                fullName = inp.fullName,
                phoneNumber = inp.phoneNumber,
                roleId = 1
            });
            dbc.SaveChanges();
            return msg("User registered successfully");
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult Me()
        {
            var userId = getUserId();
            var user = dbc.Users.Include(e => e.role).FirstOrDefault(e => e.id == userId);
            if (user == null) return err("User not found");
            return json(new
            {
                id = userId,
                user.fullName,
                user.username,
                user.email,
                user.phoneNumber,
                role = user.role.name,
                user.isActivated
            }, "Profile fetched successfully");
        }

        


        protected string generateToken(int id, string role, bool verified)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("verified", verified.ToString())
            };
            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"])), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(conf["Jwt:Issuer"], conf["Jwt:Audience"], claims, expires: DateTime.Now.AddHours(8), signingCredentials: creds);
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
        [Required][EmailAddress] public string email { get; set; } = "";
        [Required] public string phoneNumber { get; set; } = "";
        [Required] public string password { get; set; } = "";
    }
}
