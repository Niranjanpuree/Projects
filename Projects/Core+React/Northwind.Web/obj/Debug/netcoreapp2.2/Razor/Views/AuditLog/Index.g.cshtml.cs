#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "49dae21aaacbd9459d98411e7c43e87aa206bf34"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_AuditLog_Index), @"mvc.1.0.view", @"/Views/AuditLog/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/AuditLog/Index.cshtml", typeof(AspNetCore.Views_AuditLog_Index))]
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
#line 5 "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"49dae21aaacbd9459d98411e7c43e87aa206bf34", @"/Views/AuditLog/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_AuditLog_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml"
  
    ViewData["Title"] = "Audit Log";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            DefineSection("breadcrumb", async() => {
                BeginContext(312, 144, true);
                WriteLiteral("\r\n    <li class=\"breadcrumb-item\"><a href=\"/Admin/Settings\">Settings</a></li>\r\n    <li class=\"breadcrumb-item\"><a href=\"#\">Audit Logs</a></li>\r\n");
                EndContext();
            }
            );
            BeginContext(459, 35, true);
            WriteLiteral("\r\n<div id=\"auditLogGrid\"></div>\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(512, 13, true);
                WriteLiteral("\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 525, "\"", 575, 3);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml"
WriteAttributeValue("", 531, cdnUrl, 531, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 538, "/js/dist/dialog.js?v=", 538, 21, true);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml"
WriteAttributeValue("", 559, resourceVersion, 559, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(576, 23, true);
                WriteLiteral("></script>\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 599, "\"", 652, 3);
#line 21 "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml"
WriteAttributeValue("", 605, cdnUrl, 605, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 612, "/js/dist/kendogrid.js?v=", 612, 24, true);
#line 21 "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml"
WriteAttributeValue("", 636, resourceVersion, 636, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(653, 144, true);
                WriteLiteral("></script>\r\n\r\n    <script type=\"text/javascript\">\r\n        (function ()\r\n        {\r\n            window.auditLog.auditLogList(\'Audit Log List\', \'");
                EndContext();
                BeginContext(798, 71, false);
#line 26 "D:\ESS\ESS-Web\src\Northwind.Web\Views\AuditLog\Index.cshtml"
                                                       Write(User.Claims.Where(c=>c.Type.ToString() == "fullName").ToList()[0].Value);

#line default
#line hidden
                EndContext();
                BeginContext(869, 34, true);
                WriteLiteral("\');\r\n        })()\r\n    </script>\r\n");
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
