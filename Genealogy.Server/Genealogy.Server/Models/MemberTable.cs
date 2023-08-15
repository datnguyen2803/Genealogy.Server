using System;
using System.Collections.Generic;

namespace Genealogy.Models;

public partial class MemberTable
{
    public string Id { get; set; } = null!;

    public string? Surname { get; set; } = null!;

    public string? Lastname { get; set; } = null!;

    public byte Gender { get; set; }

    public DateTime? Dob { get; set; }

    public DateTime? Dod { get; set; }

    public string? BirthPlace { get; set; }

    public string? CurrentPlace { get; set; }

    public bool IsClanLeader { get; set; }

    public ushort GenNo { get; set; }

    public string? Image { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<ClanEventTable> ClanEventTableMainMems { get; set; } = new List<ClanEventTable>();

    public virtual ICollection<ClanEventTable> ClanEventTableSubMems { get; set; } = new List<ClanEventTable>();

    public virtual ICollection<RelationshipTable> RelationshipTableMainMems { get; set; } = new List<RelationshipTable>();

    public virtual ICollection<RelationshipTable> RelationshipTableSubMems { get; set; } = new List<RelationshipTable>();
}
