using MBCA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MBCA_API.Controllers
{
    [Route("mbca-api/v1/[controller]")]
    [ApiController]
    public class PhonePrefixesController : ExtControllerBase
    {
        private readonly MBCAContext dbc;

        public PhonePrefixesController(MBCAContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var data = dbc.PhonePrefixes.OrderBy(e => e.prefix).Select(e => e.prefix).ToList();
            return json(data, "Phone prefixes fetched");
        }
    }
}
