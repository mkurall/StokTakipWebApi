using Microsoft.AspNetCore.Mvc;

namespace StokTakipWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[action]")]
    public class StokTakipController : Controller
    {
        [HttpGet]
        public string Test()
        {
            return "Api ile Bağlantı çalıştı";
        }
    }
}
