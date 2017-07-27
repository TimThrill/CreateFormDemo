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
    /// Represents a list of html radio button inputs.
    /// </summary>
    [Serializable]
    public class RadioList : OrientableField
    {
        public override string RenderHtml()
        {
            using (StringWriter writer = new StringWriter()) {

                StringBuilder buildHelper = writer.GetStringBuilder();

                var html = new StringBuilder(this.Template);
                var inputName = this.GetHtmlId();

                // prompt label
                var prompt = new TagBuilder("label");
                prompt.AddCssClass(this._promptClass);
                prompt.InnerHtml.Append(this.GetPrompt());
                prompt.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                html.Replace(PlaceHolders.Prompt, writer.ToString());
                buildHelper.Remove(0, buildHelper.Length);

                // error label
                if (!this.ErrorIsClear)
                {
                    var error = new TagBuilder("label");
                    error.AddCssClass(this._errorClass);
                    //error.SetInnerText(this.Error);
                    error.InnerHtml.Append(this.Error);
                    error.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    html.Replace(PlaceHolders.Error, writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);
                }

                // list of radio buttons        
                var input = new StringBuilder();
                var ul = new TagBuilder("ul");
                ul.Attributes.Add("class",
                    this._orientation == Orientation.Vertical ? this._verticalClass : this._horizontalClass);
                ul.AddCssClass(this._listClass);
                ul.TagRenderMode = TagRenderMode.StartTag;
                ul.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                input.Append(writer.ToString());
                buildHelper.Remove(0, buildHelper.Length);


                var choicesList = this._choices.ToList();
                for (int i = 0; i < choicesList.Count; i++)
                {
                    ListItem choice = choicesList[i];
                    string radId = inputName + i;

                    // open list item
                    var li = new TagBuilder("li");
                    li.TagRenderMode = TagRenderMode.StartTag;
                    li.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    input.Append(writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length); 

                    // radio button input
                    var rad = new TagBuilder("input");
                    rad.Attributes.Add("type", "radio");
                    rad.Attributes.Add("name", inputName);
                    rad.Attributes.Add("id", radId);
                    rad.Attributes.Add("value", choice.Value);
                    if (choice.Selected)
                        rad.Attributes.Add("checked", "checked");
                    rad.MergeAttributes(this._inputHtmlAttributes);
                    rad.MergeAttributes(choice.HtmlAttributes);
                    rad.TagRenderMode = TagRenderMode.SelfClosing;
                    rad.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    input.Append(writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);

                    // checkbox label
                    var lbl = new TagBuilder("label");
                    lbl.Attributes.Add("for", radId);
                    lbl.Attributes.Add("class", this._inputLabelClass);
                    lbl.InnerHtml.Append(choice.Text);
                    lbl.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    input.Append(writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);

                    // close list item
                    li.TagRenderMode = TagRenderMode.EndTag;
                    li.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    input.Append(writer.ToString());
                    buildHelper.Remove(0, buildHelper.Length);
                }
                ul.TagRenderMode = TagRenderMode.EndTag;
                ul.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                input.Append(writer.ToString());
                buildHelper.Remove(0, buildHelper.Length);
                html.Replace(PlaceHolders.Input, input.ToString());

                // wrapper id
                html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

                writer.Close();

                return html.ToString();
            }
        }
    }
}