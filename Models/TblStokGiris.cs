﻿using System;
using System.Collections.Generic;

namespace StokTakipWebApi.Models;

public partial class TblStokGiris
{
    public int IslemId { get; set; }

    public int UrunId { get; set; }

    public int TedarikciId { get; set; }

    public double BirimFiyat { get; set; }

    public double Adet { get; set; }

    public DateTime Tarih { get; set; }

    public int IslemYapanPersonelId { get; set; }

    public virtual TblKullanicilar IslemYapanPersonel { get; set; } = null!;

    public virtual TblTedarikciler Tedarikci { get; set; } = null!;

    public virtual TblUrunler Urun { get; set; } = null!;
}
