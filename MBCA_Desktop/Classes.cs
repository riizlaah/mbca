using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBCA_Desktop
{
    public class LoginReq
    {
        public string usernameOrEmail { get; set; } = "";
        public string password { get; set; } = "";
    }

    public class LoginRes
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string token { get; set; }
        public bool isActivated { get; set; }
    }


    public class ProfileRes
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }
        public bool isActivated { get; set; }
    }


    public class OTPRes
    {
        public int userId { get; set; }
        public bool isActivated { get; set; }
        public string username { get; set; }
        public string fullName { get; set; }
        public string? code { get; set; }
        public DateTime? validUntil { get; set; }
    }


    public class Exhibit
    {
        public int id { get; set; }
        public string name { get; set; }
        public string artist { get; set; }
        public ExCategory category { get; set; }
        public string categoryName => category.name;
        public string timePeriod { get; set; }
        public string image { get; set; }
        public List<ExTag> tags { get; set; }
        public string tagsStr => string.Join(", ", tags.Select(t => t.tag));

        public string shortDetail => $"\"{name}\" by {artist}";
    }

    public class ExCategory
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ExTag
    {
        public int id { get; set; }
        public string tag { get; set; }
    }


    public class Event
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateOnly date { get; set; }
        public TimeOnly startTime { get; set; }
        public TimeOnly endTime { get; set; }
        public string dateNTime => $"{date:dd-MM-yyyy}, {startTime:hh:mm} - {endTime:hh:mm}";
        public string location { get; set; }
        public string initiator { get; set; }
        public decimal price { get; set; }
        public EvCategory category { get; set; }
        public string categoryName => category.name;
        public List<Banner> banners { get; set; }
    }

    public class EvCategory
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Banner
    {
        public int id { get; set; }
        public string banner { get; set; }
    }




}
