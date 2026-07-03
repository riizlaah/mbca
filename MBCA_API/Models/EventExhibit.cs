using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("EventExhibit")]
    public class EventExhibit
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public int exhibitId { get; set; }

        public Exhibit exhibit { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }
}
