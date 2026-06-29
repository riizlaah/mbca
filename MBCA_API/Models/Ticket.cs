using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public DateTime TransactionDate { get; set; }

    public int EventId { get; set; }

    public int UserId { get; set; }

    public int Qty { get; set; }

    public int? PromoId { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Promo? Promo { get; set; }

    public virtual User User { get; set; } = null!;
}
