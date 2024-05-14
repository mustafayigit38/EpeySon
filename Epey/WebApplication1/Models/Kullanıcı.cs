using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Kullanıcı
{
    public int Id { get; set; }

    public string İsim { get; set; } = null!;

    public string Soyİsim { get; set; } = null!;

    public string EPosta { get; set; } = null!;

    public string Sifre { get; set; } = null!;

    public int Tarih { get; set; }
}
