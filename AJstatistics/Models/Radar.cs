using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AJstatistics.Models
{
    public class Radar
    {
        public List<string> legend { get; set; }
        public List<IndicatorItem> indicator { get; set; }
        public List<DataItem> data { get; set; }
    }
}