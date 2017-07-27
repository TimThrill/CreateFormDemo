using System.Web;

namespace MvcDynamicForms.Core
{
    using System;
    using System.Linq;
    using MvcDynamicForms.Core.Fields;
    using MvcDynamicForms.Core.Fields.Abstract;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Internal;
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

    public class DynamicFormModelBinder : IModelBinder
    {
        /// <summary>
        /// There are three main steps in binding a form.
        /// 1. Read submitted form from context.
        /// 2. Get a model object.
        /// 3. Write result to bindingContext.
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            // 1. Get submitted form from context
            var postedForm = bindingContext.ActionContext.HttpContext.Request.Form;
            // 2. Get a Form object
            var form = (Form)bindingContext.Model;

            if (form == null && !string.IsNullOrEmpty(postedForm[MagicStrings.MvcDynamicSerializedForm]))
            {
                form = SerializationUtility.Deserialize<Form>(postedForm[MagicStrings.MvcDynamicSerializedForm]);
            }

            if (form == null)
                throw new NullReferenceException(
                    "The dynamic form object was not found.Be sure to include PlaceHolders.SerializedForm in your form template.");

            foreach (var key in postedForm.Keys.Where(k => k.StartsWith(form.FieldPrefix)))
            {
                string fieldKey = key.Remove(0, form.FieldPrefix.Length);
                InputField dynField = form.InputFields.SingleOrDefault(f => f.Key == fieldKey);

                if (dynField == null)
                    continue;

                if (dynField is ListField)
                {
                    var lstField = (ListField)dynField;

                    // clear all choice selections
                    foreach (var choice in lstField.Choices)
                        choice.Selected = false;

                    // set current selections
                    foreach (var value in postedForm[key])
                    {
                        var choice = lstField.Choices.FirstOrDefault(x => x.Value == value);
                        if (choice != null)
                            choice.Selected = true;
                    }
                }
                else if (dynField is TextField)
                {
                    var txtField = (TextField)dynField;
                    txtField.Value = postedForm[key];
                }
            }

            // 3. Write results to model
            bindingContext.Result = ModelBindingResult.Success(form);
            return TaskCache.CompletedTask;
        }
    }

    /*
    public class DynamicFormModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(Form))
            {
                return new BinderTypeModelBinder(typeof(DynamicFormModelBinder));
            }

            return null;
        }
    }
    */
}