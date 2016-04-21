using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AJstatistics.Models;
using System.Web.Script.Serialization;
using System.Text;
using System.Web.Http.Cors;

namespace AJstatistics.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StatisticsController : ApiController
    {
        IStatisticsRepository s = new StatisticsRepository();
        public HttpResponseMessage GetBarData(string tableName, string colName, string zoneName, string type,int year)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(s.GetBarData(tableName, colName, zoneName, type, year));
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        public HttpResponseMessage GetRadarData(string tableName, string colName, string zoneName, string type,int year)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(s.GetRadarData(tableName, colName, zoneName, type, year));
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}
