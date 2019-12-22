using System.Web;
using System.Web.Optimization;

namespace ProductTrackingSystem
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-3.4.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/index").Include(
                "~/Content/js/index.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/home.css"));

            bundles.Add(new StyleBundle("~/Content/paged").Include("~/Content/css/PagedList.css"));

            bundles.Add(new StyleBundle("~/Content/toastr").Include("~/Content/css/toastr.css"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                "~/Content/js/toastr.js"));
        }
    }
}