using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    public class IE_R_APPLY_ORDER
    {
        public string APPLY_NO { set; get; }
        public string APPLY_TYPE { set; get; }
        public string APPLY_EMP { set; get; }
        public string APPLY_NAME { set; get; }
        public string APPLY_TIME { set; get; }
        public string STATUS { set; get; }

        public string SIGN_STATION { set; get; }
        public string SIGN_STATION_NO { set; get; }
        public string SIGN_EMP { set; get; }
        public string DATA2 { set; get; }
        public string DATA3 { set; get; }
        public string UPDATE_TIME { set; get; }
        public string CREATE_EMP { set; get; }
        public string CREATE_TIME { set; get; }

    }
}