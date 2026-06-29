using System;
using System.Collections.Generic;

namespace MBCA_API.Models;

public partial class Otp
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Code { get; set; } = null!;

    public long ValidUntil { get; set; }

    public DateTime validUntildt => DateTime.FromBinary(ValidUntil);

    public virtual User User { get; set; } = null!;
}
