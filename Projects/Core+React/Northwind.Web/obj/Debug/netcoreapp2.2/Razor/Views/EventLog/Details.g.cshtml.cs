#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\EventLog\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "60d2e89c34b7bb5c2434aa664221bd3164fe9ca3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_EventLog_Details), @"mvc.1.0.view", @"/Views/EventLog/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/EventLog/Details.cshtml", typeof(AspNetCore.Views_EventLog_Details))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"60d2e89c34b7bb5c2434aa664221bd3164fe9ca3", @"/Views/EventLog/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_EventLog_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Core.AuditLog.Entities.EventLogs>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(51, 298, true);
            WriteLiteral(@"<table class=""table"">
    <thead>
        <tr>
            <th scope=""col"">Resource</th>
            <th scope=""col"">Action</th>
            <th scope=""col"">StackTrace</th>
            
          

        </tr>
    </thead>
    <tbody>

       
            <tr>
                <td>");
            EndContext();
            BeginContext(350, 14, false);
#line 17 "D:\ESS\ESS-Web\src\Northwind.Web\Views\EventLog\Details.cshtml"
               Write(Model.Resource);

#line default
#line hidden
            EndContext();
            BeginContext(364, 27, true);
            WriteLiteral("</td>\r\n                <td>");
            EndContext();
            BeginContext(392, 12, false);
#line 18 "D:\ESS\ESS-Web\src\Northwind.Web\Views\EventLog\Details.cshtml"
               Write(Model.Action);

#line default
#line hidden
            EndContext();
            BeginContext(404, 27, true);
            WriteLiteral("</td>\r\n                <td>");
            EndContext();
            BeginContext(432, 16, false);
#line 19 "D:\ESS\ESS-Web\src\Northwind.Web\Views\EventLog\Details.cshtml"
               Write(Model.StackTrace);

#line default
#line hidden
            EndContext();
            BeginContext(448, 91, true);
            WriteLiteral("</td>\r\n               \r\n            </tr>\r\n        \r\n\r\n    </tbody>\r\n</table>\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Core.AuditLog.Entities.EventLogs> Html { get; private set; }
    }
}
#pragma warning restore 1591
