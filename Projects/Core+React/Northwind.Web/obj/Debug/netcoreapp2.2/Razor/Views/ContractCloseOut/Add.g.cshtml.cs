#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "365b9c72c419e9b0481999e93863be34df6a03ec"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ContractCloseOut_Add), @"mvc.1.0.view", @"/Views/ContractCloseOut/Add.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/ContractCloseOut/Add.cshtml", typeof(AspNetCore.Views_ContractCloseOut_Add))]
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
#line 2 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"365b9c72c419e9b0481999e93863be34df6a03ec", @"/Views/ContractCloseOut/Add.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_ContractCloseOut_Add : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Questionaire.ContractCloseOutViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_FormModel.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(273, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 9 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
  
    ViewData["Title"] = "Contract : Close Out";
    Layout = "/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            DefineSection("breadcrumb", async() => {
                BeginContext(403, 82, true);
                WriteLiteral("\r\n    <li class=\"breadcrumb-item\"><a href=\"/Contract\">List Of Contracts</a></li>\r\n");
                EndContext();
#line 16 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
     if (Model.ParentContractGuid != Guid.Empty)
    {

#line default
#line hidden
                BeginContext(542, 52, true);
                WriteLiteral("        <li class=\"breadcrumb-item\">\r\n            <a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 594, "\"", 644, 2);
                WriteAttributeValue("", 601, "/Contract/Details/", 601, 18, true);
#line 19 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
WriteAttributeValue("", 619, Model.ParentContractGuid, 619, 25, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(645, 1, true);
                WriteLiteral(">");
                EndContext();
                BeginContext(647, 27, false);
#line 19 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
                                                             Write(ViewBag.ParentProjectNumber);

#line default
#line hidden
                EndContext();
                BeginContext(674, 39, true);
                WriteLiteral(" : Contract Detail</a>\r\n        </li>\r\n");
                EndContext();
                BeginContext(715, 52, true);
                WriteLiteral("        <li class=\"breadcrumb-item\">\r\n            <a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 767, "\"", 810, 2);
                WriteAttributeValue("", 774, "/Project/Details/", 774, 17, true);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
WriteAttributeValue("", 791, Model.ContractGuid, 791, 19, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(811, 1, true);
                WriteLiteral(">");
                EndContext();
                BeginContext(813, 21, false);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
                                                      Write(ViewBag.ProjectNumber);

#line default
#line hidden
                EndContext();
                BeginContext(834, 42, true);
                WriteLiteral("  : Task Order Detail</a>\r\n        </li>\r\n");
                EndContext();
                BeginContext(878, 80, true);
                WriteLiteral("        <li class=\"breadcrumb-item\"><a href=\"#\">Task Order Close Out </a></li>\r\n");
                EndContext();
#line 27 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
    }
    else
    {

#line default
#line hidden
                BeginContext(982, 52, true);
                WriteLiteral("        <li class=\"breadcrumb-item\">\r\n            <a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 1034, "\"", 1078, 2);
                WriteAttributeValue("", 1041, "/Contract/Details/", 1041, 18, true);
#line 31 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
WriteAttributeValue("", 1059, Model.ContractGuid, 1059, 19, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1079, 1, true);
                WriteLiteral(">");
                EndContext();
                BeginContext(1081, 21, false);
#line 31 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
                                                       Write(ViewBag.ProjectNumber);

#line default
#line hidden
                EndContext();
                BeginContext(1102, 40, true);
                WriteLiteral("  : Contract Detail</a>\r\n        </li>\r\n");
                EndContext();
                BeginContext(1144, 77, true);
                WriteLiteral("        <li class=\"breadcrumb-item\"><a href=\"#\">Contract Close Out</a></li>\r\n");
                EndContext();
#line 35 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
    }

#line default
#line hidden
            }
            );
            BeginContext(1231, 36, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "365b9c72c419e9b0481999e93863be34df6a03ec8691", async() => {
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
            BeginContext(1267, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1292, 15, true);
                WriteLiteral("\r\n\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 1307, "\"", 1361, 3);
#line 41 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
WriteAttributeValue("", 1313, cdnUrl, 1313, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 1320, "/js/dist/folderTree.js?v=", 1320, 25, true);
#line 41 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
WriteAttributeValue("", 1345, resourceVersion, 1345, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1362, 25, true);
                WriteLiteral("></script>\r\n\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 1387, "\"", 1444, 3);
#line 43 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
WriteAttributeValue("", 1393, cdnUrl, 1393, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 1400, "/js/proj/contractClose.js?v=", 1400, 28, true);
#line 43 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
WriteAttributeValue("", 1428, resourceVersion, 1428, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1445, 280, true);
                WriteLiteral(@"></script>


    <script>
    //assign in uploader variable the component because we need onSubmitFiles method from component after the contract close page submit succesfully from ajax ..
        window.uploader = window.loadFileUpload.pageView.loadFileUpload('fileUpload', '");
                EndContext();
                BeginContext(1726, 19, false);
#line 48 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
                                                                                  Write(ViewBag.Resourcekey);

#line default
#line hidden
                EndContext();
                BeginContext(1745, 23, true);
                WriteLiteral("\', true,\r\n            \'");
                EndContext();
                BeginContext(1769, 18, false);
#line 49 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
        Write(ViewBag.ResourceId);

#line default
#line hidden
                EndContext();
                BeginContext(1787, 4, true);
                WriteLiteral("\', \'");
                EndContext();
                BeginContext(1792, 17, false);
#line 49 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
                               Write(ViewBag.UpdatedBy);

#line default
#line hidden
                EndContext();
                BeginContext(1809, 4, true);
                WriteLiteral("\', \'");
                EndContext();
                BeginContext(1814, 17, false);
#line 49 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractCloseOut\Add.cshtml"
                                                     Write(ViewBag.UpdatedOn);

#line default
#line hidden
                EndContext();
                BeginContext(1831, 186, true);
                WriteLiteral("\', \'No path\', true, true, false, true, false, submitCallBack);\r\n\r\n        function submitCallBack() {\r\n            // finally redirect to  notification page..\r\n        }\r\n    </script>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Questionaire.ContractCloseOutViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
