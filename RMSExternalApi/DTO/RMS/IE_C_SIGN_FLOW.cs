using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    public class IE_C_SIGN_FLOW
    {
        public string  ROW_ID { set; get; }
        public string LANGUAGE_ID { set; get; }
        public string APPLY_TYPE { set; get; }
        public string PUBLIC_EMP { set; get; }
        public string PUBLIC_NAME { set; get; }
        public string PUBLIC_EMAIL { set; get; }
        public string MANAGER1_EMP { set; get; }
        public string MANAGER1_NAME { set; get; }
        public string MANAGER1_EMAIL { set; get; }
        public string MANAGER2_EMP { set; get; }
        public string MANAGER2_NAME { set; get; }
        public string MANAGER2_EMAIL { set; get; }
        public string MANAGER3_EMP { set; get; }
        public string MANAGER3_NAME { set; get; }
        public string MANAGER3_EMAIL { set; get; }
        public string STATUS { set; get; }
        public string DATA2 { set; get; }
        public string DATA3 { set; get; }
        public string UPDATE_TIME { set; get; }
        public string CREATE_EMP { set; get; }
        public string CREATE_TIME { set; get; }
        public string IS_DELETE { set; get; }
    }
}