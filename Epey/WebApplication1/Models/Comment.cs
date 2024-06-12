using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Phone Product { get; set; } = null!;
}
