using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Value
{
    public int ValueId { get; set; }

    public int ProductId { get; set; }

    public int AttributeId { get; set; }

    public string? ValueString { get; set; }

    public int? ValueInteger { get; set; }

    public double? ValueFloat { get; set; }

    public DateOnly? ValueDate { get; set; }

    public int? ValueBoolean { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
