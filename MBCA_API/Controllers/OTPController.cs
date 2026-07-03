using MBCA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MBCA_API.Controllers
{
    [Route("mbca-api/v1/[controller]")]
    [ApiController]
    public class OTPController : ExtControllerBase
    {
        private readonly MBCAContext dbc;

        public OTPController(MBCAContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            var userId = getUserId();
            var user = dbc.Users.Include(e => e.otps).FirstOrDefault(e => e.id == userId);
            if (user == null) return err("User not found");
            var otp = user.otps.OrderByDescending(e => e.validUntil).FirstOrDefault(e => e.validUntil < DateTime.Now);
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

        [HttpPost]
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
            return msg("New OTP code has been sent");
        }
    }
}
