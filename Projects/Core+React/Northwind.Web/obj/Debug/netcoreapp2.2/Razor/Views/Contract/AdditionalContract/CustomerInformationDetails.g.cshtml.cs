#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fd2690ae39d107f30c95a5d20c0afa6e718d89e0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Contract_AdditionalContract_CustomerInformationDetails), @"mvc.1.0.view", @"/Views/Contract/AdditionalContract/CustomerInformationDetails.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Contract/AdditionalContract/CustomerInformationDetails.cshtml", typeof(AspNetCore.Views_Contract_AdditionalContract_CustomerInformationDetails))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fd2690ae39d107f30c95a5d20c0afa6e718d89e0", @"/Views/Contract/AdditionalContract/CustomerInformationDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_Contract_AdditionalContract_CustomerInformationDetails : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Contract.ContractViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("control-label control-label-read"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(121, 99, true);
            WriteLiteral("\r\n<div class=\"p-4\">\r\n    <div class=\"row\">\r\n        <div class=\"col-sm-6 form-group\">\r\n            ");
            EndContext();
            BeginContext(220, 107, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fd2690ae39d107f30c95a5d20c0afa6e718d89e04127", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CustomerInformation.AwardingAgencyOffice);

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
            BeginContext(327, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.AwardingAgencyOfficeName))
            {

#line default
#line hidden
            BeginContext(438, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 480, "", 531, 1);
#line 10 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
WriteAttributeValue("", 484, Model.CustomerInformation.AwardingAgencyOffice, 484, 47, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(531, 41, true);
            WriteLiteral(" class=\"getDetailOfAgency tooltipdetail\">");
            EndContext();
            BeginContext(573, 50, false);
#line 10 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
                                                                                                                                 Write(Model.CustomerInformation.AwardingAgencyOfficeName);

#line default
#line hidden
            EndContext();
            BeginContext(623, 85, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipagency\"></div>\r\n");
            EndContext();
#line 12 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(756, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 16 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(830, 73, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-sm-6 form-group\">\r\n            ");
            EndContext();
            BeginContext(903, 106, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fd2690ae39d107f30c95a5d20c0afa6e718d89e07966", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CustomerInformation.FundingAgencyOffice);

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
            BeginContext(1009, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 21 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.FundingAgencyOfficeName))
            {

#line default
#line hidden
            BeginContext(1119, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 1161, "", 1211, 1);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
WriteAttributeValue("", 1165, Model.CustomerInformation.FundingAgencyOffice, 1165, 46, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1211, 41, true);
            WriteLiteral(" class=\"getDetailOfAgency tooltipdetail\">");
            EndContext();
            BeginContext(1253, 49, false);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
                                                                                                                                Write(Model.CustomerInformation.FundingAgencyOfficeName);

#line default
#line hidden
            EndContext();
            BeginContext(1302, 85, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipagency\"></div>\r\n");
            EndContext();
#line 25 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(1435, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 29 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(1509, 73, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-sm-6 form-group\">\r\n            ");
            EndContext();
            BeginContext(1582, 115, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fd2690ae39d107f30c95a5d20c0afa6e718d89e011814", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 33 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CustomerInformation.OfficeContractRepresentative);

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
            BeginContext(1697, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 34 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.OfficeContractRepresentativeName))
            {

#line default
#line hidden
            BeginContext(1816, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 1858, "", 1917, 1);
#line 36 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
WriteAttributeValue("", 1862, Model.CustomerInformation.OfficeContractRepresentative, 1862, 55, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1917, 42, true);
            WriteLiteral(" class=\"getDetailOfContact tooltipdetail\">");
            EndContext();
            BeginContext(1960, 58, false);
#line 36 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
                                                                                                                                          Write(Model.CustomerInformation.OfficeContractRepresentativeName);

#line default
#line hidden
            EndContext();
            BeginContext(2018, 86, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipcontact\"></div>\r\n");
            EndContext();
#line 38 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(2152, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 42 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(2226, 73, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-sm-6 form-group\">\r\n            ");
            EndContext();
            BeginContext(2299, 122, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fd2690ae39d107f30c95a5d20c0afa6e718d89e015711", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 46 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CustomerInformation.FundingOfficeContractRepresentative);

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
            BeginContext(2421, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 47 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.FundingOfficeContractRepresentativeName))
            {

#line default
#line hidden
            BeginContext(2547, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 2589, "", 2655, 1);
#line 49 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
WriteAttributeValue("", 2593, Model.CustomerInformation.FundingOfficeContractRepresentative, 2593, 62, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2655, 42, true);
            WriteLiteral(" class=\"getDetailOfContact tooltipdetail\">");
            EndContext();
            BeginContext(2698, 65, false);
#line 49 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
                                                                                                                                                 Write(Model.CustomerInformation.FundingOfficeContractRepresentativeName);

#line default
#line hidden
            EndContext();
            BeginContext(2763, 86, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipcontact\"></div>\r\n");
            EndContext();
#line 51 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(2897, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 55 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(2971, 73, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-sm-6 form-group\">\r\n            ");
            EndContext();
            BeginContext(3044, 119, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fd2690ae39d107f30c95a5d20c0afa6e718d89e019643", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 59 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CustomerInformation.OfficeContractTechnicalRepresent);

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
            BeginContext(3163, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 60 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.OfficeContractTechnicalRepresentName))
            { 

#line default
#line hidden
            BeginContext(3287, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 3329, "", 3392, 1);
#line 62 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
WriteAttributeValue("", 3333, Model.CustomerInformation.OfficeContractTechnicalRepresent, 3333, 59, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3392, 42, true);
            WriteLiteral(" class=\"getDetailOfContact tooltipdetail\">");
            EndContext();
            BeginContext(3435, 62, false);
#line 62 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
                                                                                                                                              Write(Model.CustomerInformation.OfficeContractTechnicalRepresentName);

#line default
#line hidden
            EndContext();
            BeginContext(3497, 86, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipcontact\"></div>\r\n");
            EndContext();
#line 64 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(3631, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 68 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(3705, 73, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-sm-6 form-group\">\r\n            ");
            EndContext();
            BeginContext(3778, 126, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fd2690ae39d107f30c95a5d20c0afa6e718d89e023561", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 72 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.CustomerInformation.FundingOfficeContractTechnicalRepresent);

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
            BeginContext(3904, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 73 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.FundingOfficeContractTechnicalRepresentName))
            {

#line default
#line hidden
            BeginContext(4034, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 4076, "", 4146, 1);
#line 75 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
WriteAttributeValue("", 4080, Model.CustomerInformation.FundingOfficeContractTechnicalRepresent, 4080, 66, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(4146, 42, true);
            WriteLiteral(" class=\"getDetailOfContact tooltipdetail\">");
            EndContext();
            BeginContext(4189, 69, false);
#line 75 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
                                                                                                                                                     Write(Model.CustomerInformation.FundingOfficeContractTechnicalRepresentName);

#line default
#line hidden
            EndContext();
            BeginContext(4258, 86, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipcontact\"></div>\r\n");
            EndContext();
#line 77 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(4392, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 81 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Contract\AdditionalContract\CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(4466, 36, true);
            WriteLiteral("        </div>\r\n\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Contract.ContractViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591