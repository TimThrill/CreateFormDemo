namespace MvcDynamicForms.Core.Fields
{
    using System;
    using System.Linq;
    using System.Text;
    using MvcDynamicForms.Core.Enums;
    using MvcDynamicForms.Core.Fields.Abstract;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.IO;

    /// <summary>
    /// Represents a list of html checkbox inputs.
    /// </summary>
    [Serializable]
    public class CheckBoxList : OrientableField
    {
        public override string RenderHtml()
        {
            using (StringWriter writer = new StringWriter())
            {
                // buildHelper is used to clean writer
                StringBuilder buildHelper = writer.GetStringBuilder();
                System.Text.Encodings.Web.HtmlEncoder htmlEncoding = System.Text.Encodings.Web.HtmlEncoder.Default;

                var html = new StringBuilder(this.Template);
                var inputName = this.GetHtmlId();

                // prompt label
                var prompt = new TagBuilder("label");
                prompt.AddCssClass(this._promptClass);
                prompt.InnerHtml.Append(this.GetPrompt());
                prompt.WriteTo(writer, htmlEncoding);
                html.Replace(PlaceHolders.Prompt, writer.ToString());
                buildHelper.Remove(0, buildHelper.Length);

                // error label
                if (!this.ErrorIsClear)
                {
                    var error = new TagBuilder("label");
                    error.AddCssClass(this._errorClass);
                    ;
                    error.InnerHtml.Append(this.Error);
                    error.WriteTo(writer, htmlEncoding);
                    html.Replace(PlaceHolders.Error, writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);
                }

                // list of checkboxes
                var input = new StringBuilder();
                var ul = new TagBuilder("ul");
                ul.AddCssClass(this._orientation == Orientation.Vertical ? this._verticalClass : this._horizontalClass);
                ul.AddCssClass(this._listClass);
                ul.TagRenderMode = TagRenderMode.StartTag;
                ul.WriteTo(writer, htmlEncoding);
                input.Append(writer.ToString());
                buildHelper.Remove(0, buildHelper.Length);

                var choicesList = this._choices.ToList();
                for (int i = 0; i < choicesList.Count; i++)
                {
                    ListItem choice = choicesList[i];
                    string chkId = inputName + i;

                    // open list item
                    var li = new TagBuilder("li");
                    li.TagRenderMode = TagRenderMode.StartTag;
                    li.WriteTo(writer, htmlEncoding);
                    input.Append(writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);

                    // checkbox input
                    var chk = new TagBuilder("input");
                    chk.Attributes.Add("type", "checkbox");
                    chk.Attributes.Add("name", inputName);
                    chk.Attributes.Add("id", chkId);
                    chk.Attributes.Add("value", choice.Value);
                    if (choice.Selected)
                        chk.Attributes.Add("checked", "checked");
                    chk.MergeAttributes(this._inputHtmlAttributes);
                    chk.MergeAttributes(choice.HtmlAttributes);
                    chk.TagRenderMode = TagRenderMode.SelfClosing;
                    chk.WriteTo(writer, htmlEncoding);
                    input.Append(writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);

                    // checkbox label
                    var lbl = new TagBuilder("label");
                    lbl.Attributes.Add("for", chkId);
                    lbl.AddCssClass(this._inputLabelClass);
                    lbl.InnerHtml.Append(choice.Text);
                    lbl.WriteTo(writer, htmlEncoding);
                    input.Append(writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);

                    // close list item
                    li.TagRenderMode = TagRenderMode.EndTag;
                    li.WriteTo(writer, htmlEncoding);
                    input.Append(writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);
                }
                ul.TagRenderMode = TagRenderMode.EndTag;
                ul.WriteTo(writer, htmlEncoding);
                input.Append(writer.ToString());
                buildHelper.Remove(0, buildHelper.Length);

                // add hidden tag, so that a value always gets sent
                var hidden = new TagBuilder("input");
                hidden.Attributes.Add("type", "hidden");
                hidden.Attributes.Add("id", inputName + "_hidden");
                hidden.Attributes.Add("name", inputName);
                hidden.Attributes.Add("value", string.Empty);
                hidden.TagRenderMode = TagRenderMode.SelfClosing;
                hidden.WriteTo(writer, htmlEncoding);
                html.Replace(PlaceHolders.Input, input.ToString() + writer.ToString());

                // wrapper id
                html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

                return html.ToString();
            }
        }
    }
}