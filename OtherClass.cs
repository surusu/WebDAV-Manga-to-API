using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace WebDavManga
{
    public class OtherClass
    {
        public static long HashString(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                /*if (SaveVal != "")
                    switch (SaveVal)
                    {
                        case "ID":
                            VarDirStorage.IDstorage.Save(BitConverter.ToInt64(hashBytes, 0).ToString(), input);
                            break;
                        case "Chapter":
                            VarDirStorage.ChaptersSorage.Save(BitConverter.ToInt64(hashBytes, 0).ToString(), input);
                            break;
                        default:
                            break;
                    }*/
                
                return BitConverter.ToInt64(hashBytes, 0);
            }
        }



        /*public static string WebDAVGetType(WebDav.WebDavResource value)
        {
            return value.Properties
                    .Where(p => p.Name == "{DAV:}getcontenttype")
                    .Select(p => p.Value)
                    .FirstOrDefault();
        }*/

        public static string UploadDateConvertor(WebDav.WebDavResource res)
        {
            string inputDate = res.Properties
                    .Where(p => p.Name == "{DAV:}getlastmodified")
                    .Select(p => p.Value)
                    .FirstOrDefault();
            //string inputDate = "Fri, 21 Apr 2023 19:13:39 GMT";
            DateTime date = DateTime.ParseExact(inputDate, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
            return date.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public static string GetContentType(string fileName)
        {
            // Create a new FileExtensionContentTypeProvider object
            var provider = new FileExtensionContentTypeProvider();

            // Try to get the content type for the file extension
            if (!provider.TryGetContentType(fileName, out string contentType))
            {
                // If the content type cannot be determined, use the default
                contentType = "application/octet-stream";
            }

            // Return the content type
            return contentType;
        }


    }
}
