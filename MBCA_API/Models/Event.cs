using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("Event")]
    public class Event
    {
        public int id { get; set; }
        public string title { get; set; } = null!;
        public string description { get; set; } = null!;
        public DateOnly date { get; set; }
        public TimeOnly startTime { get; set; }
        public TimeOnly endTime { get; set; }
        public string location { get; set; } = null!;
        public string initiator { get; set; } = null!;
        public decimal price { get; set; }
        public int eventCategoryId { get; set; }

        public EventCategory eventCategory { get; set; } = null!;
        public ICollection<EventBanner> eventBanners { get; set; } = new List<EventBanner>();
        public ICollection<EventExhibit> eventExhibits { get; set; } = new List<EventExhibit>();
        public ICollection<Ticket> tickets { get; set; } = new List<Ticket>();
    }
}
