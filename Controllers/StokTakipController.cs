using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokTakipWebApi.Auth;
using StokTakipWebApi.Models;
using StokTakipWebApi.Protocol;
using System.Security.Claims;

namespace StokTakipWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[action]")]
    public class StokTakipController : Controller
    {
        StokTakipDbContext context = new StokTakipDbContext();

        [AllowAnonymous]
        [HttpGet]
        public string Test()
        {
            return "Api ile Bağlantı çalıştı";
        }

        [AllowAnonymous]
        [HttpPost]
        public ApiCevap Login(string kullaniciAdi, string parola)
        {
            ApiCevap cevap = new ApiCevap();
            var kullanici = context.TblKullanicilars.FirstOrDefault(x => x.KullaniciAd == kullaniciAdi &&
            x.Parola == parola);

            if(kullanici == null)
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Kullanıcı Adı yada Parola hatalı.";
                //cevap.Data = null 
                return cevap;
            }

            string token = OturumYoneticisi.BuildToken(kullanici);

            cevap.BasariliMi = true;
            cevap.Data = token;
            return cevap;
        }


        [HttpGet]
        public ApiCevap KullanicilariGetir()
        {
            ApiCevap cevap = new ApiCevap();

            var list = context.TblKullanicilars.ToList();

            cevap.BasariliMi = true;
            cevap.Data = list;

            return cevap;
        }

        [HttpPost]
        public ApiCevap KullaniciEkle(string kullaniciAd, string parola, int yetki)
        {

            int user_yetki = Convert.ToInt32(User.Claims.Where(c => c.Type == "yetki").Select(c => c.Value).FirstOrDefault());
            
            ApiCevap cevap = new ApiCevap();

            if(yetki != 0)//Yönetici Değilse
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Bu işlem için yetkiniz yok.";
                return cevap;
            }


            TblKullanicilar kullanici = new TblKullanicilar()
            {
                KullaniciAd = kullaniciAd,
                Parola = parola,
                Yetki = yetki
            };

            context.TblKullanicilars.Add(kullanici);
            context.SaveChanges();

            cevap.BasariliMi = true;
            cevap.Data = kullanici;

            return cevap;
        }

        [HttpGet]
        public ApiCevap KategorileriGetir()
        {
            ApiCevap cevap = new ApiCevap();
            var list = context.TblKategorilers.ToList();
            cevap.BasariliMi = true;
            cevap.Data = list;

            return cevap;
        }

        [HttpPost]
        public ApiCevap KategoriEkle(string kategoriAdi)
        {
            ApiCevap cevap = new ApiCevap();
            TblKategoriler kategori = new TblKategoriler()
            {
                KategoriAdi = kategoriAdi
            };

            context.TblKategorilers.Add(kategori);
            context.SaveChanges();

            cevap.BasariliMi = true;
            cevap.Data = kategori;

            return cevap;
        }

        [HttpPost]
        public ApiCevap KategoriSil(int kategoriId)
        {
            ApiCevap cevap=new ApiCevap();

            var kategori = context.TblKategorilers.FirstOrDefault(x=>x.KategaoriId == kategoriId);
            
            if(kategori == null)//olmayan bir kategoriyi silemem
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Olmayan bir kategoriId gönderdiniz.";
                return cevap;
            }

            context.TblKategorilers.Remove(kategori);
            context.SaveChanges();
            cevap.BasariliMi = true;

            return cevap;

        }

        [HttpPost]
        public ApiCevap KategoriGuncelle(int kategoriId, string kategoriAdi)
        {
            ApiCevap cevap=new ApiCevap();

            var kategori = context.TblKategorilers.FirstOrDefault(x => x.KategaoriId == kategoriId);
        
            if(kategori == null)//olmayan bir kategoriyi güncelleyemem
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Olmayan bir kategoriId gönderdiniz.";
                return cevap;
            }

            kategori.KategoriAdi = kategoriAdi;
            context.SaveChanges();
            cevap.BasariliMi=true;
            cevap.Data = kategori;

            return cevap;
        }

        [HttpGet]
        public ApiCevap UrunleriGetir()
        {
            ApiCevap cevap = new ApiCevap();
            var list = context.TblUrunlers.ToList();
            cevap.BasariliMi = true;
            cevap.Data = list;

            return cevap;
        }

        [HttpPost]
        public ApiCevap UrunEkle(string kod, int kategoriId, string ad, int birimId, string aciklama)
        {
            ApiCevap cevap = new ApiCevap();
            TblUrunler urun = new TblUrunler()
            {
                UrunKodu = kod,
                KategoriId = kategoriId,
                UrunAd = ad,
                BirimId = birimId,
                UrunAciklama = aciklama
            };

            context.TblUrunlers.Add(urun);
            context.SaveChanges();

            cevap.BasariliMi = true;
            cevap.Data = urun;

            return cevap;
        }

        [HttpPost]
        public ApiCevap UrunSil(int urunId)
        {
            ApiCevap cevap = new ApiCevap();

            var urun = context.
                TblUrunlers.FirstOrDefault(x => x.UrunId == urunId);

            if (urun == null)//olmayan bir kategoriyi silemem
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Olmayan bir urunId gönderdiniz.";
                return cevap;
            }

            context.TblUrunlers.Remove(urun);
            context.SaveChanges();
            cevap.BasariliMi = true;

            return cevap;

        }

        [HttpPost]
        public ApiCevap UrunGuncelle(int urunId, string kod, int kategoriId, string ad, int birimId, string aciklama, float minStok, float maksStok)
        {
            ApiCevap cevap = new ApiCevap();

            var urun = context.TblUrunlers.FirstOrDefault(x => x.UrunId == urunId);

            if (urun == null)//olmayan bir kategoriyi güncelleyemem
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Olmayan bir UrunId gönderdiniz.";
                return cevap;
            }

            urun.UrunKodu = kod;
            urun.KategoriId = kategoriId;
            urun.BirimId = birimId;
            urun.UrunAd = ad;
            urun.UrunAciklama = aciklama;
            urun.MinStok = minStok;
            urun.MaksStok = maksStok;

            context.SaveChanges();
            cevap.BasariliMi = true;
            cevap.Data = urun;

            return cevap;
        }


        [HttpGet]
        public ApiCevap MusterileriGetir()
        {
            ApiCevap cevap = new ApiCevap();
            var list = context.TblMusterilers.ToList();
            cevap.BasariliMi = true;
            cevap.Data = list;

            return cevap;
        }

        [HttpPost]
        public ApiCevap MusteriEkle(string firmaAdi, string yetkiliAdSoyad, string telefon, string mail, string adres)
        {
            ApiCevap cevap = new ApiCevap();

            TblMusteriler musteri = new TblMusteriler()
            {
                FirmaAdi = firmaAdi,
                YetkiliAdSoyad = yetkiliAdSoyad,
                Telefon = telefon,
                Mail = mail,
                Adres = adres
            };

            context.TblMusterilers.Add(musteri);
            context.SaveChanges();

            cevap.BasariliMi = true;
            cevap.Data = musteri;

            return cevap;
        }

        [HttpPost]
        public ApiCevap MusteriGuncelle(int musteriId, string firmaAdi, string yetkiliAdSoyad, string telefon, string mail, string adres)
        {
            ApiCevap cevap = new ApiCevap();

            var musteri = context.TblMusterilers.FirstOrDefault(x => x.MusteriId == musteriId);

            if (musteri == null)//olmayan bir müşteri güncelleyemem
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Olmayan bir MusteriId gönderdiniz.";
                return cevap;
            }

            musteri.FirmaAdi = firmaAdi;
            musteri.YetkiliAdSoyad = yetkiliAdSoyad;
            musteri.Telefon = telefon;
            musteri.Mail = mail;
            musteri.Adres = adres;
           
            context.SaveChanges();
            cevap.BasariliMi = true;
            cevap.Data = musteri;

            return cevap;
        }

        [HttpPost]
        public ApiCevap MusteriSil(int musteriId)
        {
            ApiCevap cevap = new ApiCevap();

            var musteri = context.
                TblMusterilers.FirstOrDefault(x => x.MusteriId == musteriId);

            if (musteri == null)//olmayan bir musteri silemem
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Olmayan bir MusteriId gönderdiniz.";
                return cevap;
            }

            context.TblMusterilers.Remove(musteri);
            context.SaveChanges();
            cevap.BasariliMi = true;

            return cevap;

        }

    }

}
