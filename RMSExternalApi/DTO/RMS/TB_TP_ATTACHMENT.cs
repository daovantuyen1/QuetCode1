using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    public class TB_TP_ATTACHMENT
    {
        public string ATTID { set; get; }
        public string TPID { set; get; }
        public string FILEID { set; get; }
        public string FILEPATH { set; get; }
        public string CONTENTTYPE { set; get; }
        public string CREATE_TIME { set; get; }
        public string CREATE_EMP { set; get; }
    }
}