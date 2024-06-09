using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class User
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
