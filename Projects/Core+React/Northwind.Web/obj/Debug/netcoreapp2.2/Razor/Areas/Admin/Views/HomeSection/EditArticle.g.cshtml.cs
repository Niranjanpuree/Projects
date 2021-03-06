#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1a46b5eec4951d5b0bd481f6df70ed2eebb84c99"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_HomeSection_EditArticle), @"mvc.1.0.view", @"/Areas/Admin/Views/HomeSection/EditArticle.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/HomeSection/EditArticle.cshtml", typeof(AspNetCore.Areas_Admin_Views_HomeSection_EditArticle))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml"
using System.Web;

#line default
#line hidden
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1a46b5eec4951d5b0bd481f6df70ed2eebb84c99", @"/Areas/Admin/Views/HomeSection/EditArticle.cshtml")]
    public class Areas_Admin_Views_HomeSection_EditArticle : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Article.ArticleViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_FormModelArticle.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml"
  
    ViewData["Title"] = "Article : Edit";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(317, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
#line 12 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            DefineSection("breadcrumb", async() => {
                BeginContext(460, 156, true);
                WriteLiteral("\r\n    <li class=\"breadcrumb-item\"><a href=\"/Admin/Homesection\">List of Articles</a></li>\r\n    <li class=\"breadcrumb-item\"><a href=\"#\">Add Article</a></li>\r\n");
                EndContext();
            }
            );
            BeginContext(619, 13, true);
            WriteLiteral("\r\n<div>\r\n    ");
            EndContext();
            BeginContext(632, 43, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "1a46b5eec4951d5b0bd481f6df70ed2eebb84c994485", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(675, 12, true);
            WriteLiteral("\r\n</div>\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(705, 145, true);
                WriteLiteral("\r\n    <script>\r\n\r\n\r\n        var editor = null;\r\n        (function () {\r\n            editor = window.richEditor.loadRichEditor(\'richtexteditor\', \'");
                EndContext();
                BeginContext(851, 20, false);
#line 32 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml"
                                                                    Write(Html.Raw(Model.Body));

#line default
#line hidden
                EndContext();
                BeginContext(871, 51, true);
                WriteLiteral("\');\r\n\r\n        })()\r\n\r\n\r\n    </script>\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 922, "\"", 1000, 2);
#line 38 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml"
WriteAttributeValue("", 928, cdnUrl, 928, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 935, "/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js", 935, 65, true);
                EndWriteAttribute();
                BeginContext(1001, 23, true);
                WriteLiteral("></script>\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 1024, "\"", 1075, 3);
#line 39 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml"
WriteAttributeValue("", 1030, cdnUrl, 1030, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 1037, "/js/proj/article.js?v=", 1037, 22, true);
#line 39 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\HomeSection\EditArticle.cshtml"
WriteAttributeValue("", 1059, resourceVersion, 1059, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1076, 12, true);
                WriteLiteral("></script>\r\n");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IConfiguration Configuration { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Article.ArticleViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
