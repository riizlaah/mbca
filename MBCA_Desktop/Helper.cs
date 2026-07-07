using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Diagnostics;

namespace MBCA_Desktop
{
    internal class Helper
    {
        public static HttpClient _client { get; set; } = new HttpClient();
        public static string token { get; set; } = "";
        public const string addr = "http://localhost:5000/mbca-api/v1/";
        public static ProfileRes? profile { get; set; } = null;

        async public static Task<(bool success, APIRes<TRes> res, string message)> jsonReq<TRes, TReq>(string route, string method = "get", TReq? req = default)
        {
            if (token != "") _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage msg;
            var url = addr + route;
            try
            {
                if (method.ToLower() == "get") msg = await _client.GetAsync(url);
                else if (method.ToLower() == "delete") msg = await _client.DeleteAsync(url);
                else if (method.ToLower() == "post") msg = await _client.PostAsJsonAsync(url, req);
                else if (method.ToLower() == "put") msg = await _client.PutAsJsonAsync(url, req);
                else msg = await _client.PatchAsJsonAsync(url, req);
                var data = await msg.Content.ReadFromJsonAsync<APIRes<TRes>>();
                if (data != null)
                {
                    var message = data?.message ?? "Unknown error";
                    return (msg.IsSuccessStatusCode, data, message);
                }
                return (false, new APIRes<TRes> { message = "Response parsing failed" }, "Response parsing failed");
            }
            catch (Exception ex)
            {
                return (false, new APIRes<TRes> { message = ex.Message }, ex.Message);
            }
        }



        public static void LockWindow(Form window)
        {
            window.MinimumSize = window.Size;
            window.MaximumSize = window.Size;
            window.MaximizeBox = false;
            window.MinimizeBox = false;
        }
    }

    public class APIRes<T>
    {
        public T? data { get; set; } = default;
        public string message { get; set; } = "";
    }

}
