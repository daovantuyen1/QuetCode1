using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    /// <summary>
    /// Save jwt login of user, use for check login, logout
    /// </summary>
    public class IE_R_LOGIN_SESSION
    {
        public string F_ACCOUNT { set; get; }
        public string F_IP { set; get; }
        public string F_TOKEN { set; get; }
        public string F_CREATE_DATE { set; get; }
    }
}