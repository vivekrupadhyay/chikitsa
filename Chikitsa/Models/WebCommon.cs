using Chikitsa.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Chikitsa.Models
{
    public class WebCommon
    {
        public static ToastNotification SetToast(Response objResponse, string controller = "", string action = "", int duration = 2000)
        {
            ToastNotification objToast = new ToastNotification();
            if (objResponse.ErrorCode == "000")
            {
                objToast.toastType = ToastType.success;
                objToast.Message = "Saved Successfully";
                objToast.Controller = controller;
                objToast.Action = action;
                objToast.RedirectDuration = duration;
            }
            else if (objResponse.ErrorCode == "D000")
            {
                objToast.toastType = ToastType.info;
                objToast.Message = "Deleted Successfully";
            }
            else
            {
                objToast.toastType = ToastType.error;
                objToast.Message = "Error in saving due to " + objResponse.ErrorCode;//objResponse.Description;//
            }
            return objToast;
        }

        public static string Pager(QueryBO objQuery, PagingRoute pagingRoute)
        {
            StringBuilder sbPager = null;
            try
            {
                sbPager = new StringBuilder();
                int numberOfPages = (int)(objQuery.TotalRecords / objQuery.PageSize);
                sbPager.Append("<ul class='pagination'>");
                //sbPager.Append("");
                //sbPager.Append("");
                //sbPager.Append("");

                double dblPageCount = (double)((decimal)objQuery.TotalRecords / objQuery.PageSize);
                int pageCount = (int)Math.Ceiling(dblPageCount);
                //List<ListItem> pages = new List<ListItem>();
                if (pageCount > 0)
                {
                    sbPager.Append("<li class='paginate_button previous  " + (objQuery.PageNumber == 1 ? "disabled" : "") + "'><a href='@Html.Raw(" + GeneratePagingRoute(pagingRoute, "1") + ")'>Previous</a></li>");
                    //pages.Add(new ListItem("First", "1", currentPage > 1));
                    for (int i = 1; i <= pageCount; i++)
                    {
                        sbPager.Append("<li class='paginate_button " + (objQuery.PageNumber == i ? "disabled" : "") + "'><a href='" + GeneratePagingRoute(pagingRoute, i.ToString()) + "'>" + i.ToString() + "</a></li>");
                        //pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                    sbPager.Append("<li class='paginate_button next  " + (objQuery.PageNumber == pageCount ? "disabled" : "") + "'><a href='" + GeneratePagingRoute(pagingRoute, pageCount.ToString()) + "'>Last</a></li>");
                }
                sbPager.Append("</ul>");
                sbPager.Append("<script type='text/javascript;> $(document).ready(function(){ $('.pagination a').click(alert()); }); </script>");
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return sbPager.ToString();
        }

        private static string GeneratePagingRoute(PagingRoute PagingRoute, string id)
        {

            return "@Url.Action('" + PagingRoute.Action + "','" + PagingRoute.Controller + "' , new {Id = " + id + "})";
            //return "/" + PagingRoute.Controller + "/" + PagingRoute.Action + "?" + PagingRoute.ParameterName + "=" + id;

        }

        public static string DataTableToHtml(DataTable dt)
        {
            string html = "<table class='table table-bordered dt-responsive nowrap'>";
            //add header row
            html += "<thead>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<th>" + dt.Columns[i].ColumnName + "</th>";
            html += "</thead><tbody>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</tbody></table>";
            return html;
        }

        public static void ConvertDate(ref string strDate, bool isToDate)
        {
            try
            {
                DateTime objDt = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                strDate = objDt.Date.ToString("MM/dd/yyyy");
                if (isToDate)
                {
                    strDate += " 23:59:59";
                }
                else
                {
                    strDate += " 00:00:00";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            // return strDateTime;
        }

        public static TimeSpan StringToTimeSpan(string s)
        {
            DateTime t = DateTime.ParseExact(s, "h:mm tt", CultureInfo.InvariantCulture);
            return t.TimeOfDay;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static string Serialize(object o, string rootName)
        {
            XmlSerializer xs = new XmlSerializer(o.GetType()); //, new XmlRootAttribute(rootName)
            StringWriter sw = new StringWriter();
            XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true });
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            xs.Serialize(xmlWriter, o, ns);
            return Convert.ToString(sw);
        }

        public static List<T> Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            List<T> dezerializedList = null;

            using (StringReader stream = new StringReader(xml))
            {
                dezerializedList = (List<T>)serializer.Deserialize(stream);
            }
            return dezerializedList;
        }

        public static string ObjectToJson(object obj, string ignorePropertyNames = "")
        {
            if (!string.IsNullOrEmpty(ignorePropertyNames))
                return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new DynamicContractResolver(ignorePropertyNames) });
            else
                return JsonConvert.SerializeObject(obj);
        }

        public static List<T> JsonToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);// JavaScriptSerializer().Deserialize<List<T>>(json);
        }

        public static List<int> Years()
        {
            List<int> lstYears = new List<int>();
            lstYears.Add(2015);
            lstYears.Add(2016);
            lstYears.Add(2017);
            return lstYears;
        }
    }


    public class DynamicContractResolver : DefaultContractResolver
    {
        private readonly string _propertyNameToExclude;

        public DynamicContractResolver(string propertyNameToExclude)
        {
            _propertyNameToExclude = propertyNameToExclude;
        }   

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            // only serializer properties that are not named after the specified property.
            properties =
                properties.Where(p => string.Compare(p.PropertyName, _propertyNameToExclude, true) != 0).ToList();

            return properties;
        }
    }
}