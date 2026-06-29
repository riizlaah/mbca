using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class PhonePrefix
{
    public int Id { get; set; }

    public string Prefix { get; set; } = null!;
}
