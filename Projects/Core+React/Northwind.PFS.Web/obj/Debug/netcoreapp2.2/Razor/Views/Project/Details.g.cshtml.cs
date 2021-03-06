#pragma checksum "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f7c8724ff10de07a297873b18a94e8032d52e53a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Project_Details), @"mvc.1.0.view", @"/Views/Project/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Project/Details.cshtml", typeof(AspNetCore.Views_Project_Details))]
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
#line 6 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f7c8724ff10de07a297873b18a94e8032d52e53a", @"/Views/Project/Details.cshtml")]
    public class Views_Project_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.PFS.Web.Models.ViewModels.ProjectViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/project"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(61, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 8 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
  
    var resourceVersion = @Configuration["resourceVersion"];
    var cdnUrl = @Configuration["CDNUrl"];
    ViewData["Title"] = "Project Details";
    Layout = "_Layout";
    ViewData["class"] = "project-details";

#line default
#line hidden
            BeginContext(496, 91, true);
            WriteLiteral("<style>\r\n    #projectDetails .k-grouping-row {\r\n        display: none;\r\n    }\r\n</style>\r\n\r\n");
            EndContext();
            DefineSection("breadcrumb", async() => {
                BeginContext(621, 107, true);
                WriteLiteral("\r\n        <li class=\"breadcrumb-item\"><a href=\"#\">Operations</a></li>\r\n        <li class=\"breadcrumb-item\">");
                EndContext();
                BeginContext(728, 46, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f7c8724ff10de07a297873b18a94e8032d52e53a4128", async() => {
                    BeginContext(748, 22, true);
                    WriteLiteral("Project Financial Tool");
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
                BeginContext(774, 55, true);
                WriteLiteral("</li>\r\n        <li class=\"breadcrumb-item\"><a href=\"#\">");
                EndContext();
                BeginContext(830, 20, false);
#line 25 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                                           Write(Model.ContractNumber);

#line default
#line hidden
                EndContext();
                BeginContext(850, 33, true);
                WriteLiteral(" : Project Details</a></li>\r\n    ");
                EndContext();
            }
            );
            BeginContext(886, 305, true);
            WriteLiteral(@"<style>
    .k-dropdown-wrap .k-searchbar{
        border: 1px solid #ddd;
    }
</style>

<div class=""row top-header"">
    <div class=""col-12"">
        <div class=""row pb-2 align-items-center"">
            <div class=""col-md-8"">
                <h3 class=""contract-title"">
                    ");
            EndContext();
            BeginContext(1192, 17, false);
#line 38 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
               Write(Model.Description);

#line default
#line hidden
            EndContext();
            BeginContext(1209, 422, true);
            WriteLiteral(@"
                    <i class=""k-icon k-i-info"" data-toggle=""collapse"" href=""#projects-details-info""
                        aria-expanded=""false"" aria-controls=""projects-details-info""></i>
                </h3>
            </div>
        </div>
        <div class=""row contract-info collapse mt-2"" id=""projects-details-info"">
            <div class=""col-md-12"">
                <p class=""mb-3 pb-1 border-bottom"">");
            EndContext();
            BeginContext(1632, 17, false);
#line 46 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                                              Write(Model.Description);

#line default
#line hidden
            EndContext();
            BeginContext(1649, 175, true);
            WriteLiteral("</p>\r\n            </div>\r\n\r\n            <div class=\"col-md-6\">\r\n                <ul class=\"list-inline mb-0 p-0\">\r\n                    <li>\r\n                        Name : <b>");
            EndContext();
            BeginContext(1825, 17, false);
#line 52 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                             Write(Model.ProjectName);

#line default
#line hidden
            EndContext();
            BeginContext(1842, 103, true);
            WriteLiteral("</b>\r\n                    </li>\r\n                    <li>\r\n                        Project Number : <b>");
            EndContext();
            BeginContext(1946, 19, false);
#line 55 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                                       Write(Model.ProjectNumber);

#line default
#line hidden
            EndContext();
            BeginContext(1965, 104, true);
            WriteLiteral("</b>\r\n                    </li>\r\n                    <li>\r\n                        Contract Number : <b>");
            EndContext();
            BeginContext(2070, 20, false);
#line 58 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                                        Write(Model.ContractNumber);

#line default
#line hidden
            EndContext();
            BeginContext(2090, 325, true);
            WriteLiteral(@"</b>
                    </li>

                </ul>
            </div>
            <div class=""col-md-6 text-right"">
                <ul class=""list-inline mb-0"">
                    <li>
                        <div class=""position-r"">
                            Project Manager : <span><b class=""tooltipdetail"">");
            EndContext();
            BeginContext(2416, 20, false);
#line 67 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                                                                        Write(Model.ProjectManager);

#line default
#line hidden
            EndContext();
            BeginContext(2436, 1086, true);
            WriteLiteral(@"</b></span>
                            <span class=""popover-detail bottom right"">
                                <span class='popover-detail-container'>
                                    <span>
                                        <label>Name</label> Person Name
                                    </span>
                                    <span><label>Company </label> Company Name</span>
                                    <span><label>Title </label>Job Title</span>
                                    <span>
                                        <label>Email </label><a
                                            href='mailto:""workmail@mail.com""'
                                            target='_blank'>workmail</a>
                                    </span>
                                    <span><label>Phone </label>98xxxxxxxxx</span>
                                </span>
                            </span>
                        </div>
                    </li>
         ");
            WriteLiteral("           <li>\r\n                        Project Control : <b>");
            EndContext();
            BeginContext(3523, 20, false);
#line 86 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                                        Write(Model.ProjectControl);

#line default
#line hidden
            EndContext();
            BeginContext(3543, 95, true);
            WriteLiteral("</b>\r\n                    </li>\r\n                    <li>\r\n                        Org Id : <b>");
            EndContext();
            BeginContext(3639, 11, false);
#line 89 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                               Write(Model.OrgId);

#line default
#line hidden
            EndContext();
            BeginContext(3650, 205, true);
            WriteLiteral("</b>\r\n                    </li>\r\n                </ul>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<div class=\"row\">\r\n    <div id=\"projectDetails\" class=\"col-12\">\r\n\r\n    </div>\r\n</div>\r\n\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(3877, 118, true);
                WriteLiteral("  \r\n        <script type=\"text/javascript\">\r\n        (function () {\r\n            window.pfs.showProjectDetailsScreen(\'");
                EndContext();
                BeginContext(3996, 19, false);
#line 107 "D:\ESS\ESS-Web\src\Northwind.PFS.Web\Views\Project\Details.cshtml"
                                            Write(Model.ProjectNumber);

#line default
#line hidden
                EndContext();
                BeginContext(4015, 49, true);
                WriteLiteral("\', \"USD\");\r\n        })()\r\n        </script>\r\n    ");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.PFS.Web.Models.ViewModels.ProjectViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
