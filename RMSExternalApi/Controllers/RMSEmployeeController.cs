using RMSExternalApi.Businesses;
using RMSExternalApi.Commons;
using RMSExternalApi.Models;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMSExternalApi.Controllers
{
    public class RMSEmployeeController : RMSAPIBaseController
    {


        /// <summary>
        /// Employee register to interview worker
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public CusResponse1<object> EmpRegisterInterviewWorker(EmployeeRegister employee)
        {
            try
            {

                #region check field valid
                if (employee == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    };
                if (string.IsNullOrWhiteSpace(employee?.factory))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please input factory")
                    };
                if (string.IsNullOrWhiteSpace(employee?.name))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please input your name")
                    };
                if (string.IsNullOrWhiteSpace(employee?.mobile))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please input your phone")
                    };
                if (string.IsNullOrWhiteSpace(employee?.interviewDate))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please input your interview date")
                    };
                DateTime dateTime1;
                var rsDate = DateTime.TryParseExact(employee.interviewDate?.Replace("-", "/"), "yyyy/MM/dd", new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateTime1);
                if (rsDate == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("your interview date invalid, format:yyyy/mm/dd")
                    };

                #endregion check field valid

                // logic:
                //   chua co dlieu theo jobMail->insert
                //  da ton tai dlieu theo jobMail:
                //-> da ton tai Factory-> update
                //->chua ton tai Factory->insert

                bool rs = false;
                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var oldRegisterLs = RMSEmployeeBusiness.Instance.GetEmpRegisterInterviewLS(curAcc?.F_MAIL);
            checkPoint1:
                if (oldRegisterLs?.Count <= 0)
                {

                    rs = RMSEmployeeBusiness.Instance.AddEmployeeRegisterInterview(employee, curAcc?.F_MAIL, curAcc?.F_MOBILE);
                    goto notifyPoint;
                }

                var oldSameRegisterLs = oldRegisterLs.Where(r => r.F_FACTORY?.Trim() == employee?.factory?.Trim()
                                            && r.JOB_MAIL == curAcc.F_MAIL).ToList();

                if (oldSameRegisterLs?.Count > 0)
                {
                    rs = RMSEmployeeBusiness.Instance.UpdateEmployeeRegisterInterview(employee, curAcc?.F_MAIL);
                    goto notifyPoint;
                }
                else
                {
                    oldRegisterLs = new List<DTO.RMS.IE_R_EMPLOYEE>();
                    goto checkPoint1;
                }

            notifyPoint:
                return new CusResponse1<object>
                {
                    status = rs ? StatusType.success.ToString() : StatusType.error.ToString(),
                    message = rs ? LangHelper.Instance.Get("Register success") : LangHelper.Instance.Get("Register fail")
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-2025103031553] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-2025103031553]"   // ex.Message
                };

            }


        }

        /// <summary>
        /// Get list register to interview worker of current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public CusResponse1<List<EmployeeRegister>> GetEmpRegisterInterviewLS()
        {
            try
            {

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var dat = RMSEmployeeBusiness.Instance.GetEmppRegisterInterviewLS(curAcc?.F_MAIL);
                return new CusResponse1<List<EmployeeRegister>>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dat,
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510301554] " + ex.Message + ex.StackTrace);
                return new CusResponse1<List<EmployeeRegister>>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510301554]"   // ex.Message
                };

            }
        }

     }
}
