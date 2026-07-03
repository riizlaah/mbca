using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("Role")]
    public class Role
    {
        public int id { get; set; }
        public string name { get; set; } = null!;

        public ICollection<User> users { get; set; } = null!;
    }
}
