using System.Collections.Generic;
using System.Text.Json;
using WebDav;
//using Newtonsoft.Json;

namespace WebDavManga
{
    public class CacheInfo
    {
        public class TitleRoot
        {
            public long id { get; set; }
            public string en_name { get; set; }
            public string rus_name { get; set; }
            public string dir { get; set; }
            public string another_name { get; set; }
            public string original_name { get; set; }
            public string description { get; set; }
            public int issue_year { get; set; }
            public string avg_rating { get; set; }
            public int count_rating { get; set; }
            public int age_limit { get; set; }
            public Status status { get; set; }
            public int total_votes { get; set; }
            public int total_views { get; set; }
            public Type type { get; set; }
            public List<Genre> genres { get; set; }
            public List<Category> categories { get; set; }
        }

        public class Status
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Type
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Genre
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Category
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public static TitleRoot SaveTitle(string TitleDir)
        {
            TitleRoot t = new TitleRoot();

            t.id = OtherClass.HashString(TitleDir);
            t.en_name = TitleDir; //temp
            t.rus_name = TitleDir; //temp
            t.another_name = TitleDir; //temp
            t.description = "Здесь должно быть описание"; //temp
            t.dir = TitleDir;
            t.issue_year = 2020; //temp
            t.avg_rating = "0"; //temp
            t.age_limit = 1; //temp
            t.count_rating = 0; //temp
            t.status = new Status //temp
            {
                id = 2,
                name = "Продолжается"
            };
            t.type = new Type //temp
            {
                id = 2,
                name = "Манхва"
            };
            t.total_views = 0; //temp
            t.total_votes = 0; //temp
            t.genres = new List<Genre> { //temp
                new Genre
                {
                    id = 11,
                    name = "Драма"
                }
            };
            t.categories = new List<Category> { //temp
                new Category
                {
                    id = 6,
                    name = "В цвете"
                }
            };

            VarDirStorage.Title.Save(t.id.ToString(), t);

            return t;
        }

        public static TitleRoot LoadTitle(long id)
        {
            JsonElement jsonElement = (JsonElement)VarDirStorage.Title.Load(id.ToString()); // Load method returns a JsonElement object

            if (jsonElement is JsonElement)
            {
                return JsonSerializer.Deserialize<TitleRoot>(jsonElement.GetRawText()); // Deserialize JsonElement to a TitleRoot object
            } else
            {
                return new TitleRoot();   
            }
        }

        public class ChapterRoot
        {
            public long id { get; set; }
            public string file_name { get; set; }
            public string File_type { get; set; }
            public long title_id { get; set; }
            public string title_dir { get; set; }
            public int tome { get; set; }
            public string chapter { get; set; }
            public string name { get; set; }
            public string upload_date { get; set; }
            public Translator translator { get; set; }
        }

        public class Translator
        {
            public string name { get; set; }
            public string dir { get; set; }
        }



        public static ChapterRoot SaveChapter(WebDavResource fileInfo, string title_dir)
        {
            ChapterRoot t = new ChapterRoot();

            FileNameInfo f = new FileNameInfo(fileInfo.DisplayName);

            t.id = OtherClass.HashString(fileInfo.DisplayName);
            t.file_name = fileInfo.DisplayName;
            t.File_type = fileInfo.Properties
                    .Where(p => p.Name == "{DAV:}getcontenttype")
                    .Select(p => p.Value)
                    .FirstOrDefault();
            t.title_id = OtherClass.HashString(title_dir);
            t.title_dir = title_dir;
            t.tome = f.Tome;
            t.chapter = f.Chapter;
            t.name = f.Name;
            t.upload_date = OtherClass.UploadDateConvertor(fileInfo);
            t.translator = new Translator
            {
                name = "No Name",
                dir = "No Name"
            };

            VarDirStorage.Chapter.Save(t.id.ToString(), t);

            return t;
        }

        public static ChapterRoot LoadChapter(long id)
        {
            JsonElement jsonElement = (JsonElement)VarDirStorage.Chapter.Load(id.ToString()); // Load method returns a JsonElement object

            if (jsonElement is JsonElement)
            {
                return JsonSerializer.Deserialize<ChapterRoot>(jsonElement.GetRawText()); // Deserialize JsonElement to a ChapterRoot object
            }
            else
            {
                return new ChapterRoot();
            }
        }

        public class ImagesRoot
        {
            public long id { get; set; }
            public long title_id { get; set; }
            public string chapter { get; set; }
            public string upload_date { get; set; }
            public List<Pages> pages { get; set; }
        }

        public class Pages
        {
            public long id { get; set; }
            public string name { get; set; }
            public string origial_name { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }



        public static ImagesRoot SavePages(ChapterRoot c, List<Pages> p)
        {
            ImagesRoot t = new ImagesRoot();

            t.id = c.id;
            t.title_id = c.title_id;
            t.chapter = c.chapter;
            t.upload_date = c.upload_date;
            t.pages = p;

            VarDirStorage.Pages.Save(t.id.ToString(), t);

            return t;
        }

        public static ImagesRoot LoadPages(long id)
        {
            JsonElement jsonElement = (JsonElement)VarDirStorage.Pages.Load(id.ToString()); // Load method returns a JsonElement object

            if (jsonElement is JsonElement)
            {
                return JsonSerializer.Deserialize<ImagesRoot>(jsonElement.GetRawText()); // Deserialize JsonElement to a ChapterRoot object
            }
            else
            {
                return new ImagesRoot();
            }
        }







    }
}
