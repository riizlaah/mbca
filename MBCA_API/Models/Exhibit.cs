using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class Exhibit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Artist { get; set; } = null!;

    public string TimePeriod { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int ExhibitCategoryId { get; set; }

    public virtual ICollection<EventExhibit> EventExhibits { get; set; } = new List<EventExhibit>();

    public virtual ExhibitCategory ExhibitCategory { get; set; } = null!;

    public virtual ICollection<ExhibitTag> ExhibitTags { get; set; } = new List<ExhibitTag>();
}
