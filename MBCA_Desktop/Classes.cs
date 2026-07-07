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



}
