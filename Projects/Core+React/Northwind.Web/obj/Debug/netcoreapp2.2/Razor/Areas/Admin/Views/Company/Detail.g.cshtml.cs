#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Company\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c0f7c9e49bfd835f28bc6f6c6a2f04a7b4661d35"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Company_Detail), @"mvc.1.0.view", @"/Areas/Admin/Views/Company/Detail.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/Company/Detail.cshtml", typeof(AspNetCore.Areas_Admin_Views_Company_Detail))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c0f7c9e49bfd835f28bc6f6c6a2f04a7b4661d35", @"/Areas/Admin/Views/Company/Detail.cshtml")]
    public class Areas_Admin_Views_Company_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Company.CompanyViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(65, 160, true);
            WriteLiteral("<div class=\"\">\r\n    <div class=\"form-group\">\r\n        <div class=\"fontbold control-label\">\r\n            Company Code\r\n        </div>\r\n        <h5>\r\n            ");
            EndContext();
            BeginContext(226, 17, false);
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Company\Detail.cshtml"
       Write(Model.CompanyCode);

#line default
#line hidden
            EndContext();
            BeginContext(243, 175, true);
            WriteLiteral("\r\n        </h5>\r\n    </div>\r\n\r\n    <div class=\"form-group\">\r\n        <div class=\"fontbold control-label\">\r\n            Company Name\r\n        </div>\r\n        <h5>\r\n            ");
            EndContext();
            BeginContext(419, 17, false);
#line 17 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Company\Detail.cshtml"
       Write(Model.CompanyName);

#line default
#line hidden
            EndContext();
            BeginContext(436, 172, true);
            WriteLiteral("\r\n        </h5>\r\n    </div>\r\n\r\n    <div class=\"form-group\">\r\n        <div class=\"fontbold control-label\">\r\n            President\r\n        </div>\r\n        <h5>\r\n            ");
            EndContext();
            BeginContext(609, 19, false);
#line 26 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Company\Detail.cshtml"
       Write(Model.PresidentName);

#line default
#line hidden
            EndContext();
            BeginContext(628, 175, true);
            WriteLiteral("\r\n        </h5>\r\n    </div>\r\n\r\n    <div class=\"form-group\">\r\n        <div class=\"fontbold control-label\">\r\n            Abbreviation\r\n        </div>\r\n        <h5>\r\n            ");
            EndContext();
            BeginContext(804, 18, false);
#line 35 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Company\Detail.cshtml"
       Write(Model.Abbreviation);

#line default
#line hidden
            EndContext();
            BeginContext(822, 174, true);
            WriteLiteral("\r\n        </h5>\r\n    </div>\r\n\r\n    <div class=\"form-group\">\r\n        <div class=\"fontbold control-label\">\r\n            Description\r\n        </div>\r\n        <h5>\r\n            ");
            EndContext();
            BeginContext(997, 17, false);
#line 44 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Company\Detail.cshtml"
       Write(Model.Description);

#line default
#line hidden
            EndContext();
            BeginContext(1014, 35, true);
            WriteLiteral("\r\n        </h5>\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Company.CompanyViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591