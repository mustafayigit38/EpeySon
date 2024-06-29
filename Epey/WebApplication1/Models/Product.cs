using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int ProductCategory { get; set; }

    public string? ProductCreatedAt { get; set; }

    public string? ProductImage { get; set; }

    public virtual Category ProductCategoryNavigation { get; set; } = null!;

    public virtual ICollection<Value> Values { get; set; } = new List<Value>();
}
