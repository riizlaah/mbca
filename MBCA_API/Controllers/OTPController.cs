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
    public class OTPController : ExtControllerBase
    {
        private readonly MBCAContext dbc;
        private readonly IConfiguration conf;

        public OTPController(MBCAContext dbc, IConfiguration conf)
        {
            this.dbc = dbc;
            this.conf = conf;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            var userId = getUserId();
            var user = dbc.Users.Include(e => e.otps).FirstOrDefault(e => e.id == userId);
            if (user == null) return err("User not found");
            var otp = user.otps.OrderByDescending(e => e.validUntil).FirstOrDefault();
            return json(new
            {
                userId,
                user.isActivated,
                user.username,
                user.fullName,
                otp?.code,
                otp?.validUntil
            }, "OTP fetched successfully");
        }

        [HttpPost("new")]
        [Authorize]
        public ActionResult New()
        {
            var userId = getUserId();
            var otp = dbc.OTPs.OrderByDescending(e => e.validUntil).FirstOrDefault(e => e.validUntil < DateTime.Now && e.userId == userId);
            if (otp != null)
            {
                var span = DateTime.Now - otp.validUntil;
                if (span.TotalSeconds < 30) return err($"Please wait {30 - span.TotalSeconds} seconds before retry");
            }
            dbc.OTPs.Add(new OTP
            {
                userId = userId,
                code = randStr(6),
                validUntil = DateTime.Now.AddMinutes(2)
            });
            dbc.SaveChanges();
            return msg("New OTP code has been sent");
        }

        [HttpPost("verify")]
        [Authorize]
        public ActionResult Verify(VerifyOTPDTO input)
        {
            var userId = getUserId();
            var now = DateTime.Now;
            if (dbc.Users.Any(u => u.isActivated && u.id == userId)) return err("Account has been activated");
            var otp = dbc.OTPs.Include(e => e.user.role).FirstOrDefault(e => e.validUntil > now && e.userId == userId && e.code == input.code);
            if (otp == null)
            {
                return err("OTP Code not valid");
            }
            otp.user.isActivated = true;
            dbc.SaveChanges();
            return json(new
            {
                otp.userId,
                otp.user.username,
                newToken = generateToken(otp.userId, otp.user.role.name, otp.user.isActivated)
            }, "Account activated successfully");
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

    public class VerifyOTPDTO
    {
        [Required] public string code { get; set; } = "";
    }
}
