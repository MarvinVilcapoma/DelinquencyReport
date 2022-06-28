using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace ArtSolutions.MUN.Web
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null, string id = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            string currentID = string.Empty;
            bool isWorkFlow = false;
            if (!string.IsNullOrWhiteSpace(id))
            {
                currentID = (string)html.ViewContext.RouteData.Values["id"];
                if (!string.IsNullOrWhiteSpace(currentID) && !string.IsNullOrWhiteSpace(id))
                {
                    isWorkFlow = (id.Contains(",") ? id.Split(",".ToCharArray()).Any(a => a.ToLower().Trim() == currentID.ToLower().Trim()) :
                                    currentID == id) ? true : false;
                }
                else
                {
                    currentID = html.ViewContext.HttpContext.Request.QueryString["id"];
                    if (string.IsNullOrWhiteSpace(currentID))
                    {
                        currentID = html.ViewContext.HttpContext.Request.QueryString["workflowid"];
                    }
                    if (!string.IsNullOrWhiteSpace(currentID))
                        isWorkFlow = (id.Contains(",") ? id.Split(",".ToCharArray()).Any(a => a.ToLower().Trim() == currentID.ToLower().Trim()) :
                                        currentID == id) ? true : false;
                }
            }

            var tempString = (((controller.Contains(",") ? controller.Split(',').Any(x => x.ToLower().Trim() == currentController.ToLower())
                        : controller.ToLower() == currentController.ToLower())
                        &&
                        (action.Contains(",") ? action.Split(',').Any(x => x.ToLower().Trim() == currentAction.ToLower())
                        : action.ToLower() == currentAction.ToLower()))
                        &&
                       (controller.ToLower().Equals("case") && action.ToLower().Equals("list,view,printletter") && !string.IsNullOrWhiteSpace(id) ? isWorkFlow : true)) ?
               cssClass : String.Empty;

            return tempString;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        /// <summary>
        /// Create list of select list item based on supplied class
        /// </summary>        
        /// <param name="lstClass">List of specific class from which we want to create select list item</param>   
        /// <param name="strID">string strID which we want to use as ID of select list item</param>   
        /// <param name="strValue">string strValue which we want to use as VALUE of select list item</param>   
        /// <param name="iSelectedID">integer iSelectedID which is used to make select list item selected if supplied</param>   
        /// <returns>List of Select list item</returns>
        public static List<SelectListItem> CreateSelectList<T>(List<T> lstClass, string strID, string strValue, object objSelectedID = null, bool isSetDefaultText = false, bool showOptionLabel = false, string DefaultText = "ALL", bool setDefaultIfOnlyOneItem = false)
        {
            List<SelectListItem> lstSelectList = new List<SelectListItem>();

            object Value, ID;

            foreach (var item in lstClass)
            {
                var type = item.GetType();
                Value = type.GetProperty(strValue).GetValue(item);
                ID = type.GetProperty(strID).GetValue(item);
                if (setDefaultIfOnlyOneItem && lstClass.Count == 1 && objSelectedID == null)
                    objSelectedID = ID;
                lstSelectList.Add(new SelectListItem()
                {
                    Text = Convert.ToString(Value),
                    Value = Convert.ToString(ID),
                    Selected = (objSelectedID != null ? (ID.Equals(objSelectedID)) : false)
                });
            }

            if (showOptionLabel)
            {
                if (isSetDefaultText) lstSelectList.Insert(0, new SelectListItem { Text = DefaultText, Value = "0" });
                else lstSelectList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            }

            return lstSelectList;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            DataTable table = new DataTable();

            //// get properties of T
            var binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
            var options = PropertyReflectionOptions.IgnoreEnumerable | PropertyReflectionOptions.IgnoreIndexer;

            var properties = ReflectionExtensions.GetProperties<T>(binding, options).ToList();

            //// create table schema based on properties
            foreach (var property in properties)
            {
                //table.Columns.Add(property.Name, property.PropertyType);
                table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            //// create table data from T instances
            object[] values = new object[properties.Count];

            foreach (T item in source)
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }

                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary>
        /// Convert Partial View Result Into String
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="partialViewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string RenderPartialToString(Controller controller, string partialViewName, object model)
        {
            ViewEngineResult result = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialViewName);
            if (result.View != null)
            {
                controller.ViewData.Model = model;
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter output = new HtmlTextWriter(sw))
                    {
                        ViewContext viewContext = new ViewContext(controller.ControllerContext, result.View, controller.ViewData, controller.TempData, output);
                        result.View.Render(viewContext, output);
                    }
                }
                return sb.ToString();
            }
            return String.Empty;
        }
    }

    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets properties of T
        /// </summary>
        public static IEnumerable<PropertyInfo> GetProperties<T>(BindingFlags binding, PropertyReflectionOptions options = PropertyReflectionOptions.All)
        {
            var properties = typeof(T).GetProperties(binding);

            bool all = (options & PropertyReflectionOptions.All) != 0;
            bool ignoreIndexer = (options & PropertyReflectionOptions.IgnoreIndexer) != 0;
            bool ignoreEnumerable = (options & PropertyReflectionOptions.IgnoreEnumerable) != 0;

            foreach (var property in properties)
            {
                if (!all)
                {
                    if (ignoreIndexer && IsIndexer(property))
                    {
                        continue;
                    }

                    if (ignoreIndexer && !property.PropertyType.Equals(typeof(string)) && IsEnumerable(property))
                    {
                        continue;
                    }
                }

                yield return property;
            }
        }

        /// <summary>
        /// Check if property is indexer
        /// </summary>
        private static bool IsIndexer(PropertyInfo property)
        {
            var parameters = property.GetIndexParameters();

            if (parameters != null && parameters.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if property implements IEnumerable
        /// </summary>
        private static bool IsEnumerable(PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces().Any(x => x.Equals(typeof(System.Collections.IEnumerable)));
        }
    }

    [Flags]
    public enum PropertyReflectionOptions : int
    {
        /// <summary>
        /// Take all.
        /// </summary>
        All = 0,

        /// <summary>
        /// Ignores indexer properties.
        /// </summary>
        IgnoreIndexer = 1,

        /// <summary>
        /// Ignores all other IEnumerable properties
        /// except strings.
        /// </summary>
        IgnoreEnumerable = 2
    }
}