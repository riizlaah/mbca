using System.ComponentModel.DataAnnotations.Schema;

namespace MBCA_API.Models
{
    [Table("Ticket")]
    public class Ticket
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public int userId { get; set; }
        public int? promoId { get; set; }
        public DateTime transactionDate { get; set; }
        public int qty { get; set; }
        public decimal totalPrice { get; set; }

        public User user { get; set; } = null!;
        public Event Event { get; set; } = null!;
        public Promo? promo { get; set; }
    }
}
