using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("Exhibit")]
    public class Exhibit
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public string artist { get; set; } = null!;
        public string timePeriod { get; set; } = null!;
        public string image { get; set; } = null!;
        public int exhibitCategoryId { get; set; }

        public ExhibitCategory exhibitCategory { get; set; } = null!;
        public ICollection<ExhibitTag> exhibitTags { get; set; } = new List<ExhibitTag>();
        public ICollection<EventExhibit> eventExhibits { get; set; } = new List<EventExhibit>();
    }
}
