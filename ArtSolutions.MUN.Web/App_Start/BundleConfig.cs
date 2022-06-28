using System.Web;
using System.Web.Optimization;

namespace ArtSolutions.MUN.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Vendor scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.1.min.js"));

            //Globalize scripts
            bundles.Add(new ScriptBundle("~/bundles/jqueryGlobalize").Include(
                        "~/Scripts/cldr.min.js",
                        "~/Scripts/cldr/event.min.js",
                        "~/Scripts/cldr/supplemental.min.js",
                        "~/Scripts/globalize.min.js",
                        "~/Scripts/globalize/number.min.js",
                        "~/Scripts/globalize/date.min.js"
            ));

            // jQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.validate.min.js",
            "~/Scripts/jquery.validate.unobtrusive.min.js",
            "~/Scripts/jquery.unobtrusive-ajax.min.js",
            "~/Scripts/jquery.date.validation.min.js",
             "~/Scripts/jquery.validate.globalize.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            // Inspinia script
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Scripts/app/inspinia.js",
                      "~/Scripts/app/custom.js"
                      ));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));

            // jQuery plugins
            bundles.Add(new ScriptBundle("~/plugins/metsiMenu").Include(
                      "~/Scripts/plugins/metisMenu/metisMenu.min.js"));

            bundles.Add(new ScriptBundle("~/plugins/pace").Include(
                      "~/Scripts/plugins/pace/pace.min.js"));

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/animate.css",
                      "~/Content/style.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransformWrapper()));
            // wizardSteps styles
            bundles.Add(new StyleBundle("~/plugins/wizardStepsStyles").Include(
                      "~/Content/plugins/steps/jquery.steps.min.css"));

            // wizardSteps 
            bundles.Add(new ScriptBundle("~/plugins/wizardSteps").Include(
                      "~/Scripts/plugins/steps/jquery.steps.min.js"));
            // dropZone styles
            bundles.Add(new StyleBundle("~/Content/plugins/dropzone/dropZoneStyles").Include(
                      "~/Content/plugins/dropzone/basic.min.css",
                      "~/Content/plugins/dropzone/dropzone.min.css"));

            // dropZone 
            bundles.Add(new ScriptBundle("~/plugins/dropZone").Include(
                      "~/Scripts/plugins/dropzone/dropzone.min.js"));

            // toastr notification 
            bundles.Add(new ScriptBundle("~/plugins/toastr").Include(
                      "~/Scripts/plugins/toastr/toastr.min.js"));

            // toastr notification styles
            bundles.Add(new StyleBundle("~/plugins/toastrStyles").Include(
                      "~/Content/plugins/toastr/toastr.min.css"));

            // iCheck css styles
            bundles.Add(new StyleBundle("~/Content/plugins/iCheck/iCheckStyles").Include(
                      "~/Content/plugins/iCheck/custom.min.css"));

            // iCheck
            bundles.Add(new ScriptBundle("~/plugins/iCheck").Include(
                      "~/Scripts/plugins/iCheck/icheck.min.js"));

            // Awesome bootstrap checkbox
            bundles.Add(new StyleBundle("~/plugins/awesomeCheckboxStyles").Include(
                      "~/Content/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.min.css"));

            // dataPicker styles
            bundles.Add(new StyleBundle("~/plugins/dataPickerStyles").Include(
                      "~/Content/plugins/datapicker/datepicker3.min.css"));

            // dataPicker 
            bundles.Add(new ScriptBundle("~/plugins/dataPicker").Include(
                      "~/Scripts/plugins/datapicker/bootstrap-datepicker.min.js",
                      "~/Scripts/plugins/datapicker/locales/bootstrap-datepicker.es.min.js",
                      "~/Scripts/plugins/datapicker/locales/bootstrap-datepicker.en-GB.min.js"));

            // dataTables css styles
            bundles.Add(new StyleBundle("~/Content/plugins/dataTables/dataTablesStyles").Include(
                      "~/Content/plugins/dataTables/datatables.min.css"));

            // dataTables 
            bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                      "~/Scripts/plugins/dataTables/datatables.min.js",
                      "~/Scripts/plugins/dataTables/dataTables.conditionalPaging.min.js",
                      "~/Scripts/plugins/dataTables/dataTables.responsive.min.js"
                      ));

            // dataTable Group
            bundles.Add(new ScriptBundle("~/plugins/dataTablesGroup").Include(
                      "~/Scripts/plugins/dataTables/dataTables.rowGroup.min.js"
                      ));

            // Footable Styless
            bundles.Add(new StyleBundle("~/plugins/footableStyles").Include(
                      "~/Content/plugins/footable/footable.core.min.css", new CssRewriteUrlTransformWrapper()));

            // Footable alert
            bundles.Add(new ScriptBundle("~/plugins/footable").Include(
                      "~/Scripts/plugins/footable/footable.all.min.js"));

            // Select2 Styless
            bundles.Add(new StyleBundle("~/plugins/select2Styles").Include(
                      "~/Content/plugins/select2/select2.min.css"));

            // Select2
            bundles.Add(new ScriptBundle("~/plugins/select2").Include(
                      "~/Scripts/plugins/select2/select2.full.min.js", "~/Scripts/jquery.validate.select2.js"));
            // Pe-icon-7-stroke
            bundles.Add(new StyleBundle("~/plugins/7strokeStyles").Include(
                      "~/Content/plugins/pe-icon-7-stroke/css/pe-icon-7-stroke.min.css", new CssRewriteUrlTransformWrapper()));

            // jasnyBootstrap styles
            bundles.Add(new StyleBundle("~/plugins/jasnyBootstrapStyles").Include(
                      "~/Content/plugins/jasny/jasny-bootstrap.min.css"));

            // jasnyBootstrap 
            bundles.Add(new ScriptBundle("~/plugins/jasnyBootstrap").Include(
                      "~/Scripts/plugins/jasny/jasny-bootstrap.min.js"));

            // Sweet alert Styless
            bundles.Add(new StyleBundle("~/plugins/sweetAlertStyles").Include(
                      "~/Content/plugins/sweetalert/sweetalert.min.css"));

            // Sweet alert
            bundles.Add(new ScriptBundle("~/plugins/sweetAlert").Include(
                      "~/Scripts/plugins/sweetalert/sweetalert.min.js"));

            // Culture
            bundles.Add(new ScriptBundle("~/Culture").Include(
                      "~/Scripts/Culture/jQuery.glob.min.js",
                      "~/Scripts/Culture/jQuery.glob.en-IN.min.js",
                      "~/Scripts/Culture/jQuery.glob.es-PR.min.js",
                      "~/Scripts/Culture/jQuery.glob.es-US.min.js",
                      "~/Scripts/Culture/jQuery.glob.hi-IN.min.js"));

            // jsTree Styless
            bundles.Add(new StyleBundle("~/plugins/jsTreeStyles").Include(
                      "~/Content/plugins/jsTree/style.min.css"));
            // jsTree
            bundles.Add(new ScriptBundle("~/plugins/jsTree").Include(
                      "~/Content/plugins/jsTree/jstree.min.js"));

            // ion.rangeSlider Styless
            bundles.Add(new StyleBundle("~/plugins/ionRangesliderStyles").Include(
                      "~/Content/plugins/ion-rangeslider/ion.rangeSlider.min.css",
                      "~/Content/plugins/ion-rangeslider/ion.rangeSlider.skinFlat.min.css"));
            // ion.rangeSlider
            bundles.Add(new ScriptBundle("~/plugins/ionRangeslider").Include(
                      "~/Scripts/plugins/ion-rangeslider/ion.rangeSlider.min.js"));

            // Flag Styles
            //bundles.Add(new ScriptBundle("~/plugins/flagiconStyle").Include(
            //          "~/Content/plugins/flag-icon/flag-icon.css", new CssRewriteUrlTransformWrapper()));

            // ChartJS chart
            bundles.Add(new ScriptBundle("~/plugins/chartJs").Include(
                      "~/Scripts/plugins/chartjs/Chart.min.js"));

            // Chat
            bundles.Add(new ScriptBundle("~/plugins/Chat").Include(
                      "~/Scripts/app/InitializeChat.js"));

            // Cookie
            //bundles.Add(new ScriptBundle("~/plugins/Cookie").Include(
            //          "~/Scripts/jquery.cookie-1.4.1.min.js"));

            // Dual ListBox Style
            bundles.Add(new StyleBundle("~/plugins/DualListBoxStyle").Include(
                     "~/Content/plugins/dualListbox/bootstrap-duallistbox.min.css"));

            // Dual ListBox
            bundles.Add(new ScriptBundle("~/plugins/DualListBox").Include(
                      "~/Scripts/plugins/dualListbox/jquery.bootstrap-duallistbox.min.js"));

            BundleTable.EnableOptimizations = true;

        }
    }
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}
