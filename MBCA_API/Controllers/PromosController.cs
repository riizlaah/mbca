using MBCA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MBCA_API.Controllers
{
    [Route("mbca-api/v1/[controller]")]
    [ApiController]
    public class PromosController : ExtControllerBase
    {
        private readonly MBCAContext dbc;

        public PromosController(MBCAContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpGet("{code}/check")]
        [Authorize]
        public ActionResult Check(string code)
        {
            var rec = dbc.Promos.FirstOrDefault(p => p.code == code);
            if (rec == null || DateOnly.FromDateTime(DateTime.Now) < rec.startDate) return err("Promo not found", 404);
            if (rec.endDate < DateOnly.FromDateTime(DateTime.Now)) return err("Promo expired");
            return json(new
            {
                rec.id,
                rec.code,
                rec.discountPercentage,
                rec.startDate,
                rec.endDate,
            }, "Promo fetched");
        }
    }
}
