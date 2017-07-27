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
            using (StreamReader file = System.IO.File.OpenText(hostingEnvironment.ContentRootPath + "\\FormModule.json"))
            {
                JObject moduleForm = JObject.Parse(file.ReadToEnd());
                string formName = moduleForm["Module"]["Name"].ToString();

                JToken sections = moduleForm["Module"]["Sections"];
                foreach(var section in sections)
                {
                    foreach (var question in section["Questions"])
                    {
                        string questionType = question["Type"].ToString();
                        if (questionType.Equals("RadioList"))
                        {
                            string title = question["Title"].ToString();
                            string values = "";
                            foreach (var value in question["Values"])
                            {
                                values += value.ToString() + ",";
                            }
                            values.Remove(values.Length - 1);

                            var radioOption = new RadioList
                            {
                                ResponseTitle = title,
                                Required = question["Required"] == null ? true : (bool)question["Required"],
                                Prompt = title,
                                Orientation = MvcDynamicForms.Core.Enums.Orientation.Horizontal
                            };
                            radioOption.AddChoices(values);
                            form.Fields.Add(radioOption);
                        }
                        else if (questionType.Equals("TextArea"))
                        {
                            string title = question["Title"].ToString();
                            var textArea = new TextArea
                            {
                                ResponseTitle = title,
                                Prompt = title,
                                Required = true
                            };
                            form.Fields.Add(textArea);
                        }
                    }
                }
            }
            
            //var form = FormProvider.GetForm();
            form.Serialize = true;
            return View(form);
        }

        [HttpPost]
        public IActionResult Form(Form form)
        {
            if (form.Validate())
            {
                return View("Responses", form);
            }
            return View(form);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
