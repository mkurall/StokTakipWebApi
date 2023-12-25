using Microsoft.IdentityModel.Tokens;
using StokTakipWebApi.Models;
using StokTakipWebApi.Yardimci;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StokTakipWebApi.Auth
{
    public class OturumYoneticisi
    {
        public static Claim[] BuildClaims(TblKullanicilar kullanici)
        {
            System.Security.Claims.Claim[] claims =
            {
                new System.Security.Claims.Claim("kullaniciid", kullanici.KullaniciId.ToString()),
                new System.Security.Claims.Claim("kullaniciad",kullanici.KullaniciAd),
                new System.Security.Claims.Claim("yetki", kullanici.Yetki.ToString()),
                
            };

            return claims;
        }

        public static string BuildToken(TblKullanicilar account)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigYardimcisi.AppSetting["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime now = DateTime.Now;

            System.Security.Claims.Claim[] claims = BuildClaims(account);

            var token = new JwtSecurityToken(
                ConfigYardimcisi.AppSetting["Jwt:Issuer"],
              ConfigYardimcisi.AppSetting["Jwt:Issuer"],
              claims,
              notBefore: now,
              expires: now.AddDays(7),
              signingCredentials: creds);



            string access = new JwtSecurityTokenHandler().WriteToken(token);

           

            return access;
        }
    }
}
