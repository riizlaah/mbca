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
        private readonly MBCAContext dbc;
        private string uploadDir;

        public ExhibitsController(MBCAContext dbc, IWebHostEnvironment env)
        {
            this.dbc = dbc;
            uploadDir = Path.Combine(env.ContentRootPath, "wwwroot/uploads");
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public ActionResult GetAll(int page = 1, int size = 0, string search = "")
        {
            if (isNotVerified()) return notVerified();
            var query = dbc.Exhibits.Include(e => e.exhibitTags).Include(e => e.exhibitCategory).AsQueryable();
            if(search != "")
            {
                var str = $"%{search}%";
                query = query.Where(e => EF.Functions.Like(e.artist, str) || 
                EF.Functions.Like(e.name, str) ||
                EF.Functions.Like(e.timePeriod, str));
            }
            return paginateQuery(query, page, size, e => new
            {
                e.id,
                e.name,
                e.artist,
                category = new
                {
                    id = e.exhibitCategoryId,
                    e.exhibitCategory.name,
                },
                e.timePeriod,
                tags = e.exhibitTags.Select(et => new { et.id, et.tag })
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]
        public ActionResult Get(int id)
        {
            if (isNotVerified()) return notVerified();
            var e = dbc.Exhibits.Include(e => e.exhibitTags).Include(e => e.exhibitCategory).FirstOrDefault(e => e.id == id);
            if (e == null) return err("Exhibit not found", 404);
            return json(new
            {
                e.id,
                e.name,
                e.artist,
                category = new
                {
                    id = e.exhibitCategoryId,
                    e.exhibitCategory.name,
                },
                e.timePeriod,
                tags = e.exhibitTags.Select(et => new { et.id, et.tag }),
                e.image,
            }, "Exhibit data fetched successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        async public Task<ActionResult> Create([FromForm] ExhibitDTO input)
        {
            if (isNotVerified()) return notVerified();
            if (input.tags.Count < 1) return err("Tag required");
            if (input.image == null || !isImageValid(input.image)) return err("Image not valid");
            if (input.categoryId < 1 || !await dbc.ExhibitCategories.AnyAsync(ec => ec.id == input.categoryId)) return err("Category not found", 404);
            var imagePath = await uploadFile(input.image, uploadDir);
            await dbc.AddAsync(input.toEntity(imagePath));
            await dbc.SaveChangesAsync();
            return msg("Exhibit created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        async public Task<ActionResult> Update(int id, [FromForm] ExhibitDTO input)
        {
            if (isNotVerified()) return notVerified();
            if (input.tags.Count < 1) return err("Tag required");
            if (input.image != null && !!isImageValid(input.image)) return err("Image not valid");
            var rec = await dbc.Exhibits.Include(e => e.exhibitTags).FirstOrDefaultAsync(e => e.id == id);
            if (rec == null) return err("Exhibit not found");
            if (input.categoryId < 1 || !await dbc.ExhibitCategories.AnyAsync(ec => ec.id == input.categoryId)) return err("Category not found", 404);
            rec.name = input.name;
            rec.artist = input.artist;
            rec.timePeriod = input.timePeriod;
            if (input.image != null) rec.image = await uploadFile(input.image, uploadDir, rec.image);
            rec.exhibitCategoryId = input.categoryId;

            var i = 0;
            foreach (var tag in input.tags)
            {
                if (i > rec.exhibitTags.Count - 1)
                {
                    rec.exhibitTags.Add(new ExhibitTag { tag = tag });
                }
                else if (rec.exhibitTags.ElementAt(i).tag != tag)
                {
                    rec.exhibitTags.ElementAt(i).tag = tag;
                }
                i += 1;
            }
            if(rec.exhibitTags.Count > input.tags.Count)
            {
                var toRemoved = rec.exhibitTags.Skip(i);
                foreach (var item in toRemoved) rec.exhibitTags.Remove(item);
            }

            await dbc.SaveChangesAsync();
            return msg("Exhibit updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public ActionResult Delete(int id)
        {
            if (isNotVerified()) return notVerified();
            var rec = dbc.Exhibits.FirstOrDefault(e => e.id == id);
            if (rec == null) return err("Exhibit not found");
            dbc.Exhibits.Remove(rec);
            dbc.SaveChanges();
            return msg("Exhibit removed successfully");
        }


        // Categories
        [HttpGet("categories")]
        [Authorize(Roles = "Employee")]
        public ActionResult GetCategories()
        {
            if (isNotVerified()) return notVerified();
            var data = dbc.ExhibitCategories.Select(e => new { e.id, e.name}).ToList();
            return json(data, "Exhibit categories fetched successfully");
        }
    }

    public class ExhibitDTO
    {
        [Required] public string name { get; set; } = "";
        [Required] public string artist { get; set; } = "";
        [Required] public string timePeriod { get; set; } = "";
        [Required] public int categoryId { get; set; }
        [Required] public IFormFile? image { get; set; }
        [Required] public List<string> tags { get; set; } = new List<string>();

        public Exhibit toEntity(string imagePath)
        {
            return new Exhibit { 
                name = name, 
                artist = artist, timePeriod = timePeriod, image = imagePath, exhibitTags = tags.Select(name => new ExhibitTag { tag = name }).ToList() };
        }
    }
}
