﻿namespace MvcDynamicForms.Demo.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using MvcDynamicForms.Core;
    using MvcDynamicForms.Core.Fields;
    using MvcDynamicForms.Demo.Models;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        /*
         * First off, these Demos do the exact same thing from the end user's perspective.
         * The difference is in how the Form object is persisted across requests.
         * 
         * Most often, you'll need to keep the original Form and Field objects around for as long 
         * as your user is working on completing the form. This is because, when InputField 
         * objects are constructed, they are keyed with a new Guid. See Demos 1 & 2 for examples.
         * 
         * You can key your InputFields manually by setting the InputField.Key property.
         * If you do this and can guarantee that the Fields and their Keys will not change after
         * a complete reconstruction of all objects, then you don't have to persist the objects across
         * requests. See Demo 3.
         * 
         * In Demo 1, the Form object graph is serialized to a string and stored in a hidden field
         * in the page's HTML.
         * 
         * In Demo 2, the Form object graph is simply stored in TempData (short lived session state).
         * 
         * In Demo 3, the Form object graph is not persisted across requests. It is reconstructed
         * on each request and the InputField's keys are set manually.
         * 
         * The serialization approach (demo 1) results in more concise code in the controller. 
         * Serializing the Form is also more reliable, in my opinion.
         * 
         * However, response time increases because of serialized data 
         * and the (de)serialization process takes time, as well.
         * 
         * The approach you take depends on your needs.
         */

        public ActionResult Demo1()
        {
            var form = FormProvider.GetForm();

            // we are going to store the form and 
            // the field objects on the page across requests
            form.Serialize = true;

            return this.View("Demo", form);
        }

        [HttpPost]
        public ActionResult Demo1(Form form)
        {
            // no need to retrieve the form object from anywhere
            // just use a parameter on the Action method that we are posting to

            if (form.Validate()) //input is valid
                return this.View("Responses", form);

            // input is not valid
            return this.View("Demo", form);
        }

        public ActionResult Demo2()
        {
            var form = FormProvider.GetForm();

            // we are going to store the form 
            // in server memory across requests
            this.TempData["form"] = form;

            return this.View("Demo", form);
        }

        [HttpPost]
        [ActionName("Demo2")]
        public ActionResult Demo2Post()
        {
            // we have to get the form object from
            // server memory and manually perform model binding
            var form = (Form)this.TempData["form"];
            this.UpdateModel(form);

            if (form.Validate()) // input is valid
                return this.View("Responses", form);

            // input is not valid
            this.TempData["form"] = form;
            return this.View("Demo", form);
        }

        public ActionResult Demo3()
        {
            // recreate the form and set the keys
            var form = FormProvider.GetForm();
            this.Demo3SetKeys(form);

            // set user input on recreated form
            this.UpdateModel(form);

            if (this.Request.HttpMethod == "POST" && form.Validate()) // input is valid
                return this.View("Responses", form);

            // input is not valid
            return this.View("Demo", form);
        }

        void Demo3SetKeys(Form form)
        {
            int key = 1;
            foreach (var field in form.InputFields)
            {
                field.Key = key++.ToString();
            }
        }

        public ActionResult Demo4()
        {
            var attr = new Dictionary<string, string>();
            attr.Add("class", "form-control");
            attr.Add("placeholder", "Please Enter Name");

            var name = new TextBox
            {
                ResponseTitle = "Name",
                Prompt = "Enter your full name:",
                DisplayOrder = 20,
                Required = true,
                RequiredMessage = "Your full name is required",
                InputHtmlAttributes = attr
            };

            var form = new Form();
            form.AddFields(name);

            return this.View("Demo", form);
        }
    }
}
