using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MBCA_API
{
    public class ExtControllerBase: ControllerBase
    {
        protected int getUserId() => Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        protected string getRole() => User.FindFirstValue(ClaimTypes.Role) ?? "";
        protected ObjectResult json(object? data, string message, int code = 200)
        {
            return new ObjectResult(new {message, data}) { StatusCode = code};
        }

        protected ObjectResult msg(string message, int code = 200)
        {
            return new ObjectResult(new { message }) { StatusCode = code };
        }
        protected ObjectResult err(string message, int code = 422) => msg(message, code);

        protected string hash(string str)
        {
            using(var alg = SHA256.Create())
            {
                var bytes = alg.ComputeHash(Encoding.UTF8.GetBytes(str));
                var sb = new StringBuilder();
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
        protected bool isHashValid(string str, string hashedStr)
        {
            var str1 = hash(str);
            return StringComparer.OrdinalIgnoreCase.Compare(str1, hashedStr) == 0;
        }

        protected ObjectResult paginateQuery<TModel, TRes>(IQueryable<TModel> query, int page, int size, Func<TModel, TRes> selector)
        {
            if (page < 1) return err("Page not valid");
            if (size < 0) return err("Size not valid");
            List<TRes> data;
            var items = 0;
            if(size == 0)
            {
                items = query.Count();
                data = query.Select(selector).ToList();
            } else
            {
                items = query.Count();
                data = query.Select(selector).Skip((page - 1) * size).Take(size).ToList();
            }
            return new ObjectResult(new
            {
                data,
                pagination = new
                {
                    page,
                    size,
                    totalPage = size == 0 ? 1 : (int)Math.Ceiling((decimal)items / size),
                    items
                }
            })
            { StatusCode = items > 0 ? 200 : 404};
        }

        protected bool isImageValid(IFormFile? file)
        {
            var allowed = new[] { "image/jpeg", "image/png" };
            return file != null && file.Length > 0 && allowed.Contains(file.ContentType);
        }

        async protected Task<string> uploadFile(IFormFile file, string uploadDir, string? target = null)
        {
            var ext = Path.GetExtension(file.FileName);
            var uniqueName = target ?? $"{DateTime.Now:ddMMyyyy-HHmmss}_{Guid.NewGuid()}{ext}";
            var path = Path.Combine(uploadDir, uniqueName);
            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return uniqueName;
        }

        protected string randStr(int len)
        {
            var chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var rand = new Random();
            var str = "";
            for(var i  = 0; i < len; i++) str += chars[rand.Next(chars.Length - 1)];
            return str;
        }

        protected bool isNotVerified() => User.FindFirstValue("verified") != "True";

        protected ObjectResult notVerified() => err("Your account hasn't been activated");
    }
}
