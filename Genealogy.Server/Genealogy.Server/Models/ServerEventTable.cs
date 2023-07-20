using System;
using System.Collections.Generic;

namespace Genealogy.Models;

public partial class ServerEventTable
{
    public string Id { get; set; } = null!;

    public string Actuator { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string RecordId { get; set; } = null!;

    public byte Type { get; set; }

    public DateTime TimeOccur { get; set; }

    public string Detail { get; set; } = null!;

    public virtual UserTable ActuatorNavigation { get; set; } = null!;
}
