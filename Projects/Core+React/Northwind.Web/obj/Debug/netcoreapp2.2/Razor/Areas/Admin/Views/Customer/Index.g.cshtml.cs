#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "07060bdd785df9497f6693062b5d429d32a544b2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Customer_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/Customer/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/Customer/Index.cshtml", typeof(AspNetCore.Areas_Admin_Views_Customer_Index))]
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
#line 2 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"07060bdd785df9497f6693062b5d429d32a544b2", @"/Areas/Admin/Views/Customer/Index.cshtml")]
    public class Areas_Admin_Views_Customer_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Customer.CustomerListViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/admin/Settings"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
  
    ViewData["Title"] = "Index";
    //Layout = "_Layout";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#line 9 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];

#line default
#line hidden
            DefineSection("breadcrumb", async() => {
                BeginContext(406, 34, true);
                WriteLiteral("\r\n    <li class=\"breadcrumb-item\">");
                EndContext();
                BeginContext(440, 39, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "07060bdd785df9497f6693062b5d429d32a544b23961", async() => {
                    BeginContext(467, 8, true);
                    WriteLiteral("Settings");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(479, 70, true);
                WriteLiteral("</li>\r\n    <li class=\"breadcrumb-item\"><a href=\"#\">Customer</a></li>\r\n");
                EndContext();
            }
            );
            BeginContext(552, 186, true);
            WriteLiteral("<div class=\"row\">\r\n    <div class=\"col-lg-6 col-md-8\">\r\n        <form id=\"customerSearchForm\" class=\"search-form-r\">\r\n            <div class=\"input-group mb-3 mb-sm-0\">\r\n                ");
            EndContext();
            BeginContext(739, 109, false);
#line 22 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
           Write(Html.TextBoxFor(model => model.SearchValue, new { @class = "form-control", placeholder = " Search By Name" }));

#line default
#line hidden
            EndContext();
            BeginContext(848, 1740, true);
            WriteLiteral(@"
                <div class=""input-group-append"">
                    <div class=""input-group-text"" id=""btnSearch"">
                        <i class=""k-icon k-i-search""></i>
                    </div>
                </div>
            </div>
        </form>

    </div>
    <div class=""col-lg-6 col-md-4 text-right"">
        <button type=""button"" class=""btn btn-primary"" id=""addNewCustomer"">Add Customer</button>
        <div class=""dropdown float-right ml-1"">
            <button type=""button"" class=""btn btn-secondary dropdown-toggle"" data-toggle=""dropdown"">More</button>
            <div class=""dropdown-menu  dropdown-menu-right"">
                <a id=""ExportToPdf"" rel=""#CustomerGrid"" data-resource=""Customer"" class=""dropdown-item"" href=""#"">
                    <i class=""k-icon k-i-file-pdf""></i> Export to Pdf
                </a>
                <a id=""ExportToCSV"" rel=""#CustomerGrid"" data-resource=""Customer"" class=""dropdown-item"" href=""#"">
                    <i class=""k-icon k-i-file-excel""");
            WriteLiteral(@"></i> Export to Excel
                </a>
                <a id=""DeleteCustomer"" class=""dropdown-item"" href=""#"">
                    <i class=""k-icon k-i-delete""></i>
                    Delete
                </a>
                <a id=""DisableCustomer"" class=""dropdown-item"" href=""#"">
                    <i class=""k-icon k-i-cancel""></i>
                    Disable
                </a>
                <a id=""EnableCustomer"" class=""dropdown-item"" href=""#"">
                    <i class=""k-icon k-i-check""></i>
                    Enable
                </a>
            </div>
        </div>
    </div>

    <div class=""col-12"">
        <div class=""search-pills-container"" id=""searchPills"">
");
            EndContext();
#line 61 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
             if (!string.IsNullOrEmpty(Model.SearchValue))
            {

#line default
#line hidden
            BeginContext(2663, 84, true);
            WriteLiteral("                <div class=\"badge badge-pill badge-secondary\">\r\n                    ");
            EndContext();
            BeginContext(2748, 17, false);
#line 64 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
               Write(Model.SearchValue);

#line default
#line hidden
            EndContext();
            BeginContext(2765, 135, true);
            WriteLiteral("\r\n                    <a href=\"#\" id=\"clearSearch\" class=\"pill-close\"><i class=\"material-icons\">close</i></a>\r\n                </div>\r\n");
            EndContext();
#line 67 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
            }

#line default
#line hidden
            BeginContext(2915, 265, true);
            WriteLiteral(@"        </div>
    </div>
</div>
<div id=""dialog"">
    <div class=""content""></div>
</div>
<div class=""row mt-3"">
    <div class=""col"">
        <div id=""CustomerGrid"" class=""table-grid table-more-btn"">
        </div>
    </div>
</div>
<!-- </div> -->

");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(3198, 13, true);
                WriteLiteral("\r\n    <script");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 3211, "\"", 3263, 3);
#line 83 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
WriteAttributeValue("", 3217, cdnUrl, 3217, 7, false);

#line default
#line hidden
                WriteAttributeValue("", 3224, "/js/proj/customer.js?v=", 3224, 23, true);
#line 83 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\Admin\Views\Customer\Index.cshtml"
WriteAttributeValue("", 3247, resourceVersion, 3247, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(3264, 14, true);
                WriteLiteral("></script>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Customer.CustomerListViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591