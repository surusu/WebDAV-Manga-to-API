using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using WebDav;
using static WebDav.ApplyTo;

namespace WebDavManga.Controllers
{
    [Route("[controller]/")]
    [Route("api/[controller]/")]
    [ApiController]
    public class mediaController : ControllerBase
    {

        [HttpGet("{name}")]
        public async Task<IActionResult> GetImage(string? name, string? type, string? param1, string? param2)
        {
            //string webRootPath = _webHostEnvironment.WebRootPath;
            //string contentRootPath = _webHostEnvironment.ContentRootPath;
            Console.WriteLine("GetImage");
            Console.WriteLine("name: " + name);
            Console.WriteLine("type: " + type);

            switch (type)
            {
                case "cover":
                    string url = WebDavController.Combine(name, "cover.jpg");
                    Console.WriteLine("URL: " + WebDavController.Combine(WebDavController.dir, url));

                    byte[]? bytes = await CacheFile.Cover.Get(url, OtherClass.HashString(name).ToString() + ".jpg");
                    if (bytes == null)
                    {
                        return NotFound();
                    }

                    return new FileContentResult(bytes, "image/jpeg");
                    break;
                case "page":
                    Console.WriteLine("Type page image");
                    string filePath = Path.Combine(CacheFile.CacheDir, "Image", param1, param2, name);
                    Console.WriteLine(filePath);
                    CacheFile.FilePathExists(filePath);

                    if (System.IO.File.Exists(filePath))
                    {
                        Console.WriteLine("Loads file " + name + "' from local cache");
                        return new FileContentResult(await System.IO.File.ReadAllBytesAsync(filePath),OtherClass.GetContentType(name));
                    }

                    /*byte[]? bytes = await CacheFile.ImageDir.Get(url, OtherClass.HashString(name).ToString() + ".jpg");
                    if (bytes == null)
                    {
                        return NotFound();
                    }*/

                    //return new FileContentResult(bytes, "image/jpeg");

                    break;
                default:
                    break;
            }

            return NotFound();

            //return new FileContentResult(CacheFile.Cover.Get(url, name + ".jpg"), "image/jpeg");

            //return NotFound();

            /*
            if (type == "cover")
            {
                if (MediaCache.DoesFileExist(type, name))
                {
                    Console.WriteLine("Using Local Cache");
                    return new FileContentResult(MediaCache.GetFile(type, name), "image/jpeg");
                }
                else
                {
                    Console.WriteLine("Using WebDAV");
                    using (var response = await WebDavController.client.GetRawFile(WebDavController.Combine(WebDavController.dir, name + "/cover.jpg")))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        await response.Stream.CopyToAsync(memoryStream);

                        using (var copy1 = new MemoryStream(memoryStream.ToArray()))
                            MediaCache.SetStreamFile(copy1, type, name);

                        using (var copy2 = new MemoryStream(memoryStream.ToArray()))
                            return new FileContentResult(ReadToEnd(copy2), "image/jpeg");
                    }
                }
                */
        }



        //string path = Path.Combine(contentRootPath, "media");
        /*string path = "/Media/NewManga/";
        Console.WriteLine(path);
        Console.WriteLine(name);
        if (name.Split('/').Length > 1)
        {
            return NotFound();
        }
        else if (name.Split('_').Length <= 1)
        {
            path = Path.Combine(path, "image");
            path = Path.Combine(path, name);
        }
        else
        {
            switch (name.Split('_').First())
            {
                case "":
                    path = Path.Combine(path, "");
                    break;
                case "p":
                    path = Path.Combine(path, "cover");
                    break;
                case "u":
                    path = Path.Combine(path, "publishers");
                    break;
                case "a":
                    path = Path.Combine(path, "avatars");
                    break;
                default:
                    path = Path.Combine(path, "image");
                    break;
            }
            path = Path.Combine(path, name.Split('_', 2).Last());
        }*/
        //Console.Write(path);
        /*if (System.IO.File.Exists(path))
        {
            return base.PhysicalFile(path, "image/jpeg");
        }
        else
        {
            return NotFound();
        }

    }*/

        // A custom action result that wraps a Stream object
        /* public static byte[] ReadToEnd(System.IO.Stream stream)
         {
             long originalPosition = 0;

             if (stream.CanSeek)
             {
                 originalPosition = stream.Position;
                 stream.Position = 0;
             }

             try
             {
                 byte[] readBuffer = new byte[4096];

                 int totalBytesRead = 0;
                 int bytesRead;

                 while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                 {
                     totalBytesRead += bytesRead;

                     if (totalBytesRead == readBuffer.Length)
                     {
                         int nextByte = stream.ReadByte();
                         if (nextByte != -1)
                         {
                             byte[] temp = new byte[readBuffer.Length * 2];
                             Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                             Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                             readBuffer = temp;
                             totalBytesRead++;
                         }
                     }
                 }

                 byte[] buffer = readBuffer;
                 if (readBuffer.Length != totalBytesRead)
                 {
                     buffer = new byte[totalBytesRead];
                     Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                 }
                 return buffer;
             }
             finally
             {
                 if (stream.CanSeek)
                 {
                     stream.Position = originalPosition;
                 }
             }
         }*/


        [HttpGet("someone-site/")]
        public async Task<ActionResult> GetProxyImage(string? name)
        {

            string url = "https://api.someone-site.org/" + name;

            Console.WriteLine(url);

            using (var client = new HttpClient())
            {
                return new FileStreamResult(await client.GetStreamAsync(url), "image/jpeg");
            }

        }


    }
}
