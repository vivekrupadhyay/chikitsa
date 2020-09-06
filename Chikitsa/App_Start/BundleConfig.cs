using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Chikitsa
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //, "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css", "https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css"

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/css/bootstrap.min.css", "~/Content/css/AdminLTE.min.css", "~/Content/css/_all-skins.min.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/css/bootstrap.min.css", "~/Content/css/toastr.min.css", "~/Content/css/dataTables.bootstrap.css", "~/Content/css/sweetalert.css", "~/Content/css/datepicker3.css", "~/Content/css/bootstrap-timepicker.min.css", "~/Content/css/daterangepicker.css", "~/Content/css/switchery.min.css", "~/Content/css/AdminLTE.min.css", "~/Content/css/_all-skins.min.css"));            
            //, "~/Scripts/jquery.dataTables.min.js", "~/Scripts/dataTables.bootstrap.min.js",
            bundles.Add(new ScriptBundle("~/Scripts").Include(
                      "~/Scripts/jquery-2.2.3.min.js","~/Scripts/bootstrap.min.js", "~/Scripts/jquery.slimscroll.min.js", "~/Scripts/fastclick.min.js", "~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.date.js", "~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/toastr.min.js", "~/Scripts/sweetalert.min.js", "~/Scripts/bootstrap-datepicker.js", "~/Scripts/bootstrap-timepicker.min.js", "~/Scripts/moment.min.js", "~/Scripts/daterangepicker.js", "~/Scripts/switchery.min.js", "~/Scripts/app.min.js", "~/Scripts/ems.js"));
        }
    }
}