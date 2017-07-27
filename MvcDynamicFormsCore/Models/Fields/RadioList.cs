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
            var html = new StringBuilder(this.Template);
            var inputName = this.GetHtmlId();

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.AddCssClass(this._promptClass);
            // prompt.SetInnerText(this.GetPrompt());
            prompt.InnerHtml.Append(this.GetPrompt());
            //html.Replace(PlaceHolders.Prompt, prompt.ToString());
            var tmp = new StringWriter();
            var btmp = tmp.GetStringBuilder();
            prompt.WriteTo(tmp, System.Text.Encodings.Web.HtmlEncoder.Default);
            html.Replace(PlaceHolders.Prompt, tmp.ToString());
            btmp.Remove(0, btmp.Length);

            // error label
            if (!this.ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.AddCssClass(this._errorClass);
                //error.SetInnerText(this.Error);
                error.InnerHtml.Append(this.Error);
                error.WriteTo(tmp, System.Text.Encodings.Web.HtmlEncoder.Default);
                html.Replace(PlaceHolders.Error, tmp.ToString());
                btmp.Remove(0, btmp.Length);
            }

            // list of radio buttons        
            var input = new StringBuilder();
            var ul = new TagBuilder("ul");
            ul.Attributes.Add("class",
                this._orientation == Orientation.Vertical ? this._verticalClass : this._horizontalClass);
            ul.AddCssClass(this._listClass);
            //input.Append(ul.ToString(TagRenderMode.StartTag));
            ul.TagRenderMode = TagRenderMode.StartTag;
            ul.WriteTo(tmp, System.Text.Encodings.Web.HtmlEncoder.Default);
            input.Append(tmp.ToString());
            btmp.Remove(0, btmp.Length);


            var choicesList = this._choices.ToList();
            for (int i = 0; i < choicesList.Count; i++)
            {
                ListItem choice = choicesList[i];
                string radId = inputName + i;

                // open list item
                var li = new TagBuilder("li");
                //input.Append(li.ToString(TagRenderMode.StartTag));
                li.TagRenderMode = TagRenderMode.StartTag;
                li.WriteTo(tmp, System.Text.Encodings.Web.HtmlEncoder.Default);
                input.Append(tmp.ToString());
                btmp.Remove(0, btmp.Length); 

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
                //input.Append(rad.ToString(TagRenderMode.SelfClosing));
                rad.TagRenderMode = TagRenderMode.SelfClosing;
                rad.WriteTo(tmp, System.Text.Encodings.Web.HtmlEncoder.Default);
                input.Append(tmp.ToString());
                btmp.Remove(0, btmp.Length);

                // checkbox label
                var lbl = new TagBuilder("label");
                lbl.Attributes.Add("for", radId);
                lbl.Attributes.Add("class", this._inputLabelClass);
                //lbl.SetInnerText(choice.Text);
                lbl.InnerHtml.Append(choice.Text);
                lbl.WriteTo(tmp, System.Text.Encodings.Web.HtmlEncoder.Default);
                input.Append(tmp.ToString());
                btmp.Remove(0, btmp.Length);

                // close list item
                //input.Append(li.ToString(TagRenderMode.EndTag));
                li.TagRenderMode = TagRenderMode.EndTag;
                li.WriteTo(tmp, System.Text.Encodings.Web.HtmlEncoder.Default);
                input.Append(tmp.ToString());
                btmp.Remove(0, btmp.Length);
            }
            //input.Append(ul.ToString(TagRenderMode.EndTag));
            ul.TagRenderMode = TagRenderMode.EndTag;
            ul.WriteTo(tmp, System.Text.Encodings.Web.HtmlEncoder.Default);
            input.Append(tmp.ToString());
            btmp.Remove(0, btmp.Length);
            html.Replace(PlaceHolders.Input, input.ToString());
            tmp.Close();

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }
    }
}