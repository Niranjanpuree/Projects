#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\User\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6a087b306b1d463603beb8f5216f336fda930e00"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_IAM_Views_User_Details), @"mvc.1.0.view", @"/Areas/IAM/Views/User/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/IAM/Views/User/Details.cshtml", typeof(AspNetCore.Areas_IAM_Views_User_Details))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6a087b306b1d463603beb8f5216f336fda930e00", @"/Areas/IAM/Views/User/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0016b6767730c936e45b5440b0aea7bd14cb65d3", @"/Areas/IAM/Views/_ViewImports.cshtml")]
    public class Areas_IAM_Views_User_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Areas.IAM.Models.IAM.ViewModels.UserEditViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(787, 176, true);
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div id=\"usertabstrip\" class=\"col-md-12\">\r\n\r\n    </div>\r\n</div>\r\n<script>\r\n   \r\n\r\n    function init() {\r\n        window.iam.usergroup.loadUserDetails(\'");
            EndContext();
            BeginContext(964, 14, false);
#line 33 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\User\Details.cshtml"
                                         Write(Model.UserGuid);

#line default
#line hidden
            EndContext();
            BeginContext(978, 19, true);
            WriteLiteral("\', \'usertabstrip\',\'");
            EndContext();
            BeginContext(998, 17, false);
#line 33 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\User\Details.cshtml"
                                                                           Write(Model.DisplayName);

#line default
#line hidden
            EndContext();
            BeginContext(1015, 36, true);
            WriteLiteral("\');\r\n    }\r\n    init();\r\n</script>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Areas.IAM.Models.IAM.ViewModels.UserEditViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
