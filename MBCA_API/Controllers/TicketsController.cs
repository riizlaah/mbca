using MBCA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MBCA_API.Controllers
{
    [Route("mbca-api/v1/[controller]")]
    [ApiController]
    public class TicketsController : ExtControllerBase
    {
        private readonly MBCAContext dbc;

        public TicketsController(MBCAContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpGet]
        [Authorize(Roles = "Visitor")]
        public ActionResult GetAll(int page = 1, int size = 0)
        {
            var userId = getUserId();
            var query = dbc.Tickets.Where(e => e.userId == getUserId()).Include(e => e.promo).Include(e => e.Event).AsQueryable();
            return paginateQuery(query, page, size, e => new
            {
                e.id,
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Visitor")]
        public ActionResult Get(int id)
        {
            // TODO
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Visitor")]
        public ActionResult Buy()
        {
            // TODO
            return Ok();
        }
    }
}
