#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "aac434f2fd723efa3c7ef01131a09782d9ffabf1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RevenueRecognition__LaborCategoryRatesView), @"mvc.1.0.view", @"/Views/RevenueRecognition/_LaborCategoryRatesView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/RevenueRecognition/_LaborCategoryRatesView.cshtml", typeof(AspNetCore.Views_RevenueRecognition__LaborCategoryRatesView))]
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
#line 3 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aac434f2fd723efa3c7ef01131a09782d9ffabf1", @"/Views/RevenueRecognition/_LaborCategoryRatesView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_RevenueRecognition__LaborCategoryRatesView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.RevenueRecognition.RevenueRecognitionViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(87, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(170, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 6 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(285, 67, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "aac434f2fd723efa3c7ef01131a09782d9ffabf14744", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 10 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
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
            BeginContext(352, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
#line 12 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
 if (Model.ContractGuid != Guid.Empty || Model.ContractGuid != null)
{
    

#line default
#line hidden
#line 14 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
     if (Model.IsCsvLaborCategoryRate)
    {

#line default
#line hidden
            BeginContext(476, 325, true);
            WriteLiteral(@"        <div class=""row pt-3 form-group no-gutters"">
            <div class=""col"">
                <input type=""button"" id=""DownloadLCRGrid"" class=""download btn btn-sm btn-secondary"" value=""Export to CSV"" />
            </div>
        </div>
        <div class="""" id=""LaborCategoryGrid"">
        </div>
        <script");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 801, "\"", 861, 3);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 807, cdnUrl, 807, 7, false);

#line default
#line hidden
            WriteAttributeValue("", 814, "/js/proj/subLaborGridList.js?v=", 814, 31, true);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 845, resourceVersion, 845, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(862, 415, true);
            WriteLiteral(@" class=""SubLaborGridList"" data-gridname=""#LaborCategoryGrid""
                data-controller=""SubcontractorBillingRates"" data-idvalue=""#ContractGuid"" data-guid=""categoryRateGuid""
                data-fields=""subContractor|laborCode|employeeName|rate|startDate|endDate"" data-titles=""Sub Contractor|Labor Code|Employee Name|Rate|Start Date|End Date""
                data-downloadgrid=""#DownloadLCRGrid"" data-path=""");
            EndContext();
            BeginContext(1278, 19, false);
#line 26 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
                                                           Write(Model.LaborFilePath);

#line default
#line hidden
            EndContext();
            BeginContext(1297, 13, true);
            WriteLiteral("\"></script>\r\n");
            EndContext();
#line 27 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
    }
    else if (!string.IsNullOrEmpty(Model.LaborFilePath))
    {
        var fileName = @Model.LaborFilePath.Split("/").Last();
        

#line default
#line hidden
            BeginContext(1454, 49, true);
            WriteLiteral("<div class=\"col-md-6 form-group\">\r\n            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 1503, "\"", 1585, 4);
            WriteAttributeValue("", 1510, "/Contract/DownloadDocument?filePath=", 1510, 36, true);
#line 32 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 1546, Model.LaborFilePath, 1546, 20, false);

#line default
#line hidden
            WriteAttributeValue("", 1566, "&fileName=", 1566, 10, true);
#line 32 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 1576, fileName, 1576, 9, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("id", "\r\n               id=\"", 1586, "\"", 1616, 1);
#line 33 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
WriteAttributeValue("", 1607, fileName, 1607, 9, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1617, 65, true);
            WriteLiteral(" class=\"text-left\">\r\n                <span class=\"control-label\">");
            EndContext();
            BeginContext(1683, 8, false);
#line 34 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
                                       Write(fileName);

#line default
#line hidden
            EndContext();
            BeginContext(1691, 43, true);
            WriteLiteral("</span>\r\n            </a>\r\n        </div>\r\n");
            EndContext();
#line 37 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
    }

#line default
#line hidden
#line 37 "D:\ESS\ESS-Web\src\Northwind.Web\Views\RevenueRecognition\_LaborCategoryRatesView.cshtml"
     

}

#line default
#line hidden
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.RevenueRecognition.RevenueRecognitionViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
