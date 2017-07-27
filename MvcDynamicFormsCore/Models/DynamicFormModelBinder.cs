﻿using System.Web;

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
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var postedForm = bindingContext.ActionContext.HttpContext.Request.Form;

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