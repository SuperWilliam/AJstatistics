using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;

namespace AJstatistics.Models
{
    public class StatisticsRepository : IStatisticsRepository
    {
        public Bar GetBarData(string tableName, string colName, string zoneName, string type,int year)
        {
            string[] colNameArr = colName.Split(',');

            Bar bar = new Bar();
            List<string> legend = new List<string>();
            List<string> xAxis = new List<string>();
            List<Series> series = new List<Series>();

            string connStr = System.Configuration.ConfigurationManager.AppSettings["conn"];
            OracleConnection oleConn = new OracleConnection(connStr);
            oleConn.Open();
            for (int i = 0; i < colNameArr.Length; i++)
            {
                /*获取要查询字段的comment，并添加到legend*/
                OracleCommand commComments = oleConn.CreateCommand();
                commComments.CommandText = "select COMMENTS from user_col_comments where table_name = '" + tableName + "' and column_name = '" + colNameArr[i] + "'";
                OracleDataReader readComments = commComments.ExecuteReader();
                if (readComments.Read())
                {
                    legend.Add(readComments.GetString(0));
                    Series tempSeries = new Series();
                    tempSeries.name = readComments.GetString(0);
                    tempSeries.type = type;
                    tempSeries.data = new List<float>();
                    series.Add(tempSeries);
                }
                else
                {
                    legend.Add(null);
                    Series tempSeries = new Series();
                    tempSeries.name = null;
                    tempSeries.type = type;
                    tempSeries.data = new List<float>();
                    series.Add(tempSeries);
                }
                readComments.Close();
            }
            OracleCommand commMain = oleConn.CreateCommand();
            if (zoneName == "安吉县")
            {
                commMain.CommandText = "select " + colName + ",TOWNNAME from " + tableName + " where length(XZCODE)=9 and YEAR="+year;
                OracleDataReader readMain = commMain.ExecuteReader();
                while (readMain.Read())
                {
                    xAxis.Add(readMain.GetString(colNameArr.Length));
                    for (int i = 0; i < colNameArr.Length; i++)
                    {
                        series[i].data.Add(readMain.GetFloat(i));
                    }
                }
                readMain.Close();
            }
            else
            {
                commMain.CommandText = "select " + colName + ",VILLAGENAME from " + tableName + " where length(XZCODE)>9 and TOWNNAME='" + zoneName + "' and YEAR=" + year;
                OracleDataReader readMain = commMain.ExecuteReader();
                while (readMain.Read())
                {
                    xAxis.Add(readMain.GetString(colNameArr.Length));
                    for (int i = 0; i < colNameArr.Length; i++)
                    {
                        series[i].data.Add(readMain.GetFloat(i));
                    }
                }
                readMain.Close();
            }
            bar.legend = legend;
            bar.xAxis = xAxis;
            bar.series = series;
            oleConn.Close();
            return bar;
        }

        public Radar GetRadarData(string tableName, string colName, string zoneName, string type, int year)
        {
            string[] colNameArr = colName.Split(',');
            string[] zoneNameArr = zoneName.Split(',');

            Radar radar = new Radar();
            List<string> legend = new List<string>();
            List<IndicatorItem> indicator = new List<IndicatorItem>();
            List<DataItem> data = new List<DataItem>();

            string connStr = System.Configuration.ConfigurationManager.AppSettings["conn"];
            OracleConnection oleConn = new OracleConnection(connStr);
            oleConn.Open();
            for (int i = 0; i < colNameArr.Length; i++)
            {
                /*获取要查询字段的comment*/
                OracleCommand commComments = oleConn.CreateCommand();
                commComments.CommandText = "select COMMENTS from user_col_comments where table_name = '" + tableName + "' and column_name = '" + colNameArr[i] + "'";
                OracleDataReader readComments = commComments.ExecuteReader();
                if (readComments.Read())
                {
                    IndicatorItem tempIndicator = new IndicatorItem();
                    tempIndicator.text = readComments.GetString(0);
                    indicator.Add(tempIndicator);
                }
                else
                {
                    IndicatorItem tempIndicator = new IndicatorItem();
                    tempIndicator.text = null;
                    indicator.Add(tempIndicator);
                }
                readComments.Close();
                /*获取要查询字段的最大值*/
                OracleCommand commMax = oleConn.CreateCommand();
                commMax.CommandText = "select MAX(" + colNameArr[i] + ") from " + tableName + " where length(XZCODE)>9 and YEAR=" + year;
                OracleDataReader readMax = commMax.ExecuteReader();
                if (readMax.Read())
                {
                    indicator[i].max = readMax.GetFloat(0);
                }
                else
                {
                    indicator[i].max = 0;
                }
                readMax.Close();
            }
            for (int i = 0; i < zoneNameArr.Length; i++)
            {
                /*获取村名加入legend*/
                legend.Add(zoneNameArr[i]);
                DataItem tempData = new DataItem();
                tempData.name = zoneNameArr[i];
                tempData.value = new List<float>();
                /*获取各村的要查询字段的值存入data*/
                OracleCommand commMain = oleConn.CreateCommand();
                commMain.CommandText = "select " + colName + " from " + tableName + " where VILLAGENAME='" + zoneNameArr[i] + "' and YEAR=" + year;
                OracleDataReader readMain = commMain.ExecuteReader();
                if (readMain.Read())
                {
                    for (int j = 0; j < colNameArr.Length; j++)
                    {
                        tempData.value.Add(readMain.GetFloat(j));
                    }
                }
                else
                {
                    for (int j = 0; j < colNameArr.Length; j++)
                    {
                        tempData.value.Add(0);
                    }
                }
                readMain.Close();
                data.Add(tempData);
            }
            radar.legend = legend;
            radar.indicator = indicator;
            radar.data = data;
            return radar;
        }
    }
}