using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
