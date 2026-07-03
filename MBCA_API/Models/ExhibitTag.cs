using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("ExhibitTags")]
    public class ExhibitTag
    {
        public int id { get; set; }
        public int exhibitId { get; set; }
        public string tag { get; set; } = null!;

        public Exhibit exhibit { get; set; } = null!;
    }
}
