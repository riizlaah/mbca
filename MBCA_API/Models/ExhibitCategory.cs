using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("ExhibitCategory")]
    public class ExhibitCategory
    {
        public int id { get; set; }
        public string name { get; set; } = null!;

        public ICollection<Exhibit> exhibits { get; set; } = new List<Exhibit>();

    }
}
