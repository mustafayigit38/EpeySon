using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class AttributeSpec
{
    public int AttributeSpecsId { get; set; }

    public string AttributeSpecsName { get; set; } = null!;

    public virtual ICollection<Attribute> Attributes { get; set; } = new List<Attribute>();
}
