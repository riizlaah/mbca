using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class ExhibitCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Exhibit> Exhibits { get; set; } = new List<Exhibit>();
}
