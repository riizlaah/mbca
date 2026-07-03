using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("OTP")]
    public class OTP
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string code { get; set; } = null!;
        public DateTime validUntil { get; set; }

        public User user { get; set; } = null!;
    }
}
