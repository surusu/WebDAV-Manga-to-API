using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace WebDavManga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class searchController : ControllerBase
    {



        [HttpGet("catalog")]
        public async Task<ActionResult> GetCatalog(string? ordering, int? count, int page = 1)
        {

            Console.WriteLine("GetCatalog");

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
                                   type = t.type.name,
                                   total_views = t.total_views,
                                   total_votes = t.total_votes,
                                   cover_high = @"/media/"+ t.dir + "?type=cover",
                                   cover_mid = @"/media/" + t.dir + "?type=cover",
                                   cover_low = @"/media/" + t.dir + "?type=cover",
                                   bookmark_type = (string)null,
                                   genres = t.genres,
                                   img = new
                                   {
                                       high = @"/media/" + t.dir + "?type=cover",
                                       mid = @"/media/" + t.dir + "?type=cover",
                                       low = @"/media/" + t.dir + "?type=cover",
                                   },
                                   categories = t.categories
                               });

                /*foreach (var res in result.Resources)
                {

                    Console.WriteLine("Name: " + res.DisplayName);
                    Console.WriteLine("Is directory: " + res.IsCollection);
                    Console.WriteLine("res.ETag: " + res.ETag);
                    foreach (var item in res.ActiveLocks)
                    {
                        Console.WriteLine("1: " + item);
                    }
                    foreach (var item in res.Properties)
                    {
                        Console.WriteLine("2: " + item);
                    }
                    foreach (var item in res.PropertyStatuses)
                    {
                        Console.WriteLine("1: " + item);
                    }

                }*/

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


    }
}
