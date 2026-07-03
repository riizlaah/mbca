using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("User")]
    public class User
    {
        public int id { get; set; }
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
        public string fullName { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phoneNumber { get; set; } = null!;
        public int roleId { get; set; }
        public bool isActivated { get; set; } = false;

        public Role role { get; set; } = null!;
        public ICollection<Ticket> tickets { get; set; } = null!;
        public ICollection<OTP> otps { get; set; } = null!;
    }
}
