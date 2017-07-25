using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;

namespace CreateFormDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Form()
        {
            using (StreamReader file = System.IO.File.OpenText(hostingEnvironment.ContentRootPath + "\\FormModule.json"))
            {
                JObject moduleForm = JObject.Parse(file.ReadToEnd());
                string formName = moduleForm["Module"]["Name"].ToString();
            }
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
