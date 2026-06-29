using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class EventBanner
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public string Banner { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
