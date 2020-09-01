using System.Web.Optimization;

namespace AutoCare.Product.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new ScriptBundle("~/bundles/npmmodule").Include(
            //            "~/Scripts/lib/npmlibs/es6-shim.js",
            //            "~/Scripts/lib/npmlibs/system-polyfills.src.js",
            //            "~/Scripts/lib/npmlibs/angular2-polyfills.js",
            //            "~/Scripts/lib/npmlibs/system.src.js",
            //            "~/Scripts/lib/npmlibs/rx.js",
            //            "~/Scripts/lib/npmlibs/angular2.js",
            //            "~/Scripts/lib/npmlibs/router.dev.js",
            //            "~/Scripts/lib/npmlibs/http.dev.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
