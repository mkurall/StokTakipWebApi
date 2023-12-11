using System;
using System.Collections.Generic;

namespace StokTakipWebApi.Models;

public partial class TblKategoriler
{
    public int KategaoriId { get; set; }

    public string KategoriAdi { get; set; } = null!;

    public virtual ICollection<TblUrunler> TblUrunlers { get; set; } = new List<TblUrunler>();
}
