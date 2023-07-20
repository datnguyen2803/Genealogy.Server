using System;
using System.Collections.Generic;

namespace Genealogy.Models;

public partial class UserTable
{
    public static readonly bool ROLE_ADMIN = true;

    public static readonly bool ROLE_NORMAL = false;

    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Role { get; set; }

    public virtual ICollection<ServerEventTable> ServerEventTables { get; set; } = new List<ServerEventTable>();
}
