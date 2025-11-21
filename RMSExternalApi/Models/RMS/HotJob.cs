using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class HotJob
    {
        public string jobId { set; get; }
        public string jobPosition { set; get; }
        public string jobCategory { set; get; }
        public string jobEducation { set; get; }
        public string jobExperience { set; get; }
        public int jobQty { set; get; }
        public string jobPublicDate { set; get; }
        public string jobSalary { set; get; }
        public bool isHotJob { set; get; }
        public string jobFullPlace { set; get; }
        public string jobPlace { set; get; }
        
    }
}