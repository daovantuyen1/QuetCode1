using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO
{

    /// <summary>
    /// History of RESULT of table TB_IM_INTERVIEW
    /// </summary>
    public class TB_IM_INTERVIEW_RESULT_HISTORY
    {
        public string IMNO { set; get; }
        public string TPID { set; get; }
        public string RESULT { set; get; }
        public string RESULT_TEXT { set; get; }
        public string CREATE_DATE { set; get; }
    }
}