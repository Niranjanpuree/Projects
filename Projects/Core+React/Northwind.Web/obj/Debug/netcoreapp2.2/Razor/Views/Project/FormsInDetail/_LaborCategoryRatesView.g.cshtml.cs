#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3d3d65f6171c4282db08afbee7f9050ae9e2f56c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Project_FormsInDetail__LaborCategoryRatesView), @"mvc.1.0.view", @"/Views/Project/FormsInDetail/_LaborCategoryRatesView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Project/FormsInDetail/_LaborCategoryRatesView.cshtml", typeof(AspNetCore.Views_Project_FormsInDetail__LaborCategoryRatesView))]
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
#line 2 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3d3d65f6171c4282db08afbee7f9050ae9e2f56c", @"/Views/Project/FormsInDetail/_LaborCategoryRatesView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_Project_FormsInDetail__LaborCategoryRatesView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.LaborCategoryRatesViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "_LaborCategoryRatesView", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(262, 56, true);
            WriteLiteral("<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        ");
            EndContext();
            BeginContext(318, 1305, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3d3d65f6171c4282db08afbee7f9050ae9e2f56c5523", async() => {
                BeginContext(361, 14, true);
                WriteLiteral("\r\n            ");
                EndContext();
                BeginContext(375, 66, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3d3d65f6171c4282db08afbee7f9050ae9e2f56c5917", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#line 11 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
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
                BeginContext(441, 37, true);
                WriteLiteral("\r\n            <div>\r\n                ");
                EndContext();
                BeginContext(478, 64, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3d3d65f6171c4282db08afbee7f9050ae9e2f56c7750", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
#line 13 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CategoryRateGuid);

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
                BeginContext(542, 18, true);
                WriteLiteral("\r\n                ");
                EndContext();
                BeginContext(560, 60, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3d3d65f6171c4282db08afbee7f9050ae9e2f56c9798", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
#line 14 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
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
                BeginContext(620, 24, true);
                WriteLiteral("\r\n            </div>\r\n\r\n");
                EndContext();
#line 17 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
             if (Model.IsCsv)
            {

#line default
#line hidden
                BeginContext(690, 275, true);
                WriteLiteral(@"                <div class=""row form-group no-gutters"">
                    <div class=""col"">
                        <input type=""button"" id=""DownloadLCRGrid"" class=""download btn btn-secondary"" value=""Export to CSV"" />
                    </div>
                </div>
");
                EndContext();
                BeginContext(967, 71, true);
                WriteLiteral("                <div class=\"form-group\" id=\"LaborCategoryGrid\"></div>\r\n");
                EndContext();
#line 26 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
            }
            else
            {
                var fileName = @Model.UploadFileName.Split("/").Last();
                

#line default
#line hidden
                BeginContext(1175, 57, true);
                WriteLiteral("<div class=\"col-md-6 form-group\">\r\n                    <a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 1232, "\"", 1315, 4);
                WriteAttributeValue("", 1239, "/Contract/DownloadDocument?filePath=", 1239, 36, true);
#line 31 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 1275, Model.UploadFileName, 1275, 21, false);

#line default
#line hidden
                WriteAttributeValue("", 1296, "&fileName=", 1296, 10, true);
#line 31 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 1306, fileName, 1306, 9, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginWriteAttribute("id", "\r\n                       id=\"", 1316, "\"", 1354, 1);
#line 32 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 1345, fileName, 1345, 9, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1355, 21, true);
                WriteLiteral(" class=\"text-left\">\r\n");
                EndContext();
                BeginContext(1471, 52, true);
                WriteLiteral("                        <span class=\"control-label\">");
                EndContext();
                BeginContext(1524, 8, false);
#line 35 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
                                               Write(fileName);

#line default
#line hidden
                EndContext();
                BeginContext(1532, 59, true);
                WriteLiteral("</span>\r\n                    </a>\r\n                </div>\r\n");
                EndContext();
#line 38 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
            }

#line default
#line hidden
                BeginContext(1606, 10, true);
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
            BeginContext(1623, 31, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n<script");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 1654, "\"", 1719, 3);
#line 44 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 1660, cdnUrl, 1660, 7, false);

#line default
#line hidden
            WriteAttributeValue("", 1667, "/js/proj/projectfileUploadGrid.js?v=", 1667, 36, true);
#line 44 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 1703, resourceVersion, 1703, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1720, 384, true);
            WriteLiteral(@" class=""UploadGrid"" data-gridname=""#LaborCategoryGrid""
        data-controller=""SubcontractorBillingRates"" data-idvalue=""#ContractGuid"" data-guid=""categoryRateGuid""
        data-fields=""subContractor|laborCode|employeeName|rate|startDate|endDate"" data-titles=""Sub Contractor|Labor Code|Employee Name|Rate|Start Date|End Date""
        data-downloadgrid=""#DownloadLCRGrid"" data-path=");
            EndContext();
            BeginContext(2105, 20, false);
#line 47 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_LaborCategoryRatesView.cshtml"
                                                  Write(Model.UploadFileName);

#line default
#line hidden
            EndContext();
            BeginContext(2125, 12, true);
            WriteLiteral("></script>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.LaborCategoryRatesViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
