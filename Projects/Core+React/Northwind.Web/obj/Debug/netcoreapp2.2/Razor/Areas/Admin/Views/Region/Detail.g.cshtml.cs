#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Region\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "320b250c74eb3b69ee23b309b5ae8644fae41ac9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Region_Detail), @"mvc.1.0.view", @"/Areas/Admin/Views/Region/Detail.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/Region/Detail.cshtml", typeof(AspNetCore.Areas_Admin_Views_Region_Detail))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"320b250c74eb3b69ee23b309b5ae8644fae41ac9", @"/Areas/Admin/Views/Region/Detail.cshtml")]
    public class Areas_Admin_Views_Region_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Region.RegionViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(63, 221, true);
            WriteLiteral("\r\n    <div class=\"col-md-12\">\r\n        <div class=\"form-group col-md-4\">\r\n            <label class=\"control-label control-label-read\">\r\n                Region Code\r\n            </label>\r\n            <h6>\r\n                ");
            EndContext();
            BeginContext(285, 16, false);
#line 9 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Region\Detail.cshtml"
           Write(Model.RegionCode);

#line default
#line hidden
            EndContext();
            BeginContext(301, 227, true);
            WriteLiteral("\r\n            </h6>\r\n        </div>\r\n        <div class=\"form-group col-md-4\">\r\n            <label class=\"control-label control-label-read\">\r\n                Region Name\r\n            </label>\r\n            <h6>\r\n                ");
            EndContext();
            BeginContext(529, 16, false);
#line 17 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Region\Detail.cshtml"
           Write(Model.RegionName);

#line default
#line hidden
            EndContext();
            BeginContext(545, 232, true);
            WriteLiteral("\r\n            </h6>\r\n        </div>\r\n        <div class=\"form-group col-md-4\">\r\n            <label class=\"control-label control-label-read\">\r\n                Regional Manager\r\n            </label>\r\n            <h6>\r\n                ");
            EndContext();
            BeginContext(779, 52, false);
#line 25 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Region\Detail.cshtml"
            Write(Model.ManagerName !=null ? @Model.ManagerName :"N/A");

#line default
#line hidden
            EndContext();
            BeginContext(832, 279, true);
            WriteLiteral(@"
            </h6>
        </div>
    </div>
    <div class=""col-12"">
        <div class=""form-group col-md-4"">
            <label class=""control-label control-label-read"">
                Deputy  Regional Manager
            </label>
            <h6>
                 ");
            EndContext();
            BeginContext(1113, 67, false);
#line 35 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Region\Detail.cshtml"
             Write(Model.DeputyManagerName != null  ? @Model.DeputyManagerName : "N/A");

#line default
#line hidden
            EndContext();
            BeginContext(1181, 250, true);
            WriteLiteral("\r\n            </h6>\r\n        </div>\r\n        <div class=\"form-group col-md-4\">\r\n            <label class=\"control-label control-label-read\">\r\n                Health and Safety Regional Manager\r\n            </label>\r\n            <h6>\r\n                ");
            EndContext();
            BeginContext(1433, 56, false);
#line 43 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Region\Detail.cshtml"
            Write(Model.HSManagerName !=null ? @Model.HSManagerName :"N/A");

#line default
#line hidden
            EndContext();
            BeginContext(1490, 244, true);
            WriteLiteral("\r\n            </h6>\r\n        </div>\r\n        <div class=\"form-group col-md-4\">\r\n            <label class=\"control-label control-label-read\">\r\n                Business Development Manager\r\n            </label>\r\n            <h6>\r\n                ");
            EndContext();
            BeginContext(1736, 56, false);
#line 51 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Region\Detail.cshtml"
            Write(Model.BDManagerName !=null ? @Model.BDManagerName :"N/A");

#line default
#line hidden
            EndContext();
            BeginContext(1793, 51, true);
            WriteLiteral("\r\n            </h6>\r\n        </div>\r\n    </div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Region.RegionViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
