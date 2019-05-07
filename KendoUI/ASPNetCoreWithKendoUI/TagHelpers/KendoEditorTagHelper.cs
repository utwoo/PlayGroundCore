using System;
using System.Collections.Generic;
using ASPNetCoreWithKendoUI.Core;
using ASPNetCoreWithKendoUI.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASPNetCoreWithKendoUI.TagHelpers
{
    [HtmlTargetElement("kendo-editor", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
    public class KendoEditorTagHelper : TagHelper
    {
        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Editor template for the field
        /// </summary>
        [HtmlAttributeName("asp-template")]
        public string Template { set; get; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _htmlHelper;

        public KendoEditorTagHelper(
            IHtmlHelper htmlHelper)
        {
            this._htmlHelper = htmlHelper;
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

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //input attributes
            var htmlAttributes = new Dictionary<string, object>();
            if (!output.Attributes.ContainsName("class"))
                htmlAttributes.Add("class", "form-control");
            else
                htmlAttributes.Add("class", $"{output.Attributes["class"]} form-control");

            //generate container
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "col-sm-6");

            //generate editor

            //we have to invoke strong typed "EditorFor" method of HtmlHelper<TModel>
            //but we cannot do it because we don't have access to Expression<Func<TModel, TValue>>
            //more info at https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.ViewFeatures/ViewFeatures/HtmlHelperOfT.cs

            //so we manually invoke implementation of "GenerateEditor" method of HtmlHelper
            //more info at https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.ViewFeatures/ViewFeatures/HtmlHelper.cs

            //little workaround here. we need to access private properties of HtmlHelper
            //just ensure that they are not renamed by asp.net core team in future versions
            var viewEngine = CommonHelper.GetPrivateFieldValue(_htmlHelper, "_viewEngine") as IViewEngine;
            var bufferScope = CommonHelper.GetPrivateFieldValue(_htmlHelper, "_bufferScope") as IViewBufferScope;
            var templateBuilder = new TemplateBuilder(
                viewEngine,
                bufferScope,
                _htmlHelper.ViewContext,
                _htmlHelper.ViewData,
                For.ModelExplorer,
                For.Name,
                Template,
                readOnly: false,
                additionalViewData: new { htmlAttributes });

            var htmlOutput = templateBuilder.Build();
            output.Content.SetHtmlContent(htmlOutput.RenderHtmlContent());
        }
    }
}
