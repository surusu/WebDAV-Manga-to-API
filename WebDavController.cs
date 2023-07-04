using MannaServer;
using Microsoft.Extensions.Options;
using System.Net;
using WebDav;

namespace WebDavManga
{
    public class WebDavController
    {
        /*private static readonly WebDAVConfig _appSettings;
        public static readonly WebDavClient client;
        public static readonly string dir;

        static WebDavController()
        {
            _appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("WebDAV")
                .Get<WebDAVConfig>();

            client = new WebDavClient(new WebDavClientParams
            {
                BaseAddress = new Uri("https://webdav.yandex.ru"),
                Credentials = new NetworkCredential(_appSettings.WebDAV.Login, _appSettings.WebDAV.Password)
            });

            dir = _appSettings.WebDAV.Directory;
        }*/

        private readonly WebDAVConfig _appSettings;

        public WebDavController(IOptions<WebDAVConfig> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public static WebDavClient client = new WebDavClient(new WebDavClientParams
        {
            BaseAddress = new Uri(CustomConfig.GetValue("URL")),
            Credentials = new NetworkCredential(CustomConfig.GetValue("Login"), CustomConfig.GetValue("Password"))
        });

        //public static string dir = "/Media/NewManga/";
        public static string dir = CustomConfig.GetValue("Directory");
        
        
        public static string Combine(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }
    }
}
