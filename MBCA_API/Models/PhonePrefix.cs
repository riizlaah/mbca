using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("PhonePrefix")]
    public class PhonePrefix
    {
        public int id { get; set; }
        public string prefix { get; set; } = null!;
    }
}
