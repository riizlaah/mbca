using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string Location { get; set; } = null!;

    public string Initiator { get; set; } = null!;

    public decimal Price { get; set; }

    public int EventCategoryId { get; set; }

    public virtual ICollection<EventBanner> EventBanners { get; set; } = new List<EventBanner>();

    public virtual EventCategory EventCategory { get; set; } = null!;

    public virtual ICollection<EventExhibit> EventExhibits { get; set; } = new List<EventExhibit>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
