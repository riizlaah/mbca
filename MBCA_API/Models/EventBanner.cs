using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("EventBanner")]
    public class EventBanner
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public string banner { get; set; } = null!;

        public Event Event { get; set; } = null!;
    }
}
