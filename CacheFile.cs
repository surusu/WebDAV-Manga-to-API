using Microsoft.AspNetCore.Mvc;
//using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Xml.Linq;
using static WebDav.ApplyTo;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;

namespace WebDavManga
{
    public class CacheFile
    {

        private readonly string _folderPath;

        public CacheFile(string folderPath)
        {
            _folderPath = folderPath;
        }

        //private readonly IWebHostEnvironment _webHostEnvironment;
        public static string CacheDir = Path.Combine(AppContext.BaseDirectory, ".cache");
        
        //public static string CacheDir = Path.Combine(@"D:\Projects\WebDavManga\", ".cache");

        public static CacheFile Cover = new CacheFile(Path.Combine(CacheDir, "Cover"));
        public static CacheFile Chapter = new CacheFile(Path.Combine(CacheDir, "Chapter"));
        public static CacheFile ImageDir = new CacheFile(Path.Combine(CacheDir, "Image"));

        public async Task<byte[]> Get(string url, string? customName = null)
        {
            url = WebDavController.Combine(WebDavController.dir, url);
            string fileName = customName != null ? customName : Path.GetFileName(url);
            //string fileName = Path.GetFileName(url);

            string filePath = Path.Combine(_folderPath, fileName);
            FilePathExists(filePath);

            if (File.Exists(filePath))
            {
                Console.WriteLine("Loads file "+ fileName + "' from local cache");
                return await File.ReadAllBytesAsync(filePath);
            } else
            {
                Console.WriteLine("Downloads file '" + fileName + "' from the cloud");
                using (var response = await WebDavController.client.GetRawFile(url))
                {
                    if (response.IsSuccessful)
                    {
                        Console.WriteLine("Download Is Successful!");

                        MemoryStream memoryStream = new MemoryStream();
                        await response.Stream.CopyToAsync(memoryStream);

                        using (var copy1 = new MemoryStream(memoryStream.ToArray()))
                            try
                            {
                                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                {
                                    await copy1.CopyToAsync(fileStream);
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Error saving file");
                            }

                        using (var copy2 = new MemoryStream(memoryStream.ToArray()))
                            return ReadToEnd(copy2);
                    } else
                    {
                        Console.WriteLine("Download Is not Successful!");
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(response.Description);
                        return null;
                    }
                    
                }
            }
        }

        /*public class ImageRoot
        {
            public long id { get; set; }
            public string name { get; set; }
            public string origial_name { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }*/

        public async Task<List<CacheInfo.Pages>> GetZipImage(string url, string title_dir)
        {
            var imageList = new List<CacheInfo.Pages>();
            url = WebDavController.Combine(WebDavController.dir, url);
            string fileName = Path.GetFileName(url);
            string filePath = Path.Combine(_folderPath,"Empty zip", OtherClass.HashString(fileName).ToString());

            FilePathExists(filePath);

            if (File.Exists(filePath))
            {
                Console.WriteLine("The images have already been unpacked!");
                imageList.Add(new CacheInfo.Pages
                {
                    id = 0,
                    name = "ALREADY",
                    origial_name = "",
                    height = 0,
                    width = 0
                });
            }
            else
            {
                Console.WriteLine("Downloads file '" + fileName + "' from the cloud");
                using (var response = await WebDavController.client.GetRawFile(url))
                {
                    if (response.IsSuccessful)
                    {

                        string DirPath = Path.Combine(_folderPath, title_dir);

                        if (!Directory.Exists(DirPath))
                            Directory.CreateDirectory(DirPath);

                        Console.WriteLine("Download Is Successful!");

                        // Create a new zip archive object from the input stream
                        using ZipArchive archive = new ZipArchive(response.Stream, ZipArchiveMode.Read);

                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            Console.WriteLine(entry.FullName);

                            // Construct the full path to the entry's destination file

                            string nameWithoutExtension = Path.GetFileNameWithoutExtension(entry.Name);
                            string newFilename = OtherClass.HashString(nameWithoutExtension).ToString() + Path.GetExtension(entry.FullName);

                            string destinationFilePath = Path.Combine(DirPath, newFilename);

                            if (File.Exists(destinationFilePath))
                                File.Delete(destinationFilePath);

                            // Extract the entry to the destination file
                            await using (Stream entryStream = entry.Open()) {

                                await using FileStream destinationStream = File.Create(destinationFilePath);
                                await entryStream.CopyToAsync(destinationStream);
                            }

                            if (IsImageFile(entry.Name))
                            {
                                var (Imgidth, Imgheight) = await GetImageDimensions(destinationFilePath);

                                // Use the dimensions in your code
                                Console.WriteLine($"The image file has dimensions {Imgidth}x{Imgheight}.");

                                imageList.Add(new CacheInfo.Pages
                                {
                                    id = imageList.Count + 1,
                                    name = newFilename,
                                    origial_name = entry.FullName,
                                    height = Imgidth,
                                    width = Imgheight
                                });
                            }
                        }


                        /*using (var archive = new ZipArchive(new MemoryStream(ReadToEnd(response.Stream))))//, ZipArchiveMode.Read
                       {
                          foreach (var entry in archive.Entries)
                           {
                               // Skip over any entries that are directories or not image files
                               if (entry.Length == 0 || !IsImageFile(entry.Name)) continue;

                               string nameWithoutExtension = Path.GetFileNameWithoutExtension(entry.Name);
                               string newFilename = OtherClass.HashString(nameWithoutExtension).ToString() + Path.GetExtension(entry.Name);

                               //string imagefilePath = Path.Combine(DirPath, newFilename);
                           */
                        /*if (File.Exists(imagefilePath))
                            File.Delete(imagefilePath);*/

                        /*using (var entryStream = entry.Open())
                        {
                            var imagePath = Path.Combine(DirPath, newFilename);
                            using (var image = Image.FromStream(entryStream))
                            {
                                image.Save(imagePath);
                                imageList.Add(new CacheInfo.Pages
                                {
                                    id = imageList.Count + 1,
                                    name = entry.Name,
                                    origial_name = "",
                                    height = image.Height,
                                    width = image.Width
                                });
                            }
                        }*/

                        // Create a new CacheInfo.Pages object and fill in the basic properties
                        /*var image = new CacheInfo.Pages
                        {
                            id = imageList.Count + 1, // Generate a new ID for each image
                            name = OtherClass.HashString(nameWithoutExtension).ToString() + Path.GetExtension(entry.Name),
                            origial_name = entry.Name
                        };*/

                        //Console.WriteLine(image.id);
                        //Console.WriteLine(image.name);
                        //Console.WriteLine(image.origial_name);

                        // Extract the image to the output folder




                        //entry.ExtractToFile(imagefilePath);

                        //using (var entryStream = entry.Open())
                        //using (var outputStream = new FileStream(imagefilePath, FileMode.CreateNew))
                        //{
                        //entryStream.CopyTo(outputStream);

                        /*using (var image = new Bitmap(entryStream))
                        {
                            imageList.Add(new CacheInfo.Pages
                            {
                                id = imageList.Count + 1, // Generate a new ID for each image
                                name = OtherClass.HashString(nameWithoutExtension).ToString() + Path.GetExtension(entry.Name),
                                origial_name = entry.Name,
                                height = image.Height,
                                width = image.Width
                            });
                            Console.WriteLine("Image Info Add");
                            Console.WriteLine("{0} {1} {2} {3}", imageList.Count, entry.Name, image.Height, image.Width);
                        }**/
                        //}

                        // Open the image using a Bitmap object to get the height and width
                        /*using (var imageFile = new Bitmap(Path.Combine(DirPath, image.name)))
                        {
                            image.width = imageFile.Width;
                            image.height = imageFile.Height;
                        }

                        // Add the CacheInfo.Pages object to the list
                        imageList.Add(image);

                    }
                    }*/
                        File.Create(filePath).Close();
                    }
                    else
                    {
                        Console.WriteLine("Download Is not Successful!");
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(response.Description);
                    }
                }
            }
            return imageList;
        }

        private bool IsImageFile(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
        }
        public static async Task<(int Width, int Height)> GetImageDimensions(string imagePath)
        {
            // Open the image file as a stream
            using FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            // Create a new Image object from the stream
            using var image = await Task.Run(() => Image.Load<Rgba32>(stream));

            // Get the width and height of the image from the Image object
            int width = image.Width;
            int height = image.Height;

            // Return the dimensions as a tuple
            return (width, height);
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
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
        }

        public static void FilePathExists(string filePath)    //Проверка существования директорий
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }




    }
}
