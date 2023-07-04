using Microsoft.Extensions.Hosting.WindowsServices;
using System.Diagnostics;
using System.Net;
using System.Text;
using WebDav;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebDavManga
{
    public class Program
    {
        //public static void Main(string[] args)
        public static async Task Main(string[] args)
        {
            var options = new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = WindowsServiceHelpers.IsWindowsService()
                                     ? AppContext.BaseDirectory : default
            };

            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = WebApplication.CreateBuilder(options);
            //var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            //builder.Services.AddWindowsService

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.Host.UseWindowsService();

            /**builder.Services.Configure<WebDAVConfig>(
                builder.Configuration.GetSection("WebDAV")
            );*/

            //builder.Services.Configure<WebDAVConfig>(builder.Configuration.GetSection("WebDAV"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            Console.WriteLine("Cache directory:" + CacheFile.CacheDir);

            app.Use(async (context, next) =>
            {
                string msg = context.Request.Protocol
                  + " " + context.Request.Method
                  + " : " + context.Request.Path;
                string sep = new String('-', msg.Length);
                Console.WriteLine(sep
                    + Environment.NewLine
                    + msg
                    + Environment.NewLine
                    + sep);

                /*foreach (string key in context.Request.Headers.Keys)
                {
                    Console.WriteLine(key + " = "
                        + context.Request.Headers[key]);
                }

                foreach (string key in context.Request.Cookies.Keys)
                {
                    Console.WriteLine(key + " : " + context.Request.Cookies[key]);
                }

                if (context.Request.Body != null)
                {
                    string body = String.Empty;

                    using (StreamReader sr =
                      new StreamReader(context.Request.Body))
                    {
                        body = sr.ReadToEndAsync().Result;
                    }

                    Console.WriteLine(body);
                    context.Request.Body =
                      new MemoryStream(Encoding.UTF8.GetBytes(body));
                    context.Request.Body.Position = 0;
                }*/

                await next.Invoke();
            });

            await app.RunAsync();

            //app.Run();

            /*
            if (isService)
            {
                app.RunAsService();
            }
            else
            {
                app.Run();
            }*/
        }
    }
}