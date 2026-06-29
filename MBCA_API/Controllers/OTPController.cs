using MBCA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MBCA_API.Controllers
{
    [Route("mbca-api/v1/[controller]")]
    [ApiController]
    public class OTPController : ExtControllerBase
    {
        private readonly MbcaContext dbc;

        public OTPController(MbcaContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpGet]
        [Authorize]
        public ActionResult OTP()
        {
            var userId = getUserId();
            var otp = dbc.Otps.OrderByDescending(e => e.ValidUntil).FirstOrDefault(e => e.UserId == getUserId());
            if (otp == null) return err("OTP not found", 404);
            if (otp.validUntildt < DateTime.Now) return err("No current OTP available.", 404);
            return json(new
            {
                id = otp.Id,
                userId = otp.UserId,
                code = otp.Code,
                validUntil = otp.validUntildt
            }, "OTP fetched successfully");
        }

        [HttpPost]
        [Authorize]
        public ActionResult New()
        {
            var userId = getUserId();
            var otp = dbc.Otps.OrderByDescending(e => e.ValidUntil).FirstOrDefault(e => e.UserId == getUserId());
            if (otp != null)
            {
                var span = DateTime.Now - otp.validUntildt;
                if (span.TotalSeconds < 30) return err("Please wait 30 seconds before generating a new OTP code.", 404);
            }
            dbc.Otps.Add(new Otp
            {
                UserId = userId,
                Code = randStr(6),
                ValidUntil = DateTime.Now.AddMinutes(2).ToBinary()
            });
            return msg("New OTP code generated successfully");
        }
    }
}
