using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS.Requests
{
    public class CompanyPosterAppReq
    {
        public string applyNo { set; get; }
        public string applyEmp { set; get; }
        public string signEmp { set; get; }
        public int page { set; get; }
        public int pageSize { set; get; }
    }

    public class CompanyProfileAppReq: CompanyPosterAppReq
    {

    }

    public class OurBusinessAppReq : CompanyPosterAppReq
    {

    }

    public class SkilledWorkerAppReq : CompanyPosterAppReq
    {

    }


    public class PublicWelfareAppReq : CompanyPosterAppReq
    {

    }

}