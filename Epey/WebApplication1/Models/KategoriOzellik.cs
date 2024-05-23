using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class KategoriOzellik
{
    public int Id { get; set; }

    public int KategoriId { get; set; }

    public string Ad { get; set; } = null!;

    public string Birim { get; set; } = null!;

    public virtual Kategori Kategori { get; set; } = null!;

    public virtual ICollection<KategoriÖzellikÜrünDeğer> KategoriÖzellikÜrünDeğers { get; set; } = new List<KategoriÖzellikÜrünDeğer>();
}
