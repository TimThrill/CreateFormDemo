using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using MvcDynamicForms.Core;
using MvcDynamicForms.Core.Fields;
using Microsoft.AspNetCore.Http;
using MvcDynamicForms.Demo.Models;
using MvcDynamicForms.Core.Enums;

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
            Form form = new Form();
            form.parseJsonToForm(hostingEnvironment.ContentRootPath + "\\FormTest.json");
            form.Serialize = true;
            return View(form);
        }

        [HttpPost]
        public IActionResult Form(Form form)
        {
            if (form.Validate())
            {
                string jsonRes = form.RenderToJson();
                using (StreamWriter outputFile = new StreamWriter(hostingEnvironment.ContentRootPath + "\\FormTest.json"))
                {
                    outputFile.Write(jsonRes);
                }
                return View(form);
            }
            return View(form);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
