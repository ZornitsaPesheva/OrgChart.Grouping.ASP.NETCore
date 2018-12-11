using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace OrgChart.Grouping.ASP.NETCore.Controllers
{
    public class HomeController : Controller
    {
        private IMemoryCache cache;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IMemoryCache memoryCache, IHostingEnvironment environment)
        {
            cache = memoryCache;
            hostingEnvironment = environment;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public EmptyResult Update(string id, string node)
        {
            Dictionary<string, string> nodes = null;
            cache.TryGetValue("nodes", out nodes);
            nodes[id] = node;
            return new EmptyResult();
        }

        [HttpPost]
        public EmptyResult Add(string id, string node)
        {
            Dictionary<string, string> nodes = null;
            cache.TryGetValue("nodes", out nodes);
            nodes.Add(id, node);
            return new EmptyResult();
        }

        [HttpPost]
        public EmptyResult Remove(string id)
        {
            Dictionary<string, string> nodes = null;
            cache.TryGetValue("nodes", out nodes);
            nodes.Remove(id);
            return new EmptyResult();
        }

        [HttpPost]
        public EmptyResult UpdateTags(string tags)
        {
            cache.Set("tags", tags);
            return new EmptyResult();
        }        

        public JsonResult Read()
        {
            var nodes = new Dictionary<string, string>();

            if (!cache.TryGetValue("nodes", out nodes))
            {
                nodes = new Dictionary<string, string>();
                nodes.Add("1", JsonConvert.SerializeObject(new { id = "1", tags = new string[] { "Directors" }, name = "Billy Moore", title = "CEO", img = "https://balkangraph.com/js/img/2.jpg" }));
                nodes.Add("2", JsonConvert.SerializeObject(new { id = "2", tags = new string[] { "Directors" }, name = "Marley Wilson", title = "Director", img = "https://balkangraph.com/js/img/3.jpg" }));
                nodes.Add("3", JsonConvert.SerializeObject(new { id = "3", tags = new string[] { "Directors" }, name = "Bennie Shelton", title = "Shareholder", img = "https://balkangraph.com/js/img/4.jpg" }));
                nodes.Add("4", JsonConvert.SerializeObject(new { id = "4", pid = "1", name = "Billie Rose", title = "Shareholder", img = "https://balkangraph.com/js/img/5.jpg" }));
                nodes.Add("5", JsonConvert.SerializeObject(new { id = "5", pid = "1", tags = new string[] { "HRs" }, name = "Glenn Bell", title = "HR", img = "https://balkangraph.com/js/img/10.jpg" }));
                nodes.Add("6", JsonConvert.SerializeObject(new { id = "6", pid = "1", tags = new string[] { "HRs" }, name = "Blair Francis", title = "HR", img = "https://balkangraph.com/js/img/11.jpg" }));
                nodes.Add("7", JsonConvert.SerializeObject(new { id = "7", pid = "1", name = "Skye Terrell", title = "Manager", img = "https://balkangraph.com/js/img/12.jpg" }));
                nodes.Add("8", JsonConvert.SerializeObject(new { id = "8", pid = "4", tags = new string[] { "Devs" }, name = "Jordan Harris", title = "JS Developer", img = "https://balkangraph.com/js/img/6.jpg" }));
                nodes.Add("9", JsonConvert.SerializeObject(new { id = "9", pid = "4", tags = new string[] { "Devs" }, name = "Will Woods", title = "JS Developer", img = "https://balkangraph.com/js/img/7.jpg" }));
                nodes.Add("10", JsonConvert.SerializeObject(new { id = "10", pid = "4", tags = new string[] { "Devs" }, name = "Skylar Parrish", title = "node.js Developer", img = "https://balkangraph.com/js/img/8.jpg" }));
                nodes.Add("11", JsonConvert.SerializeObject(new { id = "11", pid = "4", tags = new string[] { "Devs" }, name = "Ashton Koch", title = "C# Developer", img = "https://balkangraph.com/js/img/9.jpg" }));
                nodes.Add("12", JsonConvert.SerializeObject(new { id = "12", pid = "7", tags = new string[] { "Sales" }, name = "Bret Fraser", title = "Sales", img = "https://balkangraph.com/js/img/13.jpg" }));
                nodes.Add("13", JsonConvert.SerializeObject(new { id = "13", pid = "7", tags = new string[] { "Sales" }, name = "Steff Haley", title = "Sales", img = "https://balkangraph.com/js/img/14.jpg" }));
                cache.Set("nodes", nodes);
            }

            string tags = null;

            if (!cache.TryGetValue("tags", out tags))
            {
                tags = JsonConvert.SerializeObject(new
                {
                    Directors = new
                    {
                        group = true,
                        groupName = "Directors",
                        groupState = 0,
                        template = "group_grey"
                    },
                    HRs = new
                    {
                        group = true,
                        groupName = "HR Team",
                        groupState = 1,
                        template = "group_grey"
                    },
                    Sales = new
                    {
                        group = true,
                        groupName = "Sales Team",
                        groupState = 0,
                        template = "group_grey"
                    },
                    Devs = new
                    {
                        group = true,
                        groupName = "Dev Team",
                        groupState = 0,
                        template = "group_grey"
                    }
                });
                cache.Set("tags", tags);
            }

            return Json(new
            {
                nodes = nodes.Select(p => JsonConvert.DeserializeObject(p.Value)),
                tags = JsonConvert.DeserializeObject(tags)
            }, new JsonSerializerSettings() { MaxDepth = 2 });
        }
    }
}
