namespace MvcDynamicForms.Core.Fields
{
    using System;
    using System.Text;
    using MvcDynamicForms.Core.Fields.Abstract;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Represents an html textbox input element.
    /// </summary>
    [Serializable]
    public class TextBox : TextField
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder(this.Template);
            var inputName = this.GetHtmlId();

            // prompt label
            var prompt = new TagBuilder("label");
            //prompt.SetInnerText(this.GetPrompt());
            prompt.InnerHtml.Append(this.GetPrompt());
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", this._promptClass);
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // error label
            if (!this.ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.Attributes.Add("for", inputName);
                error.Attributes.Add("class", this._errorClass);
                //error.SetInnerText(this.Error);
                error.InnerHtml.Append(this.Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // input element
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");
            txt.Attributes.Add("value", this.Value);
            txt.MergeAttributes(this._inputHtmlAttributes);
            //html.Replace(PlaceHolders.Input, txt.ToString(TagRenderMode.SelfClosing));
            html.Replace(PlaceHolders.Input, txt.ToString());

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }
    }
}