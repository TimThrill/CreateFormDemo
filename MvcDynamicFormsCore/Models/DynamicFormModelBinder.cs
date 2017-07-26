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

    class DynamicFormModelBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackBinder;

        public DynamicFormModelBinder(IModelBinder fallbackBinder)
        {
            if (fallbackBinder == null)
                throw new ArgumentNullException(nameof(fallbackBinder));

            _fallbackBinder = fallbackBinder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                var valueAsString = valueProviderResult.FirstValue;

                if (string.IsNullOrEmpty(valueAsString))
                {
                    return _fallbackBinder.BindModelAsync(bindingContext);
                }

                var result = HtmlEncoder.Default.Encode(valueAsString);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            return TaskCache.CompletedTask;
        }
    }
}