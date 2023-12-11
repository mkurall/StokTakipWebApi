using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StokTakipWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[action]")]
    public class StokTakipController : Controller
    {
        static List<Ogrenci> ogreciler = new List<Ogrenci>
        (   new[]{
            new Ogrenci(){Nu=5,Ad="Abdi",Soyad="Şenol"},
            new Ogrenci(){Nu=15,Ad="Başar",Soyad="Acaroğlu"},
            new Ogrenci(){Nu=41,Ad="Hakan", Soyad="Volkan"},
             new Ogrenci(){Nu=1231,Ad="Serkan", Soyad="Zirek"},
              new Ogrenci(){Nu=2500,Ad="Sinan", Soyad="Ürün"},
        });


        [HttpGet]
        public string Test()
        {
            return "Api ile Bağlantı çalıştı";
        }

        [HttpGet]
        public string Listele()
        {
            return JsonConvert.SerializeObject(ogreciler);
        }

        [HttpPost]
        public string OgrenciEkle(Ogrenci ogr)
        {
            ogreciler.Add(ogr);
            return "Öğrenci Ekşlendi";
        }
    }

    public class Ogrenci
    {
        public int Nu { get; set; }

        public string Ad { get; set; }
        public string Soyad { get; set; }
    }
}
