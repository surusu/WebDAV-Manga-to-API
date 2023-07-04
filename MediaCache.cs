using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace WebDavManga
{
    public class MediaCache
    {

        //public static string CacheDir = Path.Combine(AppContext.BaseDirectory, ".cache");
        

        /*

        public static bool SetFile(byte[] bytes, string category, string name)
        {
            switch (category)
            {
                case "cover":
                    try
                    {
                        string getPath = Path.Combine(CacheDir, "cover", name + ".jpg");
                        EnsureFilePathExists(getPath);

                        File.WriteAllBytes(getPath, bytes);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    
                    break;
                default:
                    return false;
                    break;
            }
            
        }*/

        /*ublic static async Task<bool> SetStreamFile(Stream stream, string category, string name)
        {
            switch (category)
            {
                case "cover":
                    try
                    {
                        string getPath = Path.Combine(CacheDir, "cover", name + ".jpg");
                        EnsureFilePathExists(getPath);

                        using (var fileStream = new FileStream(getPath, FileMode.Create, FileAccess.Write))
                        {
                            await stream.CopyToAsync(fileStream);
                        }

                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    break;
                default:
                    return false;
                    break;
            }

        }

        public static byte[] GetFile(string category, string name)
        {
            switch (category)
            {
                case "cover":
                    string getPath = Path.Combine(CacheDir, "cover", name + ".jpg");
                    EnsureFilePathExists(getPath);
                    return File.ReadAllBytes(getPath);
                    break;
                default:
                    return null;
                    break;
            }

            
        }*/

        /*public static bool DoesFileExist(string category, string name)    //Проверка существования файла
        {
            switch (category)
            {
                case "cover":
                    string getPath = Path.Combine(CacheDir, "cover", name + ".jpg");
                    Console.WriteLine("DoesFileExist: " + getPath);
                    EnsureFilePathExists(getPath);
                    return File.Exists(getPath);
                    break;
                default:
                    return false;
                    break;
            }
            
        }


        public static bool MemoryOverflow()
        {




            return false;
        }*/

        

    }
}
