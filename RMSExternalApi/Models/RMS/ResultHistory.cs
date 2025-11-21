using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{

    /// <summary>
    /// Result history of external job
    /// </summary>
    public class ResultHistory
    {
        public string result { set; get; }
        public string createDate { set; get; }
    }
}