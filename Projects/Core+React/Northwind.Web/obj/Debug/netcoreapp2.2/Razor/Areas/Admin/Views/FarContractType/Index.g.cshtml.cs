#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b0e2fd3f0fbd8b02b70b06001543a612e3b669b3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_FarContractType_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/FarContractType/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/FarContractType/Index.cshtml", typeof(AspNetCore.Areas_Admin_Views_FarContractType_Index))]
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
#line 5 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b0e2fd3f0fbd8b02b70b06001543a612e3b669b3", @"/Areas/Admin/Views/FarContractType/Index.cshtml")]
    public class Areas_Admin_Views_FarContractType_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml"
  
    ViewData["Title"] = "Far Contract Type";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(294, 181, true);
            WriteLiteral("\r\n<style>\r\n    .auditLog_grid .btn-link {\r\n        color: #00649B;\r\n    }\r\n\r\n        .auditLog_grid .btn-link:hover {\r\n            text-decoration: underline;\r\n        }\r\n</style>\r\n");
            EndContext();
            DefineSection("breadcrumb", async() => {
                BeginContext(501, 151, true);
                WriteLiteral("\r\n    <li class=\"breadcrumb-item\"><a href=\"/Admin/Settings\">Settings</a></li>\r\n    <li class=\"breadcrumb-item\"><a href=\"#\">Far Contract Type</a></li>\r\n");
                EndContext();
            }
            );
            BeginContext(655, 102, true);
            WriteLiteral("\r\n<div id=\"farContractTypeGrid\"></div>\r\n<div id=\"dialog\">\r\n    <div class=\"content\"></div>\r\n</div>\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(775, 13, true);
                WriteLiteral("\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 788, "\"", 838, 3);
#line 33 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml"
WriteAttributeValue("", 794, cdnUrl, 794, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 801, "/js/dist/dialog.js?v=", 801, 21, true);
#line 33 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml"
WriteAttributeValue("", 822, resourceVersion, 822, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(839, 23, true);
                WriteLiteral("></script>\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 862, "\"", 915, 3);
#line 34 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml"
WriteAttributeValue("", 868, cdnUrl, 868, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 875, "/js/dist/kendogrid.js?v=", 875, 24, true);
#line 34 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml"
WriteAttributeValue("", 899, resourceVersion, 899, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(916, 157, true);
                WriteLiteral("></script>\r\n\r\n    <script type=\"text/javascript\">\r\n        (function () {\r\n            window.farContractType.farContractTypeList(\'Far Contract Type List\', \'");
                EndContext();
                BeginContext(1074, 71, false);
#line 38 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarContractType\Index.cshtml"
                                                                             Write(User.Claims.Where(c=>c.Type.ToString() == "fullName").ToList()[0].Value);

#line default
#line hidden
                EndContext();
                BeginContext(1145, 34, true);
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
