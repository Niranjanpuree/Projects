#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fa62ab35d00d10ca8470092e933062ab0ec64b67"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Project_FormsInDetail__EmployeeBillingRatesView), @"mvc.1.0.view", @"/Views/Project/FormsInDetail/_EmployeeBillingRatesView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Project/FormsInDetail/_EmployeeBillingRatesView.cshtml", typeof(AspNetCore.Views_Project_FormsInDetail__EmployeeBillingRatesView))]
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
#line 2 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fa62ab35d00d10ca8470092e933062ab0ec64b67", @"/Views/Project/FormsInDetail/_EmployeeBillingRatesView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_Project_FormsInDetail__EmployeeBillingRatesView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.EmployeeBillingRatesViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "_EmployeeBillingRatesView", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(264, 56, true);
            WriteLiteral("<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        ");
            EndContext();
            BeginContext(320, 1308, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fa62ab35d00d10ca8470092e933062ab0ec64b675545", async() => {
                BeginContext(365, 14, true);
                WriteLiteral("\r\n            ");
                EndContext();
                BeginContext(379, 66, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fa62ab35d00d10ca8470092e933062ab0ec64b675939", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#line 11 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary = global::Microsoft.AspNetCore.Mvc.Rendering.ValidationSummary.ModelOnly;

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-summary", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(445, 37, true);
                WriteLiteral("\r\n            <div>\r\n                ");
                EndContext();
                BeginContext(482, 63, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "fa62ab35d00d10ca8470092e933062ab0ec64b677774", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
#line 13 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.BillingRateGuid);

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(545, 18, true);
                WriteLiteral("\r\n                ");
                EndContext();
                BeginContext(563, 60, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "fa62ab35d00d10ca8470092e933062ab0ec64b679823", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
#line 14 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.ContractGuid);

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(623, 24, true);
                WriteLiteral("\r\n            </div>\r\n\r\n");
                EndContext();
#line 17 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
             if (Model.IsCsv)
            {

#line default
#line hidden
                BeginContext(693, 275, true);
                WriteLiteral(@"                <div class=""row form-group no-gutters"">
                    <div class=""col"">
                        <input type=""button"" id=""DownloadEBRGrid"" class=""download btn btn-secondary"" value=""Export to CSV"" />
                    </div>
                </div>
");
                EndContext();
                BeginContext(970, 73, true);
                WriteLiteral("                <div class=\"form-group\" id=\"EmployeeBillingGrid\"></div>\r\n");
                EndContext();
#line 26 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
            }
            else
            {
                var fileName = @Model.UploadFileName.Split("/").Last();
                

#line default
#line hidden
                BeginContext(1180, 57, true);
                WriteLiteral("<div class=\"col-md-6 form-group\">\r\n                    <a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 1237, "\"", 1320, 4);
                WriteAttributeValue("", 1244, "/Contract/DownloadDocument?filePath=", 1244, 36, true);
#line 31 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
WriteAttributeValue("", 1280, Model.UploadFileName, 1280, 21, false);

#line default
#line hidden
                WriteAttributeValue("", 1301, "&fileName=", 1301, 10, true);
#line 31 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
WriteAttributeValue("", 1311, fileName, 1311, 9, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginWriteAttribute("id", "\r\n                       id=\"", 1321, "\"", 1359, 1);
#line 32 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
WriteAttributeValue("", 1350, fileName, 1350, 9, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1360, 21, true);
                WriteLiteral(" class=\"text-left\">\r\n");
                EndContext();
                BeginContext(1476, 52, true);
                WriteLiteral("                        <span class=\"control-label\">");
                EndContext();
                BeginContext(1529, 8, false);
#line 35 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
                                               Write(fileName);

#line default
#line hidden
                EndContext();
                BeginContext(1537, 59, true);
                WriteLiteral("</span>\r\n                    </a>\r\n                </div>\r\n");
                EndContext();
#line 38 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
            }

#line default
#line hidden
                BeginContext(1611, 10, true);
                WriteLiteral("\r\n        ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1628, 31, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n<script");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 1659, "\"", 1724, 3);
#line 44 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
WriteAttributeValue("", 1665, cdnUrl, 1665, 7, false);

#line default
#line hidden
            WriteAttributeValue("", 1672, "/js/proj/projectfileUploadGrid.js?v=", 1672, 36, true);
#line 44 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
WriteAttributeValue("", 1708, resourceVersion, 1708, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1725, 352, true);
            WriteLiteral(@" class=""UploadGrid"" data-gridname=""#EmployeeBillingGrid""
        data-controller=""EmployeeBillingRates"" data-idvalue=""#ContractGuid"" data-guid=""billingRateGuid""
        data-fields=""laborCode|employeeName|rate|startDate|endDate"" data-titles=""Labor Code|Employee Name|Rate|Start Date|End Date""
        data-downloadgrid=""#DownloadEBRGrid"" data-path=""");
            EndContext();
            BeginContext(2078, 20, false);
#line 47 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_EmployeeBillingRatesView.cshtml"
                                                   Write(Model.UploadFileName);

#line default
#line hidden
            EndContext();
            BeginContext(2098, 13, true);
            WriteLiteral("\"></script>\r\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.EmployeeBillingRatesViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
