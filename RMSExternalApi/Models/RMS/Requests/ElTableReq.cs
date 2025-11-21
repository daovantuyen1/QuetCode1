using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS.Requests
{
    public class ElTableReq
    {
        /// <summary>
        /// Order of page to display: example page 1 , page 2
        /// </summary>
        public int page { set; get; }
        /// <summary>
        /// Total qty of rows will show on a page : example :  pageSize=10 ,pageSize=20
        /// </summary>
        public int pageSize { set; get; }
    }
}