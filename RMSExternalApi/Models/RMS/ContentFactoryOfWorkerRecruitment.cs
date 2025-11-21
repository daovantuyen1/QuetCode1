using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class ContentFactoryOfWorkerRecruitment
    {
        public string factory { set; get; }
        public string content { set; get; }
        public int sort { set; get; }
        public string country { set; get; }
        public string imageName { set; get; }
        public string imagePath { set; get; }
        public string imageDesc { set; get; }
    }
}