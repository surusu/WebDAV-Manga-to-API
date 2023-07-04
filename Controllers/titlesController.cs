using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static WebDavManga.CacheInfo;

namespace WebDavManga.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class titlesController : Controller
    {

        [HttpGet]
        public async Task<ActionResult> GetTitles(string? ordering, int? count, int page = 1)
        {
            Console.WriteLine("GetCatalog");
            Console.WriteLine("ordering: " + ordering);
            Console.WriteLine("count: " + count);
            Console.WriteLine("page: " + page);

            var result = WebDavController.client.Propfind(WebDavController.dir).Result;
            //Console.WriteLine(result.StatusCode);
            //Console.WriteLine(result.Description);
            if (result.IsSuccessful)
            {
                var content = (from res in result.Resources.Skip(1)
                               let t = CacheInfo.SaveTitle(res.DisplayName)
                               where res.IsCollection == true
                               select new
                               {
                                   id = t.id,
                                   en_name = t.en_name,
                                   rus_name = t.rus_name,
                                   dir = t.dir,
                                   is_licensed = false,
                                   issue_year = t.issue_year,
                                   avg_rating = t.avg_rating,
                                   admin_rating = "0.0",
                                   total_views = t.total_views,
                                   total_votes = t.total_votes,
                                   type = t.type.name,
                                   promo_link = "",
                                   cover_high = @"/media/" + t.dir + "?type=cover",
                                   cover_mid = @"/media/" + t.dir + "?type=cover",
                                   cover_low = @"/media/" + t.dir + "?type=cover",
                                   bookmark_type = (string)null,
                                   genres = t.genres,
                                   count_chapters = 0,
                                   img = new
                                   {
                                       high = @"/media/" + t.dir + "?type=cover",
                                       mid = @"/media/" + t.dir + "?type=cover",
                                       low = @"/media/" + t.dir + "?type=cover",
                                   },
                                   is_promo = false
                               });

                return new JsonResult(new
                {
                    msg = "",
                    content = content,
                    props = new
                    {
                        total_items = result.Resources.Count(),
                        total_pages = 1,
                        page = page,
                    }
                });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{IdOrName}")]
        public async Task<ActionResult> GetTitle(string IdOrName)
        {
            TitleRoot t;

            Console.WriteLine("GetTitle");

            if (long.TryParse(IdOrName, out long result))
            {
                Console.WriteLine("Get ID: " + IdOrName);
                t = CacheInfo.LoadTitle(result);
                Console.WriteLine("Found name: " + t.dir);
            }
            else
            {
                Console.WriteLine("Get Name: " + IdOrName);
                t = CacheInfo.LoadTitle(OtherClass.HashString(Uri.UnescapeDataString(IdOrName)));
                Console.WriteLine("Found id: " + t.id);
            }

            if (t.dir == "")
                return NotFound();

            return new JsonResult(new
            {
                msg = "",
                content = new
                {
                    id = t.id,
                    img = new
                    {
                        high = @"/media/" + t.dir + "?type=cover",
                        mid = @"/media/" + t.dir + "?type=cover",
                        low = @"/media/" + t.dir + "?type=cover",
                    },
                    en_name = t.en_name,
                    rus_name = t.rus_name,
                    another_name = t.another_name,
                    dir = t.dir,
                    description = t.description,
                    issue_year = t.issue_year,
                    avg_rating = t.avg_rating,
                    admin_rating = "0.0",
                    count_rating = t.count_rating,
                    age_limit = t.age_limit,
                    status = t.status,
                    count_bookmarks = 0,
                    total_votes = t.total_votes,
                    total_views = t.total_views,
                    type = t.type,
                    genres = t.genres,
                    categories = t.categories,
                    bookmark_type = (string)null,
                    rated = (string)null,
                    branches = new[]
                    {
                        new {
                                id = t.id,
                                img = "/media/u_low_cover.jpg",
                                subscribed = false,
                                total_votes = 0,
                                count_chapters = 0,
                                publishers = new[] {
                                    new {
                                        id = 26,
                                        name = "Mino Project",
                                        img = "/media/u_8116.jpg",
                                        dir = "alkal",
                                        tagline = "Переводы ваших любимых манхв",
                                        type = "Переводчик"
                                    }
                                },
                        }
                    },
                    active_branch = (string)null,
                    count_chapters = 0,
                    first_chapter = new
                    {
                        id = 678136,
                        tome = 22,
                        chapter = "1"
                    },
                    continue_reading = new
                    {
                        chapter = 14,
                        id = 676804,
                        tome = 1
                    },
                    is_licensed = false,
                    newlate_id = (string)null,
                    newlate_title = (string)null,
                    related = (string)null,
                    uploaded = 1,
                    can_post_comments = false,
                    adaptation = (string)null,
                    publishers = new[] {
                        new {
                            id = 26,
                            name = "Mino Project",
                            img = "/media/u_8116.jpg",
                            dir = "alkal",
                            tagline = "Переводы ваших любимых манхв",
                            type = "Переводчик"
                        }
                     }
                },
                props = new
                {
                    bookmark_types = new[]
                    {
                        new {
                            id = 1,
                            name = "Читаю"
                        },
                        new {
                            id = 2,
                            name = "Буду читать"
                        },
                        new {
                            id = 3,
                            name = "Прочитано"
                        },
                        new {
                            id = 4,
                            name = "Отложено"
                        },
                        new {
                            id =  5,
                            name = "Брошено"
                        },
                        new {
                            id = 6,
                            name = "Не интересно"
                        }
                    },
                    age_limit = new[]
                    {
                        new {
                            id = 1,
                            name = "Для всех"
                        },
                        new {
                            id = 2,
                            name = "16+"
                        },
                        new {
                            id = 3,
                            name = "18+"
                        },
                    },

                    can_upload_chapters = true,
                    can_update = true,
                    can_pin_comment = false,
                    promo_offer = "Продвижение",
                    admin_link = "/Panel/Titles/Details?id=" + t.id,
                    panel_link = "/Panel/?id=" + t.id,
                    edit_link = "/Panel/Titles/Edit?id=" + t.id
                }
            });
        }

        [HttpGet("chapters")]
        public async Task<ActionResult> GetTitleChapters(long branch_id, int? count, string? ordering, int page, int user_data)
        {
            TitleRoot t = CacheInfo.LoadTitle(branch_id);
            //string name = VarDirStorage.IDstorage.Load(id.ToString()).ToString();

            Console.WriteLine("Get Branch Chapters");
            Console.WriteLine("Get ID (Branch id): " + t.id);
            Console.WriteLine("Found name: " + t.dir);

            var result = WebDavController.client.Propfind(WebDavController.Combine(WebDavController.dir, t.dir)).Result;

            var zipItems = result.Resources.Skip(1)
                    .Where(res => res.Properties
                        .Any(p => p.Name == "{DAV:}getcontenttype" && p.Value == "application/zip"))
                    .ToList();

            /*foreach (var res in zipItems)
            {
                Console.WriteLine("--- Name: " + res.DisplayName);
                Console.WriteLine("--- Is directory: " + res.IsCollection);
                Console.WriteLine("--- res.ETag: " + res.ETag);

                var contentType = res.Properties
                    .Where(p => p.Name == "{DAV:}getcontenttype")
                    .Select(p => p.Value)
                    .FirstOrDefault();

                Console.WriteLine("--- Content type: " + contentType);
                Console.WriteLine("---------");
            }*/

            return new JsonResult(new
            {
                msg = "",
                content = from z in zipItems
                          let c = CacheInfo.SaveChapter(z,t.dir)
                          orderby double.Parse(Regex.Match(c.chapter, @"\d+(\.\d+)?").Value) descending
                          select new
                          {
                              id = c.id,
                              rated = (string)null,
                              viewed = (string)null,
                              is_bought = true,
                              publishers = new[]
                              {
                                c.translator
                              },
                              //index = c.ETag,
                              tome = c.tome,
                              chapter = c.chapter,
                              name = c.name == null ? "" : c.name,
                              price = 0,
                              score = 0,
                              upload_date = c.upload_date,
                              pub_date = c.upload_date,
                              is_paid = false
                          },
                props = new
                {
                    page = 1,
                    branch_id = t.id
                }
            });
        }


        [HttpGet("chapters/{id}")]
        public async Task<ActionResult> GetTitleChaptersId(long id)
        {
            Console.WriteLine("Get Chapter");
            Console.WriteLine("Chapter id" + id);

            ChapterRoot c = CacheInfo.LoadChapter(id);
            ImagesRoot i = new ImagesRoot();

            if (c.file_name == "")
                return NotFound();

            string fileUri = WebDavController.Combine(c.title_dir, c.file_name);

            Console.WriteLine("Chapter file names: " + c.file_name);
            Console.WriteLine("Chapter file URI: " + fileUri);

            string chapterimagedir = Path.Combine(OtherClass.HashString(c.title_dir).ToString(), c.id.ToString());

            List<CacheInfo.Pages> result = await CacheFile.ImageDir.GetZipImage(fileUri, chapterimagedir);
            if (result == null)
            {
                Console.WriteLine("Chapter File not Found!");
                return NotFound();
            }

            if ((result.Count == 1) && (result[0].name == "ALREADY"))
            {
                i = CacheInfo.LoadPages(id);
            } else
            {
                i = CacheInfo.SavePages(c,result);
            }

            foreach (var item in i.pages)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}", item.id, item.name, item.origial_name, item.height, item.width);
            }

            //return new FileContentResult(new byte[] {}, "image/jpeg");
            return new JsonResult(new
                {
                    msg = "",
                    content = new
                    {
                        id = c.id,
                        tome = c.tome,
                        chapter = c.chapter,
                        name = c.name == null ? "" : c.name,
                        score = 0,
                        rated = (string)null,
                        view_id = (string)null,
                        upload_date = c.upload_date,
                        is_paid = false,
                        is_bought = true,
                        title_id = c.title_id,
                        volume_id = (string)null,
                        branch_id = c.title_id,
                        price = "0",
                        pub_date = c.upload_date,
                        //index = c.index,

                        publishers = new[]
                                    {
                                        c.translator
                                    },
                        delay_pub_date = (string)null,
                        is_published = true,
                        pages = (from p in i.pages
                                 select new
                                 {
                                     id = p.id,
                                     link = @"/media/" + p.name + "?type=page&param1=" + c.title_id + "&param2=" + c.id,
                                     page = p.id,
                                     height = p.height,
                                     width = p.width,
                                     count_comments = 0
                                 }
                                            ).ToList()
                    },
                    props = new
                    {

                    }
                });



                /*return new JsonResult(new
                {
                    msg = "",
                    content = new
                    {
                        id = id,
                        tome = 1,
                        chapter = "18",
                        name = "",
                        score = 1890,
                        rated = (string)null,
                        view_id = (string)null,
                        upload_date = "2022-10-01T00:00:40.522498",
                        is_paid = false,
                        title_id = 73727,
                        volume_id = (string)null,
                        branch_id = 53102,
                        price = "15.00",
                        pub_date = "2022-10-04T00:00:36.349840",
                        index = 18,
                        publishers = new[]
                        {
                            new
                            {
                                id = 8848,
                                name = "Yumi Group",
                                dir = "yumigroup",
                                show_donate = true,
                                donate_page_text = "Донаты ускоряют выход глав!",
                                img = new
                                {
                                    high = "/../images/low_cover.jpg",
                                    mid = "/../images/low_cover.jpg",
                                    low = "/../images/low_cover.jpg",
                                }
                            }
                        },
                        delay_pub_date = (string)null,
                        is_published = true,
                        pages = new[]
                        {
                            new
                            {
                                id = 28303866,
                                link =  "/../images/1cec57e5674c728c5679cd9bc5732765.jpeg",
                                page = 1,
                                height = 2779,
                                width = 720,
                                count_comments = 1
                            }
                        }
                    },
                    props = new
                    {

                    }
                });*/
                //{ StatusCode = 423 };
            }





    }
}
