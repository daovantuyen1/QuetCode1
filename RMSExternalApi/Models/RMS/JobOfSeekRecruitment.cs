using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class JobOfSeekRecruitment
    {
        public string jobId { set; get; }
        public string position { set; get; }
        public string category { set; get; }
        public string education { set; get; }
        public string experience { set; get; }
        public int qty { set; get; }
        public string publicDate { set; get; }
        public string salary { set; get; }
        public bool isHotJob { set; get; }
        public string jobFullPlace { set; get; }
    }
}