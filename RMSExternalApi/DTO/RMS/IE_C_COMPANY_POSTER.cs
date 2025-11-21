using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    public class IE_C_COMPANY_POSTER
    {
        public string ROW_ID { set; get; }
        public string FILE_ID { set; get; }
        public string LANGUAGE_ID { set; get; }
        public string DESCRIPTION { set; get; }
        public string APPLY_TYPE { set; get; }
        public string APPLY_NO { set; get; }
        public string SORT { set; get; }
        public string STATUS { set; get; }
        public string DATA1 { set; get; }
        public string DATA2 { set; get; }
        public string CREATE_EMP { set; get; }
        public string CREATE_TIME { set; get; }
        public string IS_DELETE { set; get; }
        public string UPDATE_EMP { set; get; }
        public string UPDATE_TIME { set; get; }
    }
}