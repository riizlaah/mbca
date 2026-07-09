using MBCA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
            if (isNotVerified()) return notVerified();
            var userId = getUserId();
            var query = dbc.Tickets.Where(e => e.userId == userId).Include(e => e.promo).Include(e => e.Event.eventBanners).Include(e => e.Event.eventCategory).OrderByDescending(e => e.transactionDate).AsQueryable();
            return paginateQuery(query, page, size, e => new
            {
                e.id,
                Event = new
                {
                    e.Event.id,
                    e.Event.title,
                    e.Event.date,
                    e.Event.startTime,
                    e.Event.endTime,
                    e.Event.price,
                    category = new
                    {
                        id = e.Event.eventCategoryId,
                        e.Event.eventCategory.name,
                    },
                    banners = e.Event.eventBanners.Select(eb => new
                    {
                        eb.id,
                        eb.banner
                    }).ToList(),
                },
                promo = e.promo == null ? null : new
                {
                    e.promo.id,
                    e.promo.code,
                    e.promo.discountPercentage,
                    e.promo.startDate,
                    e.promo.endDate,
                },
                e.qty,
                e.totalPrice,
                e.transactionDate,
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Visitor")]
        public ActionResult Get(int id)
        {
            if (isNotVerified()) return notVerified();
            var userId = getUserId();
            var e = dbc.Tickets.Include(e => e.promo).Include(e => e.Event.eventBanners).Include(e => e.Event.eventCategory)
                .Include(e => e.Event.eventExhibits).ThenInclude(e => e.exhibit.exhibitTags).Include(e => e.Event.eventExhibits).ThenInclude(e => e.exhibit.exhibitCategory).FirstOrDefault(e => e.id == id && e.userId == userId);
            if (e == null) return err("Ticket not found", 404);
            return json(new
            {
                e.id,
                Event = new
                {
                    e.Event.id,
                    e.Event.title,
                    e.Event.date,
                    e.Event.startTime,
                    e.Event.endTime,
                    e.Event.price,
                    category = new
                    {
                        id = e.Event.eventCategoryId,
                        e.Event.eventCategory.name,
                    },
                    banners = e.Event.eventBanners.Select(eb => new
                    {
                        eb.id,
                        eb.banner
                    }).ToList(),
                    exhibits = e.Event.eventExhibits.Select(ee => new
                    {
                        relId = ee.id,
                        exhibitId = ee.exhibit.id,
                        ee.exhibit.name,
                        ee.exhibit.artist,
                        ee.exhibit.timePeriod,
                        ee.exhibit.image,
                        category = new
                        {
                            id = ee.exhibit.exhibitCategoryId,
                            ee.exhibit.exhibitCategory.name,
                        },
                        tags = ee.exhibit.exhibitTags.Select(et => new
                        {
                            et.id,
                            et.tag
                        })
                    })
                },
                promo = e.promo == null ? null : new
                {
                    e.promo.id,
                    e.promo.code,
                    e.promo.discountPercentage,
                    e.promo.startDate,
                    e.promo.endDate,
                },
                e.qty,
                e.totalPrice,
                e.transactionDate,
            }, "Ticket fetched successfully");
        }

        [HttpPost("purchase")]
        [Authorize(Roles = "Visitor")]
        public ActionResult Purchase(PurchaseTicketDTO input)
        {
            if (isNotVerified()) return notVerified();
            if (input.qty < 1) return err("Quantity not valid");
            var evnt = dbc.Events.FirstOrDefault(e => e.id == input.eventId);
            if (evnt == null) return err("Event not found", 404);
            Promo? promo = null;
            var mult = 1m;
            if(input.code != "")
            {
                promo = dbc.Promos.FirstOrDefault(p => p.code == input.code);
                if (promo == null) return err("Promo not found", 404);
                if (promo.startDate > DateOnly.FromDateTime(DateTime.Now)) return err("Promo hasn't started yet");
                if (promo.endDate < DateOnly.FromDateTime(DateTime.Now)) return err("Promo expired");
                mult = 1m - (promo.discountPercentage * 0.01m);
            }
            dbc.Tickets.Add(new Ticket
            {
                userId = getUserId(),
                eventId = input.eventId,
                promoId = promo?.id,
                totalPrice = evnt.price * mult,
                qty = input.qty,
                transactionDate = DateTime.Now,
            });
            dbc.SaveChanges();
            return msg("Ticket purchased successfully");
        }
    }

    public class PurchaseTicketDTO
    {
        [Required] public int qty { get; set; }
        [Required] public int eventId { get; set; }
        [Required(AllowEmptyStrings = true)] public string code { get; set; } = "";
    }
}
