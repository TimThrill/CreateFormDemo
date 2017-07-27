namespace MvcDynamicForms.Core.Fields
{
    using System;
    using System.Text;
    using MvcDynamicForms.Core.Fields.Abstract;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.IO;

    /// <summary>
    /// Represents an html textarea element.
    /// </summary>
    [Serializable]
    public class TextArea : TextField
    {
        public override string RenderHtml()
        {
            using (StringWriter writer = new StringWriter())
            {
                // healBuilder is used to clean StringWriter writer every time.
                var helpBuilder = writer.GetStringBuilder();
                System.Text.Encodings.Web.HtmlEncoder htmlEncoding = System.Text.Encodings.Web.HtmlEncoder.Default;

                var html = new StringBuilder(this.Template);
                var inputName = this.GetHtmlId();

                // prompt label
                var prompt = new TagBuilder("label");
                prompt.InnerHtml.Append(this.GetPrompt());
                prompt.Attributes.Add("for", inputName);
                prompt.Attributes.Add("class", this._promptClass);
                prompt.WriteTo(writer, htmlEncoding);
                html.Replace(PlaceHolders.Prompt, writer.ToString());
                helpBuilder.Remove(0, helpBuilder.Length);

                // error label
                if (!this.ErrorIsClear)
                {
                    var error = new TagBuilder("label");
                    error.Attributes.Add("for", inputName);
                    error.Attributes.Add("class", this._errorClass);
                    error.InnerHtml.Append(this.Error);
                    error.WriteTo(writer, htmlEncoding);
                    html.Replace(PlaceHolders.Error, writer.ToString());
                    helpBuilder.Remove(0, helpBuilder.Length);
                }

                // input element
                var txt = new TagBuilder("textarea");
                txt.Attributes.Add("name", inputName);
                txt.Attributes.Add("id", inputName);
                txt.InnerHtml.Append(this.Value);
                txt.MergeAttributes(this._inputHtmlAttributes);
                txt.WriteTo(writer, htmlEncoding);
                html.Replace(PlaceHolders.Input, writer.ToString());
                helpBuilder.Remove(0, helpBuilder.Length);

                // wrapper id
                html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

                return html.ToString();
            }
        }
    }
}