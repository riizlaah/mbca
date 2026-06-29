using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class EventExhibit
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int ExhibitId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Exhibit Exhibit { get; set; } = null!;
}
