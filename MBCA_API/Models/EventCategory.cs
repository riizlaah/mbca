using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("EventCategory")]
    public class EventCategory
    {
        public int id { get; set; }
        public string name { get; set; } = null!;

        public ICollection<Event> events { get; set; } = new List<Event>();
    }
}
