using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    public class IE_R_LOGIN_OTP
    {
        /// <summary>
        /// mail or phone
        /// </summary>
        public string F_KEY { set; get; }
        /// <summary>
        /// OTP code
        /// </summary>
        public string F_OTP { set; get; }
        /// <summary>
        /// Create date
        /// </summary>
        public string F_SYSDATE { set; get; }



       /// <summary>
       /// CLient IP
       /// </summary>
        public string F_IP { set; get; }

    }
}