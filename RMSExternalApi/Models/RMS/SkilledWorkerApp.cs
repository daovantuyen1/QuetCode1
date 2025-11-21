using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class SkilledWorkerApp
    {

        public string LangId { set; get; }
        public string site { set; get; }
        public string content { set; get; }
        public string country { set; get; }
        public string recruimentName { set; get; }
        public string applyType { set; get; }
        public string applyNo { set; get; }
        public int? sort { set; get; }
        public string publicStatus { set; get; }
        public string createdAppEmp { set; get; }
        public string createdAppDate { set; get; }
        public string isDelete { set; get; }
        public string applyEmp { set; get; }
        public string applyName { set; get; }
        public string applyTime { set; get; }
        public string applyStatus { set; get; }
        public string signStationName { set; get; }
        public string FlowId { set; get; }
        public string signEmp { set; get; }
        public string signName { set; get; }
        public string signMail { set; get; }
        public string fileId { set; get; }

        public string fileName { set; get; }
        public string fileExpandedName { set; get; }
        public string filePath { set; get; }
        public string fileSize { set; get; }
        public string fileDesc { set; get; }
    }
}