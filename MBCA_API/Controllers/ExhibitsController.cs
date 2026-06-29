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
    public class ExhibitsController : ExtControllerBase
    {
        private readonly MbcaContext dbc;

        public ExhibitsController(MbcaContext dbc)
        {
            this.dbc = dbc;
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public ActionResult GetAll(int page = 1, int size = 0, string search = "")
        {
            var query = dbc.Exhibits.Include(e => e.ExhibitTags).Include(e => e.ExhibitCategory).AsQueryable();
            if (search.Trim() != "")
            {
                var str = $"%{search}%";
                query.Where(e => EF.Functions.Like(e.Name, str) || EF.Functions.Like(e.Artist, str) || EF.Functions.Like(e.TimePeriod, str) || EF.Functions.Like(e.ExhibitCategory.Name, str));
            }
            return PaginateQuery(query, page, size, e => new
            {
                id = e.Id,
                name = e.Name,
                artist = e.Artist,
                timePeriod = e.TimePeriod,
                category = new
                {
                    id = e.ExhibitCategoryId,
                    name = e.ExhibitCategory.Name
                },
                tags = e.ExhibitTags.ToList().Select(e => new
                {
                    id = e.Id,
                    name = e.Tag
                })
            }, "Exhibits data fetched successfully");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]
        public ActionResult GetAll(int id)
        {
            var e = dbc.Exhibits.Include(e => e.ExhibitTags).Include(e => e.ExhibitCategory).FirstOrDefault(e => e.Id == id);
            if (e == null) return err("Exhibit not found");
            return json(new
            {
                id = e.Id,
                name = e.Name,
                artist = e.Artist,
                timePeriod = e.TimePeriod,
                category = new
                {
                    id = e.ExhibitCategoryId,
                    name = e.ExhibitCategory.Name
                },
                tags = e.ExhibitTags.ToList().Select(e => new
                {
                    id = e.Id,
                    name = e.Tag
                })
            }, "Exhibit data fetched successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public ActionResult Create(ExhibitDTO input)
        {
            if (input.tags.Length < 1) return err("Exhibit must have at least one tag");
            if (dbc.ExhibitCategories.Any(e => e.Id == input.categoryId)) return err("Category not found", 404);
            dbc.Exhibits.Add(input.ToEntity());
            dbc.SaveChanges();
            return msg("Exhibit created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public ActionResult Update(int id, ExhibitDTO input)
        {
            var rec = dbc.Exhibits.Include(e => e.ExhibitTags).FirstOrDefault(e => e.Id == id);
            if (rec == null) return err("Exhibit not found");
            if (input.tags.Length < 1) return err("Exhibit must have at least one tag");
            if (dbc.ExhibitCategories.Any(e => e.Id == input.categoryId)) return err("Category not found", 404);
            input.UpdateEntity(rec);
            dbc.SaveChanges();
            return msg("Exhibit updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public ActionResult Delete(int id)
        {
            var rec = dbc.Exhibits.FirstOrDefault(e => e.Id == id);
            if (rec == null) return err("Exhibit not found");
            dbc.Exhibits.Remove(rec);
            dbc.SaveChanges();
            return msg("Exhibit deleted successfully");
        }





        // Categories
        [HttpGet("categories")]
        [Authorize(Roles = "Employee")]
        public ActionResult GetAllCategories()
        {
            var data = dbc.ExhibitCategories.ToList().Select(e => new
            {
                id = e.Id,
                name = e.Name,
            });
            return json(data, "Categories fetched successfully");
        }
    }

    public class ExhibitDTO
    {
        [Required] public string name { get; set; } = null!;
        [Required] public string artist { get; set; } = null!;
        [Required] public int categoryId { get; set; }
        [Required] public string timePeriod { get; set; } = null!;
        [Required] public string[] tags { get; set; } = null!;

        public Exhibit ToEntity()
        {
            var e = new Exhibit
            {
                Name = name,
                Artist = artist,
                TimePeriod = timePeriod,
                ExhibitCategoryId = categoryId,
                ExhibitTags = tags.Select(t => new ExhibitTag
                {
                    Tag = t
                }).ToList()
            };
            return e;
        }

        public Exhibit UpdateEntity(Exhibit e)
        {
            e.Name = name;
            e.Artist = artist;
            e.TimePeriod = timePeriod;
            e.ExhibitCategoryId = categoryId;
            var toRemoved = new List<ExhibitTag>();
            foreach(var t in e.ExhibitTags)
            {
                if(!tags.Contains(t.Tag)) toRemoved.Add(t);
            }
            foreach(var t in tags)
            {
                if(!e.ExhibitTags.Any(e => e.Tag == t)) e.ExhibitTags.Add(new ExhibitTag { Tag = t });
            }
            e.ExhibitTags = e.ExhibitTags.Where(t => !toRemoved.Any(r => r.Tag == t.Tag)).ToList();
            return e;
        }
    }
}
