#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "35d53ea3ebf7ce285ef871222163db8e6eb7c7b0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Test_Index), @"mvc.1.0.view", @"/Views/Test/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Test/Index.cshtml", typeof(AspNetCore.Views_Test_Index))]
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
#line 2 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"35d53ea3ebf7ce285ef871222163db8e6eb7c7b0", @"/Views/Test/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_Test_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(83, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 5 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(198, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 10 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(288, 39, true);
            WriteLiteral("\r\n<div id=\"distributionList\"></div>\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(345, 13, true);
                WriteLiteral("\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 358, "\"", 408, 3);
#line 18 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
WriteAttributeValue("", 364, cdnUrl, 364, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 371, "/js/dist/dialog.js?v=", 371, 21, true);
#line 18 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
WriteAttributeValue("", 392, resourceVersion, 392, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(409, 23, true);
                WriteLiteral("></script>\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 432, "\"", 485, 3);
#line 19 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
WriteAttributeValue("", 438, cdnUrl, 438, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 445, "/js/dist/kendogrid.js?v=", 445, 24, true);
#line 19 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
WriteAttributeValue("", 469, resourceVersion, 469, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(486, 23, true);
                WriteLiteral("></script>\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 509, "\"", 569, 3);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
WriteAttributeValue("", 515, cdnUrl, 515, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 522, "/js/dist/distributionlist.js?v=", 522, 31, true);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Test\Index.cshtml"
WriteAttributeValue("", 553, resourceVersion, 553, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(570, 171, true);
                WriteLiteral("></script>\r\n    <script type=\"text/javascript\">\r\n        (function () {\r\n            window.distributionList.pageView.loadDistributionList()\r\n        })()\r\n    </script>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591