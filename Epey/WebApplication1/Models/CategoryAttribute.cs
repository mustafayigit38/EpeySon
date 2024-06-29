using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class CategoryAttribute
{
    public int CategoryAttributeId { get; set; }

    public int CategoryId { get; set; }

    public int AttributeId { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
