#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cc852d099a40fc03605c422482f21420503f1c97"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Project_FormsInDetail__CustomerInformationDetails), @"mvc.1.0.view", @"/Views/Project/FormsInDetail/_CustomerInformationDetails.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Project/FormsInDetail/_CustomerInformationDetails.cshtml", typeof(AspNetCore.Views_Project_FormsInDetail__CustomerInformationDetails))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cc852d099a40fc03605c422482f21420503f1c97", @"/Views/Project/FormsInDetail/_CustomerInformationDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_Project_FormsInDetail__CustomerInformationDetails : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Contract.ContractViewModel>
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
            WriteLiteral("\r\n<div class=\"p-4\">\r\n    <div class=\"row\">\r\n        <div class=\"col-md-6 form-group\">\r\n            ");
            EndContext();
            BeginContext(220, 107, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "cc852d099a40fc03605c422482f21420503f1c974092", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
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
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.AwardingAgencyOfficeName))
            {

#line default
#line hidden
            BeginContext(438, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 480, "", 531, 1);
#line 10 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
WriteAttributeValue("", 484, Model.CustomerInformation.AwardingAgencyOffice, 484, 47, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(531, 41, true);
            WriteLiteral(" class=\"getDetailOfAgency tooltipdetail\">");
            EndContext();
            BeginContext(573, 68, false);
#line 10 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
                                                                                                                                 Write(Html.DisplayFor(m => m.CustomerInformation.AwardingAgencyOfficeName));

#line default
#line hidden
            EndContext();
            BeginContext(641, 85, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipagency\"></div>\r\n");
            EndContext();
#line 12 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(774, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 16 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(848, 73, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-md-6 form-group\">\r\n            ");
            EndContext();
            BeginContext(921, 106, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "cc852d099a40fc03605c422482f21420503f1c977919", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
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
            BeginContext(1027, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 21 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.FundingAgencyOfficeName))
            {

#line default
#line hidden
            BeginContext(1137, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 1179, "", 1229, 1);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
WriteAttributeValue("", 1183, Model.CustomerInformation.FundingAgencyOffice, 1183, 46, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1229, 41, true);
            WriteLiteral(" class=\"getDetailOfAgency tooltipdetail\">");
            EndContext();
            BeginContext(1271, 67, false);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
                                                                                                                                Write(Html.DisplayFor(m => m.CustomerInformation.FundingAgencyOfficeName));

#line default
#line hidden
            EndContext();
            BeginContext(1338, 85, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipagency\"></div>\r\n");
            EndContext();
#line 25 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(1471, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 29 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(1545, 156, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-md-6 form-group\">\r\n            <label class=\"control-label control-label-read\">Task Order Representative</label>\r\n");
            EndContext();
#line 34 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.OfficeContractRepresentativeName))
            {

#line default
#line hidden
            BeginContext(1818, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 1860, "", 1919, 1);
#line 36 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
WriteAttributeValue("", 1864, Model.CustomerInformation.OfficeContractRepresentative, 1864, 55, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1919, 42, true);
            WriteLiteral(" class=\"getDetailOfContact tooltipdetail\">");
            EndContext();
            BeginContext(1962, 76, false);
#line 36 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
                                                                                                                                          Write(Html.DisplayFor(m => m.CustomerInformation.OfficeContractRepresentativeName));

#line default
#line hidden
            EndContext();
            BeginContext(2038, 86, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipcontact\"></div>\r\n");
            EndContext();
#line 38 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(2172, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 42 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(2246, 156, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-md-6 form-group\">\r\n            <label class=\"control-label control-label-read\">Task Order Representative</label>\r\n");
            EndContext();
#line 47 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.FundingOfficeContractRepresentativeName))
            {

#line default
#line hidden
            BeginContext(2526, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 2568, "", 2634, 1);
#line 49 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
WriteAttributeValue("", 2572, Model.CustomerInformation.FundingOfficeContractRepresentative, 2572, 62, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2634, 42, true);
            WriteLiteral(" class=\"getDetailOfContact tooltipdetail\">");
            EndContext();
            BeginContext(2677, 83, false);
#line 49 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
                                                                                                                                                 Write(Html.DisplayFor(m => m.CustomerInformation.FundingOfficeContractRepresentativeName));

#line default
#line hidden
            EndContext();
            BeginContext(2760, 86, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipcontact\"></div>\r\n");
            EndContext();
#line 51 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(2894, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 55 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(2968, 166, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-md-6 form-group\">\r\n            <label class=\"control-label control-label-read\">Task Order Technical Representative</label>\r\n");
            EndContext();
#line 60 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.OfficeContractTechnicalRepresentName))
            {

#line default
#line hidden
            BeginContext(3255, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 3297, "", 3360, 1);
#line 62 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
WriteAttributeValue("", 3301, Model.CustomerInformation.OfficeContractTechnicalRepresent, 3301, 59, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3360, 42, true);
            WriteLiteral(" class=\"getDetailOfContact tooltipdetail\">");
            EndContext();
            BeginContext(3403, 80, false);
#line 62 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
                                                                                                                                              Write(Html.DisplayFor(m => m.CustomerInformation.OfficeContractTechnicalRepresentName));

#line default
#line hidden
            EndContext();
            BeginContext(3483, 86, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipcontact\"></div>\r\n");
            EndContext();
#line 64 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(3617, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 68 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(3691, 166, true);
            WriteLiteral("        </div>\r\n\r\n        <div class=\"col-md-6 form-group\">\r\n            <label class=\"control-label control-label-read\">Task Order Technical Representative</label>\r\n");
            EndContext();
#line 73 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
             if (!string.IsNullOrEmpty(Model.CustomerInformation?.FundingOfficeContractTechnicalRepresentName))
            {

#line default
#line hidden
            BeginContext(3985, 42, true);
            WriteLiteral("                <div class=\"form-value\"><b");
            EndContext();
            BeginWriteAttribute("id", " id=", 4027, "", 4097, 1);
#line 75 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
WriteAttributeValue("", 4031, Model.CustomerInformation.FundingOfficeContractTechnicalRepresent, 4031, 66, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(4097, 42, true);
            WriteLiteral(" class=\"getDetailOfContact tooltipdetail\">");
            EndContext();
            BeginContext(4140, 87, false);
#line 75 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
                                                                                                                                                     Write(Html.DisplayFor(m => m.CustomerInformation.FundingOfficeContractTechnicalRepresentName));

#line default
#line hidden
            EndContext();
            BeginContext(4227, 86, true);
            WriteLiteral("</b></div>\r\n                <div class=\"popover-detail bottom tooltipcontact\"></div>\r\n");
            EndContext();
#line 77 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(4361, 59, true);
            WriteLiteral("                <div class=\"form-value\">Not Entered</div>\r\n");
            EndContext();
#line 81 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Project\FormsInDetail\_CustomerInformationDetails.cshtml"
            }

#line default
#line hidden
            BeginContext(4435, 34, true);
            WriteLiteral("        </div>\r\n    </div>\r\n</div>");
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