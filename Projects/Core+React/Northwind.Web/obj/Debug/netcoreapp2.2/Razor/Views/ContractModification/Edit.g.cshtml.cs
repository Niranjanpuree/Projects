#pragma checksum "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractModification\Edit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cf22fbfb7c5e008c6a4b7ae1a7b5e5592ba8d9d7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ContractModification_Edit), @"mvc.1.0.view", @"/Views/ContractModification/Edit.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/ContractModification/Edit.cshtml", typeof(AspNetCore.Views_ContractModification_Edit))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cf22fbfb7c5e008c6a4b7ae1a7b5e5592ba8d9d7", @"/Views/ContractModification/Edit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df9da4197f546b34ce64d5b35a2f420d4dbb640", @"/Views/_ViewImports.cshtml")]
    public class Views_ContractModification_Edit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Northwind.Web.Models.ViewModels.Contract.ContractModificationViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_FormModel", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(79, 11, true);
            WriteLiteral("<div>\r\n    ");
            EndContext();
            BeginContext(90, 29, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "cf22fbfb7c5e008c6a4b7ae1a7b5e5592ba8d9d73764", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(119, 160, true);
            WriteLiteral("\r\n</div>\r\n<script>\r\n     //call submit method from contract index.tsx\r\n    window.uploaderMod = window.loadFileUpload.pageView.loadFileUpload(\'fileUploadMod\', \'");
            EndContext();
            BeginContext(280, 19, false);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractModification\Edit.cshtml"
                                                                                    Write(ViewBag.Resourcekey);

#line default
#line hidden
            EndContext();
            BeginContext(299, 10, true);
            WriteLiteral("\', true, \'");
            EndContext();
            BeginContext(310, 18, false);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractModification\Edit.cshtml"
                                                                                                                  Write(ViewBag.ResourceId);

#line default
#line hidden
            EndContext();
            BeginContext(328, 4, true);
            WriteLiteral("\', \'");
            EndContext();
            BeginContext(333, 17, false);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractModification\Edit.cshtml"
                                                                                                                                         Write(ViewBag.UpdatedBy);

#line default
#line hidden
            EndContext();
            BeginContext(350, 4, true);
            WriteLiteral("\', \'");
            EndContext();
            BeginContext(355, 17, false);
#line 7 "D:\ESS\ESS-Web\src\Northwind.Web\Views\ContractModification\Edit.cshtml"
                                                                                                                                                               Write(ViewBag.UpdatedOn);

#line default
#line hidden
            EndContext();
            BeginContext(372, 743, true);
            WriteLiteral(@"', 'No path', true, false, true, true, false, submitCallBack);

    function submitCallBack(resourceId) {
        //call the notification component..
        // finally redirect to  notification page..

        window.loadDistributionListDialog.pageView.loadDistributionListDialog('distributionList',
            'ContractModification.Edit',
            resourceId,
            true,
            true,
            false,
            false,
            true,
            notifyCallBack,
            skipCallBack,
            true);

        //$(""#loading"").hide();
        $(""#loadingFileUpload"").hide();
    }
</script>
<script>
    function notifyCallBack() {

    }
    function skipCallBack() {

    }
</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Northwind.Web.Models.ViewModels.Contract.ContractModificationViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
