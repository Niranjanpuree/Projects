#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\FarContract\_RequiredClauses.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "88b25d0e9e529f9ab307cff933d64a68db1f1268"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_FarContract__RequiredClauses), @"mvc.1.0.view", @"/Views/FarContract/_RequiredClauses.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/FarContract/_RequiredClauses.cshtml", typeof(AspNetCore.Views_FarContract__RequiredClauses))]
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
#line 1 "D:\ESS\ESS-Web\src\Northwind.Web\Views\_ViewImports.cshtml"
using Northwind.Web.Infrastructure;

#line default
#line hidden
#line 2 "D:\ESS\ESS-Web\src\Northwind.Web\Views\_ViewImports.cshtml"
using Northwind.Web.Infrastructure.Models;

#line default
#line hidden
#line 3 "D:\ESS\ESS-Web\src\Northwind.Web\Views\_ViewImports.cshtml"
using Northwind.Web.Infrastructure.Helpers;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88b25d0e9e529f9ab307cff933d64a68db1f1268", @"/Views/FarContract/_RequiredClauses.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_FarContract__RequiredClauses : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.FarClause.FarContractViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("control-label mb-2 border-bottom py-2 d-block"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(71, 69, true);
            WriteLiteral("    <div class=\"col-12\">\r\n        <div class=\"row border-top pt-2\">\r\n");
            EndContext();
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Views\FarContract\_RequiredClauses.cshtml"
             for (int i = 0; i < Model.RequiredFarClauses.Count; i++)
            {

#line default
#line hidden
            BeginContext(226, 56, true);
            WriteLiteral("            <div class=\"col-md-6\">\r\n                    ");
            EndContext();
            BeginContext(282, 304, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "88b25d0e9e529f9ab307cff933d64a68db1f12684288", async() => {
                BeginContext(390, 26, true);
                WriteLiteral("\r\n                        ");
                EndContext();
                BeginContext(417, 43, false);
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Views\FarContract\_RequiredClauses.cshtml"
                   Write(Model.RequiredFarClauses[i].FarClauseNumber);

#line default
#line hidden
                EndContext();
                BeginContext(460, 2, true);
                WriteLiteral(" (");
                EndContext();
                BeginContext(463, 46, false);
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Views\FarContract\_RequiredClauses.cshtml"
                                                                 Write(Model.RequiredFarClauses[i].FarClauseParagraph);

#line default
#line hidden
                EndContext();
                BeginContext(509, 4, true);
                WriteLiteral(") - ");
                EndContext();
                BeginContext(514, 42, false);
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Views\FarContract\_RequiredClauses.cshtml"
                                                                                                                    Write(Model.RequiredFarClauses[i].FarClauseTitle);

#line default
#line hidden
                EndContext();
                BeginContext(556, 22, true);
                WriteLiteral("\r\n                    ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\FarContract\_RequiredClauses.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.RequiredFarClauses[i].FarClauseTitle);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(586, 22, true);
            WriteLiteral("\r\n            </div>\r\n");
            EndContext();
#line 11 "D:\ESS\ESS-Web\src\Northwind.Web\Views\FarContract\_RequiredClauses.cshtml"
            }

#line default
#line hidden
            BeginContext(623, 28, true);
            WriteLiteral("        </div>\r\n    </div>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.FarClause.FarContractViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
