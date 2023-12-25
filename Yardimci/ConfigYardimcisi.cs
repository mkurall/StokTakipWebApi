namespace StokTakipWebApi.Yardimci
{
    public class ConfigYardimcisi
    {
        public static IConfiguration AppSetting { get; }
        static ConfigYardimcisi()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }
}
