using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MBCA_Desktop
{
    internal class Helper
    {
        public static HttpClient _client { get; set; } = new HttpClient();
        public static string token { get; set; } = "";
        public const string addr = "http://localhost:5000/";
        public static ProfileRes? profile { get; set; } = null;

        async public static Task<(bool success, APIRes<TRes> res, string message)> jsonReq<TRes, TReq>(string route, string method = "get", TReq? req = default)
        {
            if (token != "") _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage? msg = null;
            var url = addr + "mbca-api/v1/" + route;
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
            Console.WriteLine("Status Code :", msg?.StatusCode);
            return (false, new APIRes<TRes> { message = "Response parsing failed" }, "Response parsing failed");
        }

        async public static Task<(bool success, APIRes<object> res, string message)> multipartReq(string route, string method, MultipartFormDataContent req)
        {
            if (token != "") _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage? msg;
            var url = addr + "mbca-api/v1/" + route;
            if (method.ToLower() == "post") msg = await _client.PostAsync(url, req);
            else if (method.ToLower() == "put") msg = await _client.PutAsync(url, req);
            else msg = await _client.PatchAsync(url, req);
            var data = await msg.Content.ReadFromJsonAsync<APIRes<object>>();
            if (data != null)
            {
                var message = data?.message ?? "Unknown error";
                return (msg.IsSuccessStatusCode, data, message);
            }
            return (false, new APIRes<object> { message = "Response parsing failed" }, "Response parsing failed");
        }

        async public static Task<Image?> GetImage(string path)
        {
            var url = addr + $"uploads/{path}";
            Debug.WriteLine(url);
            try
            {
                var bytes = await _client.GetByteArrayAsync(url);
                using (var ms = new MemoryStream(bytes))
                {
                    var image = Image.FromStream(ms);
                    return image;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        public static void LockWindow(Form window)
        {
            window.MinimumSize = window.Size;
            window.MaximumSize = window.Size;
            window.MaximizeBox = false;
            window.MinimizeBox = false;
        }

        public static void GenTableColumns(DataGridView table, string[] headers, string[] bindings)
        {
            table.AutoGenerateColumns = false;
            table.RowHeadersVisible = false;
            for (var i = 0; i < headers.Length; i++)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    HeaderText = headers[i],
                    Name = headers[i],
                    ReadOnly = true,
                    DataPropertyName = bindings[i],
                };
                table.Columns.Add(col);
            }
            //table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }
    }

    public class APIRes<T>
    {
        public T? data { get; set; } = default;
        public string message { get; set; } = "";
    }

}
