using System;
using System.Collections.Generic;

namespace Genealogy.Models;

public partial class RelationshipTable
{
    public string Id { get; set; } = null!;

    public string MainMemId { get; set; } = null!;

    public string SubMemId { get; set; } = null!;

    public byte RelateCode { get; set; }

    public DateTime? DateStart { get; set; }

    public virtual MemberTable MainMem { get; set; } = null!;

    public virtual MemberTable SubMem { get; set; } = null!;
}
