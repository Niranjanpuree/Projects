#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6e079c1531251f319d99c05d8e35960ca0bc6cab"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_JobRequest__EmployeeBillingRates), @"mvc.1.0.view", @"/Views/JobRequest/_EmployeeBillingRates.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/JobRequest/_EmployeeBillingRates.cshtml", typeof(AspNetCore.Views_JobRequest__EmployeeBillingRates))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6e079c1531251f319d99c05d8e35960ca0bc6cab", @"/Views/JobRequest/_EmployeeBillingRates.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_JobRequest__EmployeeBillingRates : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.JobRequestViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(60, 13, true);
            WriteLiteral("\r\n<div>\r\n    ");
            EndContext();
            BeginContext(73, 84, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "6e079c1531251f319d99c05d8e35960ca0bc6cab3847", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.EmployeeBillingRates.BillingRateGuid);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(157, 6, true);
            WriteLiteral("\r\n    ");
            EndContext();
            BeginContext(163, 60, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "6e079c1531251f319d99c05d8e35960ca0bc6cab5800", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
#line 5 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.ContractGuid);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(223, 337, true);
            WriteLiteral(@"
</div>

<div class="""">
    <div class=""col-12"">
        <div class=""row pt-3 align-items-center"">
            <div class=""col"">
                <p class=""mb-0"">
                    Please review the Employee Billing Rates below.  If changes are required, click the Edit Here button.
                </p>
            </div>

");
            EndContext();
#line 17 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
             if (Model.EmployeeBillingRates.IsCsv)
            {

#line default
#line hidden
            BeginContext(627, 241, true);
            WriteLiteral("                <div class=\"col col-sm-auto\">\r\n                    <input id=\"idEditEmployeeBillingRates\" class=\"btn btn-sm btn-secondary\" type=\"button\"\r\n                           value=\"Not Complete? Edit here\" />\r\n                </div>\r\n");
            EndContext();
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(916, 230, true);
            WriteLiteral("                <div class=\"col col-sm-auto\">\r\n                    <input id=\"idViewEBRNonCSV\" class=\"btn btn-sm btn-secondary\" type=\"button\"\r\n                           value=\"Not Complete? Edit here\" />\r\n                </div>\r\n");
            EndContext();
#line 30 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
            }

#line default
#line hidden
            BeginContext(1161, 32, true);
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n\r\n");
            EndContext();
#line 35 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
     if (Model.EmployeeBillingRates.IsCsv)
    {

#line default
#line hidden
            BeginContext(1244, 96, true);
            WriteLiteral("        <div class=\"p-3\">\r\n            <div id=\"JobEmployeeBillingGrid\"></div>\r\n        </div>\r\n");
            EndContext();
#line 40 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
    }
    else
    {
        

#line default
#line hidden
#line 43 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
         if (Model.EmployeeBillingRates != null)
        {
            var fileName = !string.IsNullOrWhiteSpace(@Model.EmployeeBillingRates.UploadFileName) ? Model.EmployeeBillingRates.UploadFileName.Split("/").Last() : "";

#line default
#line hidden
            BeginContext(1592, 69, true);
            WriteLiteral("            <div class=\"col-md-12 uploaded-file\">\r\n                <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 1661, "\"", 1759, 2);
            WriteAttributeValue("", 1668, "/ContractResourceFile/DownloadDocument/", 1668, 39, true);
#line 47 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
WriteAttributeValue("", 1707, Model.EmployeeBillingRates.ContractResourceFileGuid, 1707, 52, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("id", "\r\n                   id=\"", 1760, "\"", 1794, 1);
#line 48 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
WriteAttributeValue("", 1785, fileName, 1785, 9, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1795, 108, true);
            WriteLiteral(">\r\n                    <i class=\"k-icon k-i-file-txt\"></i>\r\n                    <span class=\"control-label\">");
            EndContext();
            BeginContext(1904, 8, false);
#line 50 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
                                           Write(fileName);

#line default
#line hidden
            EndContext();
            BeginContext(1912, 51, true);
            WriteLiteral("</span>\r\n                </a>\r\n            </div>\r\n");
            EndContext();
#line 53 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
        }

#line default
#line hidden
#line 53 "D:\ESS\ESS-Web\src\Northwind.Web\Views\JobRequest\_EmployeeBillingRates.cshtml"
         
    }

#line default
#line hidden
            BeginContext(1981, 8, true);
            WriteLiteral("\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.JobRequestViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
