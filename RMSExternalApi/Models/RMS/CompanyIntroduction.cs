using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class CompanyIntroduction
    {
        public string templateContent { set; get; }
        public string country { set; get; }
        public int sort { set; get; }
        public string templateHightLight { set; get; }
        public string imageName { set; get; }
        public string imagePath { set; get; }
        public string imageDesc { set; get; }
    }
}