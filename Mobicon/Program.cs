using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Mobicon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // https://sentry.io/grigory-perepechko/mobicon/
                // Credentials: mylce@ya.ru / Mobicon1
                .UseSentry("https://0eb99ad19d364d89876e76c19b7f3afc@sentry.io/1282941")
                .UseStartup<Startup>();
    }
}
