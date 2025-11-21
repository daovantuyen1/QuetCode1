
using RMSExternalApi.Businesses;
using RMSExternalApi.Commons;
using RMSExternalApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMSExternalApi.Controllers
{
    public class RMSCVTotalController : RMSAPIBaseController
    {

        /// <summary>
        ///  count total job your registered
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public CusResponse1<int?> GetCountTotalJobRegister()
        {
            try
            {

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var count1 = RMSCVExternalJobBusiness.Instance.GetCVExternalJobLsOfJobMail(curAcc?.F_MAIL);
                var count2 = RMSCVSchoolJobBusiness.Instance.GetCVSchoolJobLsOfJobMail(curAcc?.F_MAIL);
                var count3 = RMSEmployeeBusiness.Instance.GetEmpRegisterInterviewLS(curAcc?.F_MAIL);
                return new CusResponse1<int?>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = count1?.Count + count2?.Count + count3?.Count,
                };


            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202511071855] " + ex.Message + ex.StackTrace);
                return new CusResponse1<int?>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202511071855]"  ,
                    data=0 ,
                };

            }


        }
    }
}
