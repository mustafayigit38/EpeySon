using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Attribute
{
    public int AttributeId { get; set; }

    public string AttributeName { get; set; } = null!;

    public string AttributeType { get; set; } = null!;

    public int AttributeSpec { get; set; }

    public virtual AttributeSpec AttributeSpecNavigation { get; set; } = null!;

    public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();

    public virtual ICollection<Value> Values { get; set; } = new List<Value>();
}
