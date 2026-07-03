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
    public class EventsController : ExtControllerBase
    {
        private readonly MBCAContext dbc;
        private string uploadDir;

        public EventsController(MBCAContext dbc, IWebHostEnvironment env)
        {
            this.dbc = dbc;
            uploadDir = Path.Combine(env.ContentRootPath, "wwwroot/uploads");
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetAll(int page = 1, int size = 0, string search = "")
        {
            var query = dbc.Events.Include(e => e.eventCategory).AsQueryable();
            if (search != "")
            {
                var str = $"%{search}%";
                query = query.Where(e => EF.Functions.Like(e.title, str) || 
                EF.Functions.Like(e.description, str) ||
                EF.Functions.Like(e.initiator, str) ||
                EF.Functions.Like(e.eventCategory.name, str) ||
                EF.Functions.Like(e.location, str));
            }
            return paginateQuery(query, page, size, e => new
            {
                e.id,
                e.title,
                e.description,
                e.date,
                e.startTime,
                e.endTime,
                e.location,
                e.initiator,
                e.price,
                category = new
                {
                    id = e.eventCategoryId,
                    e.eventCategory.name,
                },
            });
        }

        [HttpGet("{id}/exhibits")]
        [Authorize]
        public ActionResult GetExhibit(int id)
        {
            var data = dbc.EventExhibits.Include(e => e.exhibit.exhibitTags).Include(e => e.exhibit.exhibitCategory).Where(e => e.eventId == id).ToList();
            return json(data.Select(e => new
            {
                e.exhibit.id,
                e.exhibit.name,
                e.exhibit.artist,
                category = new
                {
                    id = e.exhibit.exhibitCategoryId,
                    e.exhibit.exhibitCategory.name,
                },
                e.exhibit.timePeriod,
                tags = e.exhibit.exhibitTags.Select(et => new { et.id, et.tag })
            }).ToList(), "Event exhibits fetched successfully");
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult Get(int id)
        {
            var e = dbc.Events.Include(e => e.eventCategory).Include(e => e.eventBanners).Include(e => e.eventExhibits).ThenInclude(e => e.exhibit.exhibitCategory).FirstOrDefault(e => e.id == id);
            if (e == null) return err("Event not found", 404);
            return json(new
            {
                e.id,
                e.title,
                e.description,
                e.date,
                e.startTime,
                e.endTime,
                e.location,
                e.initiator,
                e.price,
                banners = e.eventBanners.Select(eb => new
                {
                    eb.id,
                    eb.banner
                }).ToList(),
                category = new
                {
                    id = e.eventCategoryId,
                    e.eventCategory.name,
                },
                exhibits = e.eventExhibits.Select(ee => new
                {
                    relId = ee.id,
                    exhibitId = ee.exhibit.id,
                    ee.exhibit.name,
                    ee.exhibit.artist,
                    category = new
                    {
                        id = ee.exhibit.exhibitCategoryId,
                        ee.exhibit.exhibitCategory.name,
                    },
                    ee.exhibit.timePeriod,
                }).ToList()
            }, "Exhibit data fetched successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        async public Task<ActionResult> Create([FromForm] EventDTO input)
        {
            if (input.price <= 0m) return err("Price must be greater than zero");
            if (input.banners.Count < 1) return err("Banner required");
            if (input.exhibits.Count < 1) return err("Exhibit required");
            if (input.startTime >= input.endTime) return err("Time not valid");
            if (input.categoryId <= 0 || !await dbc.EventCategories.AnyAsync(ec => ec.id == input.categoryId)) return err("Category not found", 404);
            foreach(var id in input.exhibits)
            {
                if (!await dbc.Exhibits.AnyAsync(e => e.id == id)) return err("Some exhibit doesn't found", 404);
            }
            if (!input.banners.Any(b => isImageValid(b))) return err("Some banner image doesn't valid");
            var evnt = input.toEntity();
            evnt.eventExhibits = input.exhibits.Select(id => new EventExhibit { exhibitId = id }).ToList();
            foreach(var b in input.banners)
            {
                evnt.eventBanners.Add(new EventBanner { banner = await uploadFile(b, uploadDir) });
            }
            await dbc.Events.AddAsync(evnt);
            await dbc.SaveChangesAsync();
            return msg("Event created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        async public Task<ActionResult> Update(int id, EventDTO input)
        {
            if (input.price <= 0m) return err("Price must be greater than zero");
            if (input.banners.Count > 0 && !input.banners.Any(b => isImageValid(b))) return err("Some banner image doesn't valid");
            if (input.startTime >= input.endTime) return err("Time not valid");
            var rec = dbc.Events.Include(e => e.eventCategory).Include(e => e.eventBanners).Include(e => e.eventExhibits).ThenInclude(e => e.exhibit.exhibitCategory).FirstOrDefault(e => e.id == id);
            if (rec == null) return err("Event not found");
            if (input.categoryId <= 0 || !await dbc.EventCategories.AnyAsync(ec => ec.id == input.categoryId)) return err("Category not found", 404);
            if (input.exhibits.Count < 1)
            {
                foreach (var exhibitId in input.exhibits)
                {
                    if (!await dbc.Exhibits.AnyAsync(e => e.id == exhibitId)) return err("Some exhibit doesn't found", 404);
                }
            }
            rec.title = input.title;
            rec.description = input.description;
            rec.location = input.location;
            rec.initiator = input.initiator;
            rec.date = input.date;
            rec.startTime = input.startTime;
            rec.endTime = input.endTime;
            rec.price = input.price;
            rec.eventCategoryId = input.categoryId;

            var i = 0;
            foreach (var exhibitId in input.exhibits)
            {
                if (i > rec.eventExhibits.Count - 1)
                {
                    rec.eventExhibits.Add(new EventExhibit { exhibitId = exhibitId });
                }
                else if (rec.eventExhibits.ElementAt(i).exhibitId != exhibitId)
                {
                    rec.eventExhibits.ElementAt(i).exhibitId = exhibitId;
                }
                i += 1;
            }
            if(rec.eventExhibits.Count > input.exhibits.Count)
            {
                var toRemoved = rec.eventExhibits.Skip(i);
                foreach (var item in toRemoved) rec.eventExhibits.Remove(item);
            }

            if(input.banners.Count > 0)
            {
                foreach(var b in input.banners)
                {
                    rec.eventBanners.Add(new EventBanner { banner = await uploadFile(b, uploadDir) });
                }
            }

            await dbc.SaveChangesAsync();
            return msg("Event updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public ActionResult Create(int id)
        {
            var rec = dbc.Events.FirstOrDefault(e => e.id == id);
            if (rec == null) return err("Event not found");
            dbc.Events.Remove(rec);
            dbc.SaveChanges();
            return msg("Event created successfully");
        }


        // Categories
        [HttpGet("categories")]
        [Authorize(Roles = "Employee")]
        public ActionResult GetCategories()
        {
            var data = dbc.EventCategories.Select(e => new { e.id, e.name}).ToList();
            return json(data, "Exhibit categories fetched successfully");
        }
    }

    public class EventDTO
    {
        [Required] public string title { get; set; } = "";
        [Required] public string description { get; set; } = "";
        [Required] public int categoryId { get; set; }
        [Required] public DateOnly date { get; set; }
        [Required] public TimeOnly startTime { get; set; }
        [Required] public TimeOnly endTime { get; set; }
        [Required] public string location { get; set; } = "";
        [Required] public string initiator { get; set; } = "";
        [Required] public decimal price { get; set; }
        [Required] public IFormFileCollection banners { get; set; } = new FormFileCollection();
        [Required] public List<int> exhibits { get; set; } = new List<int>();


        public Event toEntity()
        {
            return new Event { title =  title, description = description, date = date, initiator = initiator, price = price, startTime = startTime, endTime =  endTime, location = location, eventCategoryId = categoryId };
        }
    }
}
