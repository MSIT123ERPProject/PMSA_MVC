using System.Web;
using System.Web.Optimization;

namespace layoutTest
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/fontawesome-all.min.css",
                      "~/Content/sbTemplate/sb-admin.css",
                      "~/Content/dataTables_css/dataTables.bootstrap4.css",
                      "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/allpluginjs").Include(
                        "~/Scripts/jquery.easing.min.js",
                        "~/Scripts/sbTemplate_js/sb-admin.js",
                        "~/Scripts/dataTables_js/dataTables.bootstrap4.js",
                        "~/Scripts/dataTables_js/jquery.dataTables.js"));
        }
    }
}