using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Brand
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
