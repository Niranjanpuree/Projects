#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "02231c657e0fd3758a471507e4a3054eb63bb185"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_IAM_Views_Group_DeleteUserBatch), @"mvc.1.0.view", @"/Areas/IAM/Views/Group/DeleteUserBatch.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/IAM/Views/Group/DeleteUserBatch.cshtml", typeof(AspNetCore.Areas_IAM_Views_Group_DeleteUserBatch))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"02231c657e0fd3758a471507e4a3054eb63bb185", @"/Areas/IAM/Views/Group/DeleteUserBatch.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0016b6767730c936e45b5440b0aea7bd14cb65d3", @"/Areas/IAM/Views/_ViewImports.cshtml")]
    public class Areas_IAM_Views_Group_DeleteUserBatch : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Northwind.Web.Areas.IAM.Models.IAM.ViewModels.UserGroupViewModel>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onsubmit", new global::Microsoft.AspNetCore.Html.HtmlString("return false;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(86, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(88, 1271, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "02231c657e0fd3758a471507e4a3054eb63bb1853886", async() => {
                BeginContext(133, 2, true);
                WriteLiteral("\r\n");
                EndContext();
#line 4 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
     foreach (var item in Model)
    {
        if (item.GroupGuid != null)
        {
        

#line default
#line hidden
                BeginContext(233, 39, false);
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
   Write(Html.HiddenFor(model => item.GroupGuid));

#line default
#line hidden
                EndContext();
#line 8 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
                                                
        }
    }

#line default
#line hidden
                BeginContext(292, 106, true);
                WriteLiteral("\r\n    <table class=\"table\">\r\n        <thead>\r\n            <tr>\r\n                <th>\r\n                    ");
                EndContext();
                BeginContext(399, 44, false);
#line 16 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
               Write(Html.DisplayNameFor(model => model.UserName));

#line default
#line hidden
                EndContext();
                BeginContext(443, 67, true);
                WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
                EndContext();
                BeginContext(511, 45, false);
#line 19 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
               Write(Html.DisplayNameFor(model => model.FirstName));

#line default
#line hidden
                EndContext();
                BeginContext(556, 67, true);
                WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
                EndContext();
                BeginContext(624, 44, false);
#line 22 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
               Write(Html.DisplayNameFor(model => model.LastName));

#line default
#line hidden
                EndContext();
                BeginContext(668, 96, true);
                WriteLiteral("\r\n                </th>\r\n               \r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n");
                EndContext();
#line 28 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
                BeginContext(821, 60, true);
                WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
                EndContext();
                BeginContext(882, 43, false);
#line 32 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
               Write(Html.DisplayFor(modelItem => item.UserName));

#line default
#line hidden
                EndContext();
                BeginContext(925, 67, true);
                WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
                EndContext();
                BeginContext(993, 44, false);
#line 35 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
               Write(Html.DisplayFor(modelItem => item.FirstName));

#line default
#line hidden
                EndContext();
                BeginContext(1037, 67, true);
                WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
                EndContext();
                BeginContext(1105, 43, false);
#line 38 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
               Write(Html.DisplayFor(modelItem => item.LastName));

#line default
#line hidden
                EndContext();
                BeginContext(1148, 67, true);
                WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
                EndContext();
                BeginContext(1216, 43, false);
#line 41 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
               Write(Html.HiddenFor(modelItem => item.GroupGuid));

#line default
#line hidden
                EndContext();
                BeginContext(1259, 44, true);
                WriteLiteral("\r\n                </td>\r\n            </tr>\r\n");
                EndContext();
#line 44 "D:\ESS\ESS-Web\src\Northwind.Web\Areas\IAM\Views\Group\DeleteUserBatch.cshtml"
            }

#line default
#line hidden
                BeginContext(1318, 34, true);
                WriteLiteral("        </tbody>\r\n    </table>\r\n\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1359, 2, true);
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Northwind.Web.Areas.IAM.Models.IAM.ViewModels.UserGroupViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
