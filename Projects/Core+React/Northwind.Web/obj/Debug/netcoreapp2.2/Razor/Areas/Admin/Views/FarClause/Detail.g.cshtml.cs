#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "56da107bc324e7776b9ea94375de0b8008e67d5a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_FarClause_Detail), @"mvc.1.0.view", @"/Areas/Admin/Views/FarClause/Detail.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/FarClause/Detail.cshtml", typeof(AspNetCore.Areas_Admin_Views_FarClause_Detail))]
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
#line 2 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"56da107bc324e7776b9ea94375de0b8008e67d5a", @"/Areas/Admin/Views/FarClause/Detail.cshtml")]
    public class Areas_Admin_Views_FarClause_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.FarClause.FarClauseViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            BeginContext(263, 44, true);
            WriteLiteral("<div class=\"row\">\r\n    <div class=\"col-md-6\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 307, "\"", 325, 1);
#line 9 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
WriteAttributeValue("", 312, Model.Number, 312, 13, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(326, 160, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Number\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(487, 12, false);
#line 15 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
           Write(Model.Number);

#line default
#line hidden
            EndContext();
            BeginContext(499, 74, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 573, "\"", 590, 1);
#line 19 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
WriteAttributeValue("", 578, Model.Title, 578, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(591, 159, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Title\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(751, 11, false);
#line 25 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
           Write(Model.Title);

#line default
#line hidden
            EndContext();
            BeginContext(762, 74, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 836, "\"", 857, 1);
#line 29 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
WriteAttributeValue("", 841, Model.Paragraph, 841, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(858, 163, true);
            WriteLiteral(">\r\n        <div class=\"form-group\">\r\n            <label class=\"control-label\">\r\n                Paragraph\r\n            </label>\r\n            <h5>\r\n                ");
            EndContext();
            BeginContext(1022, 15, false);
#line 35 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
           Write(Model.Paragraph);

#line default
#line hidden
            EndContext();
            BeginContext(1037, 203, true);
            WriteLiteral("\r\n            </h5>\r\n        </div>\r\n    </div>\r\n</div>\r\n<div id=\"farContractTypeList\" className=\"far-contract-detail\"></div>\r\n<script>\r\n    window.loadFarClause.loadFarClauseList(\"farContractTypeList\",\'");
            EndContext();
            BeginContext(1241, 19, false);
#line 42 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\FarClause\Detail.cshtml"
                                                             Write(Model.FarClauseGuid);

#line default
#line hidden
            EndContext();
            BeginContext(1260, 23, true);
            WriteLiteral("\',\'detail\');\r\n</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.FarClause.FarClauseViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
