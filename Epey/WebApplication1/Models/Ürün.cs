using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Ürün
{
    public int Id { get; set; }

    public int KategoriId { get; set; }

    public string Ad { get; set; } = null!;

    public int Fiyat { get; set; }

    public string Acıklama { get; set; } = null!;

    public int Puan { get; set; }

    public string Resim { get; set; } = null!;

    public int MarkaId { get; set; }

    public virtual Kategori Kategori { get; set; } = null!;

    public virtual ICollection<KategoriÖzellikÜrünDeğer> KategoriÖzellikÜrünDeğers { get; set; } = new List<KategoriÖzellikÜrünDeğer>();

    public virtual Marka Marka { get; set; } = null!;
}
