using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class KategoriÖzellikÜrünDeğer
{
    public int Id { get; set; }

    public int KategoriÖzellikId { get; set; }

    public int ÜrünId { get; set; }

    public string Değer { get; set; } = null!;

    public virtual KategoriOzellik KategoriÖzellik { get; set; } = null!;

    public virtual Ürün Ürün { get; set; } = null!;
}
