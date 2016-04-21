using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJstatistics.Models
{
    public interface IStatisticsRepository
    {
        Bar GetBarData(string tableName,string colName,string zoneName,string type,int year);
        Radar GetRadarData(string tableName, string colName, string zoneName, string type, int year);
    }
}
