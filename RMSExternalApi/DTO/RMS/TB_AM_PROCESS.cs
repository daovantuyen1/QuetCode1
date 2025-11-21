using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    public class TB_AM_PROCESS
    {
        public string ID { set; get; }
        public string IM_NO { set; get; }
        public string ROLE { set; get; }
        public string EMP_NO { set; get; }
        public string NAME { set; get; }
        public string RESULT { set; get; }
        public string CREATE_DATE { set; get; }
        public string SIGN_COMMENT { set; get; }
    }
}