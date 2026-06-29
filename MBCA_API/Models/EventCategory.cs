using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class EventCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
