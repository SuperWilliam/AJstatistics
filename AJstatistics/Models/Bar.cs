using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AJstatistics.Models
{
    public class Bar
    {
        public List<string> legend { get; set; }
        public List<string> xAxis { get; set; }
        public List<Series> series { get; set; }
    }
}