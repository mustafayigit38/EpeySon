using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Kategori
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public virtual ICollection<KategoriOzellik> KategoriOzelliks { get; set; } = new List<KategoriOzellik>();

    public virtual ICollection<Ürün> Ürüns { get; set; } = new List<Ürün>();
}
