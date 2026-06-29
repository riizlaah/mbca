using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MBCA_API
{
    public class ExtControllerBase: ControllerBase
    {
        protected int getUserId()
        {
            return Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        protected ObjectResult json(object? data, string message, int code = 200)
        {
            return new ObjectResult(new { message, data })
            {
                StatusCode = code
            };
        }

        protected ObjectResult msg(string message, int code = 200)
        {
            return new ObjectResult(new { message })
            {
                StatusCode = code
            };
        }

        protected ObjectResult err(string message, int code = 422)
        {
            return msg(message, code);
        }

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

        protected string randStr(int len)
        {
            var rand = new Random();
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var str = "";
            for (var i = 0; i < len; i++) str += chars[rand.Next(chars.Length - 1)];
            return str;
        }

        protected ObjectResult PaginateQuery<TModel, TRes>(IQueryable<TModel> query, int page, int size, Func<TModel, TRes> selector, string message)
        {
            if (page <= 0) return err("Page not valid");
            if (size < 0) return err("Size not valid");
            var items = query.Count();
            var totalPage = (int)Math.Ceiling((decimal)items / size);
            var data = query.Select(selector).Skip((page - 1) * size).Take(size);
            return new ObjectResult(new
            {
                message,
                data,
                pagination = new
                {
                    page,
                    size,
                    totalPage,
                    items
                }
            })
            {
                StatusCode = 200
            };
        }


    }
}
