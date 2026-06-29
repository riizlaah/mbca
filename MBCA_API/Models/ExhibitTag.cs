using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class ExhibitTag
{
    public int Id { get; set; }

    public int ExhibitId { get; set; }

    public string Tag { get; set; } = null!;

    public virtual Exhibit Exhibit { get; set; } = null!;
}
