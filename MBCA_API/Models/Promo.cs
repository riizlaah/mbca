namespace MBCA_API.Models
{
    public class Promo
    {
        public int id { get; set; }
        public string code { get; set; } = null!;
        public decimal discountPercentage { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }

        public ICollection<Ticket> tickets { get; set; } = new List<Ticket>();
    }
}
