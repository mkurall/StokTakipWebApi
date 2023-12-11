﻿using System;
using System.Collections.Generic;

namespace StokTakipWebApi.Models;

public partial class TblKullanicilar
{
    public int KullaniciId { get; set; }

    public string KullaniciAd { get; set; } = null!;

    public string Parola { get; set; } = null!;

    public int? Yetki { get; set; }

    public virtual ICollection<TblStokCiki> TblStokCikis { get; set; } = new List<TblStokCiki>();

    public virtual ICollection<TblStokGiris> TblStokGirises { get; set; } = new List<TblStokGiris>();
}
