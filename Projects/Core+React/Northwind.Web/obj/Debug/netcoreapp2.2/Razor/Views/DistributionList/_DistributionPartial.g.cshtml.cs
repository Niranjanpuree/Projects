#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dddaab235d7df0bfbe002512541688e18bcad3f5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_DistributionList__DistributionPartial), @"mvc.1.0.view", @"/Views/DistributionList/_DistributionPartial.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/DistributionList/_DistributionPartial.cshtml", typeof(AspNetCore.Views_DistributionList__DistributionPartial))]
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
#line 5 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dddaab235d7df0bfbe002512541688e18bcad3f5", @"/Views/DistributionList/_DistributionPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_DistributionList__DistributionPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
  
    ViewData["Title"] = "User Distribution";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(294, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
            DefineSection("breadcrumb", async() => {
                BeginContext(324, 74, true);
                WriteLiteral("\r\n    <li class=\"breadcrumb-item\"><a href=\"#\">User Distribution</a></li>\r\n");
                EndContext();
            }
            );
            BeginContext(401, 46, true);
            WriteLiteral("\r\n<div id=\"distributionList\"></div>\r\n\r\n<script");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 447, "\"", 509, 3);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
WriteAttributeValue("", 453, cdnUrl, 453, 7, false);

#line default
#line hidden
            WriteAttributeValue("", 460, "/lib/jquery/dist/jquery.min.js?v=", 460, 33, true);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
WriteAttributeValue("", 493, resourceVersion, 493, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(510, 21, true);
            WriteLiteral("></script>\r\n\r\n<script");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 531, "\"", 581, 3);
#line 22 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
WriteAttributeValue("", 537, cdnUrl, 537, 7, false);

#line default
#line hidden
            WriteAttributeValue("", 544, "/js/dist/dialog.js?v=", 544, 21, true);
#line 22 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
WriteAttributeValue("", 565, resourceVersion, 565, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(582, 19, true);
            WriteLiteral("></script>\r\n<script");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 601, "\"", 654, 3);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
WriteAttributeValue("", 607, cdnUrl, 607, 7, false);

#line default
#line hidden
            WriteAttributeValue("", 614, "/js/dist/kendogrid.js?v=", 614, 24, true);
#line 23 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
WriteAttributeValue("", 638, resourceVersion, 638, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(655, 21, true);
            WriteLiteral("></script>\r\n\r\n<script");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 676, "\"", 736, 3);
#line 25 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
WriteAttributeValue("", 682, cdnUrl, 682, 7, false);

#line default
#line hidden
            WriteAttributeValue("", 689, "/js/dist/distributionlist.js?v=", 689, 31, true);
#line 25 "D:\ESS\ESS-Web\src\Northwind.Web\Views\DistributionList\_DistributionPartial.cshtml"
WriteAttributeValue("", 720, resourceVersion, 720, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(737, 537, true);
            WriteLiteral(@"></script>
<script type=""text/javascript"">

    function notifyCallBack(e, e1) {
        //redirect to url..
    }
    function skipCallBack(e, e1) {
        //redirect to url..
    }

    (function () {
        window.loadDistributionListDialog.pageView.loadDistributionListDialog('distributionList',
            'key',
            'resourceId',
            true,
            true,
            false,
            false,
            true,
            notifyCallBack,
            skipCallBack);
    })();
</script>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
