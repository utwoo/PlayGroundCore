using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASPNetCoreWithKendoUI.TagHelpers
{
    [HtmlTargetElement("kendo-editor", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
    public class KendoEditorTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; set; }

        public KendoEditorTagHelper(IHtmlGenerator generator)
        {
            this.Generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var classValue = output.Attributes.ContainsName("class")
                                ? $"{output.Attributes["class"].Value} form-control"
                                : "form-control";

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "col-sm-6");

            var tagBuilder = Generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, null, string.Empty, null);
            tagBuilder.Attributes.Remove("class");
            tagBuilder.Attributes.Add("class", classValue);
            output.Content.SetHtmlContent(tagBuilder);
        }
    }
}
