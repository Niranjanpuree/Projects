#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7952f76385b0941ee10878cd0662981e68c8e611"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Notification_Index), @"mvc.1.0.view", @"/Views/Notification/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Notification/Index.cshtml", typeof(AspNetCore.Views_Notification_Index))]
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
#line 1 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7952f76385b0941ee10878cd0662981e68c8e611", @"/Views/Notification/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_Notification_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
  
    ViewData["Title"] = "Notification";
    Layout = "~/Views/Shared/_Layout.cshtml";

    string key = ViewBag.key.ToString();
    var parentControllerName = string.Empty;
    var controllerName = key.Split('.')[0];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(355, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("breadcrumb", async() => {
                BeginContext(383, 4, true);
                WriteLiteral("\r\n\r\n");
                EndContext();
#line 17 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
     if (!string.IsNullOrEmpty(ViewBag.parentRedirection))
    {

#line default
#line hidden
                BeginContext(503, 56, true);
                WriteLiteral("        <li class=\"breadcrumb-item\"><a href=\"/Contract\">");
                EndContext();
                BeginContext(560, 16, false);
#line 19 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
                                                   Write(ViewBag.cameFrom);

#line default
#line hidden
                EndContext();
                BeginContext(576, 49, true);
                WriteLiteral("</a></li>\r\n        <li class=\"breadcrumb-item\"><a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 625, "\"", 658, 1);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
WriteAttributeValue("", 632, ViewBag.parentRedirection, 632, 26, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(659, 1, true);
                WriteLiteral(">");
                EndContext();
                BeginContext(661, 28, false);
#line 20 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
                                                                    Write(ViewBag.parentContractNumber);

#line default
#line hidden
                EndContext();
                BeginContext(689, 67, true);
                WriteLiteral(" : Contract Detail</a></li>\r\n        <li class=\"breadcrumb-item\"><a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 756, "\"", 807, 4);
                WriteAttributeValue("", 763, "/", 763, 1, true);
#line 21 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
WriteAttributeValue("", 764, controllerName, 764, 15, false);

#line default
#line hidden
                WriteAttributeValue("", 779, "/Details/", 779, 9, true);
#line 21 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
WriteAttributeValue("", 788, ViewBag.resourceId, 788, 19, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(808, 1, true);
                WriteLiteral(">");
                EndContext();
                BeginContext(810, 20, false);
#line 21 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
                                                                                      Write(ViewBag.resourceName);

#line default
#line hidden
                EndContext();
                BeginContext(830, 31, true);
                WriteLiteral(" : Task Order Detail</a></li>\r\n");
                EndContext();
#line 22 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
    }
    else
    {

#line default
#line hidden
                BeginContext(885, 38, true);
                WriteLiteral("        <li class=\"breadcrumb-item\"><a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 923, "\"", 946, 2);
                WriteAttributeValue("", 930, "/", 930, 1, true);
#line 25 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
WriteAttributeValue("", 931, controllerName, 931, 15, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(947, 1, true);
                WriteLiteral(">");
                EndContext();
                BeginContext(949, 16, false);
#line 25 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
                                                          Write(ViewBag.cameFrom);

#line default
#line hidden
                EndContext();
                BeginContext(965, 49, true);
                WriteLiteral("</a></li>\r\n        <li class=\"breadcrumb-item\"><a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 1014, "\"", 1065, 4);
                WriteAttributeValue("", 1021, "/", 1021, 1, true);
#line 26 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
WriteAttributeValue("", 1022, controllerName, 1022, 15, false);

#line default
#line hidden
                WriteAttributeValue("", 1037, "/Details/", 1037, 9, true);
#line 26 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
WriteAttributeValue("", 1046, ViewBag.resourceId, 1046, 19, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1066, 1, true);
                WriteLiteral(">");
                EndContext();
                BeginContext(1068, 20, false);
#line 26 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
                                                                                      Write(ViewBag.resourceName);

#line default
#line hidden
                EndContext();
                BeginContext(1088, 29, true);
                WriteLiteral(" : Contract Detail</a></li>\r\n");
                EndContext();
#line 27 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
    }

#line default
#line hidden
                BeginContext(1124, 61, true);
                WriteLiteral("    <li class=\"breadcrumb-item\"><a href=\"#\">Notify</a></li>\r\n");
                EndContext();
            }
            );
            BeginContext(1188, 51, true);
            WriteLiteral("    <div class=\"alert alert-primary\">\r\n        The ");
            EndContext();
            BeginContext(1240, 27, false);
#line 31 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
       Write(ViewBag.resourceDisplayName);

#line default
#line hidden
            EndContext();
            BeginContext(1267, 538, true);
            WriteLiteral(@" been saved successfully.  Please identify the individuals or distribution list of who should be 
        notified of this action.  These individuals will be notified in the event of future Mods and/or Contract changes.  To create a 
        distribution list for use with future contracts, click the Create button and a new distribution list will be applied to this list 
        of individuals.  If you prefer to not notify other individuals, click on the Skip button.
    </div>

<div id=""distributionList"" class=""p-3""></div>

");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1823, 114, true);
                WriteLiteral("\r\n    <script type=\"text/javascript\">\r\n\r\n        function notifyCallBack() {\r\n            window.location.href = \"");
                EndContext();
                BeginContext(1938, 19, false);
#line 43 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
                               Write(ViewBag.redirectUrl);

#line default
#line hidden
                EndContext();
                BeginContext(1957, 86, true);
                WriteLiteral("\";\r\n        }\r\n        function skipCallBack() {\r\n            window.location.href = \"");
                EndContext();
                BeginContext(2044, 19, false);
#line 46 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
                               Write(ViewBag.redirectUrl);

#line default
#line hidden
                EndContext();
                BeginContext(2063, 143, true);
                WriteLiteral("\";\r\n        }\r\n        (function () {\r\n            window.distributionList.pageView.loadDistributionList(\'distributionList\',\r\n                \'");
                EndContext();
                BeginContext(2207, 11, false);
#line 50 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
            Write(ViewBag.key);

#line default
#line hidden
                EndContext();
                BeginContext(2218, 21, true);
                WriteLiteral("\',\r\n                \'");
                EndContext();
                BeginContext(2240, 18, false);
#line 51 "D:\ESS\ESS-Web\src\Northwind.Web\Views\Notification\Index.cshtml"
            Write(ViewBag.resourceId);

#line default
#line hidden
                EndContext();
                BeginContext(2258, 214, true);
                WriteLiteral("\',\r\n                true,\r\n                true,\r\n                true,\r\n                false,\r\n                true,\r\n                notifyCallBack,\r\n                skipCallBack);\r\n        })()\r\n    </script>\r\n");
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
