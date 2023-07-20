using System;
using System.Collections.Generic;

namespace Genealogy.Models;

public partial class ClanEventTable
{
    public string Id { get; set; } = null!;

    public string MainMemId { get; set; } = null!;

    public string SubMemId { get; set; } = null!;

    public byte Type { get; set; }

    public DateTime TimeOccur { get; set; }

    public string? Detail { get; set; }

    public virtual MemberTable MainMem { get; set; } = null!;

    public virtual MemberTable SubMem { get; set; } = null!;
}
