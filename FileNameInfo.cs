using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WebDavManga
{
    public class FileNameInfo
    {
        public int Tome { get; set; }
        public string Name { get; set; }
        public string Chapter { get; set; }

        /*public FileNameInfo(string fileName)
        {
            var parts = Path.GetFileNameWithoutExtension(fileName).Split('_');

            if (parts.Length == 3 && int.TryParse(parts[0], out int tome) && int.TryParse(parts[1], out int chapter))
            {
                Tome = tome;
                Chapter = chapter;
                Name = parts[2];
            }
            else
            {
                Console.WriteLine("Error: Invalid file name format.");
            }
        }*/

        public FileNameInfo(string fileName)
        {
            //Console.WriteLine("Regex: " + fileName);

            Regex regex = new Regex(@"^(?<tome>\[(?<tomeValue>\d+)\]\s+)?(?<chapter>\[Ch. (?<chapterValue>[\d\.]+)\]\s+)?(?<title>.+?)( \[(?<name>.+)\])?(\.cbz)?$");

            Match match = regex.Match(fileName);
            if (match.Success)
            {
                // Extract tome value
                string tomeStr = match.Groups["tomeValue"].Value;
                if (!string.IsNullOrEmpty(tomeStr))
                {
                    Tome = int.Parse(tomeStr);
                } else
                {
                    Tome = 1;
                }
                //Console.WriteLine(": "+ Tome);
                // Extract chapter value
                Chapter = match.Groups["chapterValue"].Value;
                //Console.WriteLine(": " + Chapter);

                // Extract name value
                Name = match.Groups["name"].Value;
                //Console.WriteLine(": " + Name);
                //Console.WriteLine("-----");
            }
            else
            {
                Console.WriteLine("Error: Invalid file name format.");
            }

            /*var regex = new Regex(@"^(?<title>[^\d\s]+(?:\s+[^\d\s]+)*)\s*(?:(?<tome>\d+)\s+)?(?<chapter>(?:\d+(?:\.\d+)?)|(?:\.\d+))\s+(?<name>.+?)(?:\.cbz)?$");

            //regex = new Regex(@"^(.*)\s+(\d+(?:\.\d+)?)\s+(.+?)(?:\.cbz)?$");


            var match = regex.Match(fileName);
            if (match.Success)
            {
                int.TryParse(match.Groups["tome"].Value.Trim() ?? "0", out int tome);
                Tome = tome;
                Chapter = match.Groups["chapter"].Value.Trim();
                ChapterName = match.Groups["name"].Value.Trim();
            }
            else
            {
                Tome = 0;
                Chapter = "";
                ChapterName = "";
            }*/

            //var regex = new Regex(@"^(?<TitleName>.+?)\s+(?<Tome>\d+)\.(?<Chapter>\d+)\s+(?<Name>.+?)\.cbz$");

            /*var regex = new Regex(@"^(?<name>.+?)\s(?<tome>\d+)(\.(?<chapter>\d+))?\s(?<names>.+?)\.cbz$");
            var match = regex.Match(fileName);

            if (!match.Success)
            {
                Console.WriteLine("Error: Invalid file name format.");
                Tome = "";
                Chapter = 0;
                Name= "";
            } else
            {
                Tome = match.Groups["tome"].Value;
                Chapter = match.Groups["chapter"].Success ? int.Parse(match.Groups["chapter"].Value) : 1; ;
                Name = match.Groups["names"].Value;
                Index = Convert.ToInt32(Tome) * Chapter;
            }*/

        }

    }
}
