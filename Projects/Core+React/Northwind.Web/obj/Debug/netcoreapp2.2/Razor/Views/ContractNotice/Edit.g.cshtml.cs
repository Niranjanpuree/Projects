#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractNotice\Edit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b8189b9fa65ae846f73a6d0aa9df82a330f877be"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ContractNotice_Edit), @"mvc.1.0.view", @"/Views/ContractNotice/Edit.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/ContractNotice/Edit.cshtml", typeof(AspNetCore.Views_ContractNotice_Edit))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b8189b9fa65ae846f73a6d0aa9df82a330f877be", @"/Views/ContractNotice/Edit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_ContractNotice_Edit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Contract.ContractNoticeViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(73, 7, true);
            WriteLiteral("<div>\r\n");
            EndContext();
#line 3 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractNotice\Edit.cshtml"
      Html.RenderPartial("ContractNoticeFormModel");

#line default
#line hidden
            BeginContext(135, 158, true);
            WriteLiteral("</div>\r\n<script>\r\n     //call submit method from contract index.tsx\r\n    window.uploaderMod = window.loadFileUpload.pageView.loadFileUpload(\'fileUploadMod\', \'");
            EndContext();
            BeginContext(294, 19, false);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractNotice\Edit.cshtml"
                                                                                    Write(ViewBag.Resourcekey);

#line default
#line hidden
            EndContext();
            BeginContext(313, 10, true);
            WriteLiteral("\', true, \'");
            EndContext();
            BeginContext(324, 24, false);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractNotice\Edit.cshtml"
                                                                                                                  Write(Model.ContractNoticeGuid);

#line default
#line hidden
            EndContext();
            BeginContext(348, 4, true);
            WriteLiteral("\', \'");
            EndContext();
            BeginContext(353, 17, false);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractNotice\Edit.cshtml"
                                                                                                                                               Write(ViewBag.UpdatedBy);

#line default
#line hidden
            EndContext();
            BeginContext(370, 4, true);
            WriteLiteral("\', \'");
            EndContext();
            BeginContext(375, 17, false);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractNotice\Edit.cshtml"
                                                                                                                                                                     Write(ViewBag.UpdatedOn);

#line default
#line hidden
            EndContext();
            BeginContext(392, 233, true);
            WriteLiteral("\', \'No path\', true, false, true, true, false, submitCallBack,true);\r\n\r\n    function submitCallBack() {\r\n        \r\n    }\r\n</script>\r\n<script>\r\n    function notifyCallBack() {\r\n\r\n    }\r\n    function skipCallBack() {\r\n\r\n    }\r\n</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Contract.ContractNoticeViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
