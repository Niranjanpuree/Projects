#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3bfa609df525fa990f014848622096034e6e1ae7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ProjectModification_Details), @"mvc.1.0.view", @"/Views/ProjectModification/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/ProjectModification/Details.cshtml", typeof(AspNetCore.Views_ProjectModification_Details))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3bfa609df525fa990f014848622096034e6e1ae7", @"/Views/ProjectModification/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_ProjectModification_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Contract.ContractModificationViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(79, 47, true);
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col-md-6 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 126, "\"", 151, 1);
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 131, Model.ProjectNumber, 131, 20, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(152, 171, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Task Order Number\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(324, 19, false);
#line 10 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
           Write(Model.ProjectNumber);

#line default
#line hidden
            EndContext();
            BeginContext(343, 75, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 418, "\"", 443, 1);
#line 14 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 423, Model.ContractTitle, 423, 20, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(444, 170, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Task Order Title\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(615, 19, false);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
           Write(Model.ContractTitle);

#line default
#line hidden
            EndContext();
            BeginContext(634, 75, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 709, "\"", 739, 1);
#line 24 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 714, Model.ModificationNumber, 714, 25, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(740, 173, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Modification Number\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(914, 24, false);
#line 30 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
           Write(Model.ModificationNumber);

#line default
#line hidden
            EndContext();
            BeginContext(938, 74, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1012, "\"", 1040, 1);
#line 34 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 1017, Model.ModificationType, 1017, 23, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1041, 171, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Modification Type\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(1214, 85, false);
#line 40 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
            Write(string.IsNullOrEmpty(Model.ModificationType) ? "Not Entered" : Model.ModificationType);

#line default
#line hidden
            EndContext();
            BeginContext(1300, 75, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1375, "\"", 1404, 1);
#line 44 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 1380, Model.ModificationTitle, 1380, 24, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1405, 172, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Modification Title\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(1578, 23, false);
#line 50 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
           Write(Model.ModificationTitle);

#line default
#line hidden
            EndContext();
            BeginContext(1601, 75, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1676, "\"", 1699, 1);
#line 54 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 1681, Model.AwardAmount, 1681, 18, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1700, 166, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Award Amount\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(1867, 26, false);
#line 60 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
           Write(Model.FormattedAwardAmount);

#line default
#line hidden
            EndContext();
            BeginContext(1893, 75, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1968, "\"", 1993, 1);
#line 64 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 1973, Model.FundingAmount, 1973, 20, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1994, 167, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Funded Amount\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(2162, 28, false);
#line 70 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
           Write(Model.FormattedFundingAmount);

#line default
#line hidden
            EndContext();
            BeginContext(2190, 75, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 2265, "\"", 2285, 1);
#line 74 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 2270, Model.POPStart, 2270, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2286, 186, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Period of performance Start Date\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(2474, 108, false);
#line 80 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
            Write(string.IsNullOrWhiteSpace(Model.POPStart?.ToString()) ? "No change" : Model.POPStart?.ToString("MM/dd/yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(2583, 75, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 2658, "\"", 2676, 1);
#line 84 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 2663, Model.POPEnd, 2663, 13, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2677, 184, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Period of performance End Date\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(2863, 104, false);
#line 90 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
            Write(string.IsNullOrWhiteSpace(Model.POPEnd?.ToString()) ? "No change" : Model.POPEnd?.ToString("MM/dd/yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(2968, 76, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-12 \"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 3044, "\"", 3067, 1);
#line 94 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
WriteAttributeValue("", 3049, Model.Description, 3049, 18, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3068, 159, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Notes\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(3228, 17, false);
#line 100 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
           Write(Model.Description);

#line default
#line hidden
            EndContext();
            BeginContext(3245, 214, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-12\">\r\n        <div class=\"d-flex mb-3 justify-content-between\">\r\n            <h5 class=\"mb-0\">\r\n                Mod Files\r\n            </h5>\r\n");
            EndContext();
            BeginContext(3692, 243, true);
            WriteLiteral("        </div>\r\n        <div class=\"border bg-light text-center\">\r\n            <div id=\"fileUploadDescription\"></div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<script>\r\n    window.loadFileUpload.pageView.loadFileUpload(\'fileUploadDescription\', \'");
            EndContext();
            BeginContext(3936, 19, false);
#line 118 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
                                                                       Write(ViewBag.Resourcekey);

#line default
#line hidden
            EndContext();
            BeginContext(3955, 10, true);
            WriteLiteral("\',false, \'");
            EndContext();
            BeginContext(3966, 18, false);
#line 118 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
                                                                                                     Write(ViewBag.ResourceId);

#line default
#line hidden
            EndContext();
            BeginContext(3984, 4, true);
            WriteLiteral("\', \'");
            EndContext();
            BeginContext(3989, 17, false);
#line 118 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
                                                                                                                            Write(ViewBag.UpdatedBy);

#line default
#line hidden
            EndContext();
            BeginContext(4006, 4, true);
            WriteLiteral("\', \'");
            EndContext();
            BeginContext(4011, 17, false);
#line 118 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
                                                                                                                                                  Write(ViewBag.UpdatedOn);

#line default
#line hidden
            EndContext();
            BeginContext(4028, 4, true);
            WriteLiteral("\', \"");
            EndContext();
            BeginContext(4033, 37, false);
#line 118 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ProjectModification\Details.cshtml"
                                                                                                                                                                        Write(ViewBag.FilePath.Replace("\\","\\\\"));

#line default
#line hidden
            EndContext();
            BeginContext(4070, 43, true);
            WriteLiteral("\", true, false,false,true,true);\r\n</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Contract.ContractModificationViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
