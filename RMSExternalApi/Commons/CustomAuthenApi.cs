using RMSExternalApi.Businesses;
using RMSExternalApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace RMSExternalApi.Commons
{
    /// <summary>
    /// Custom authen for web api
    /// </summary>
    public class CustomAuthenApiAttribute : System.Web.Http.AuthorizeAttribute
    {
        private bool? isRoled = null;
        public string Role1 { set; get; }



        protected  override bool IsAuthorized(HttpActionContext actionContext)
        {
            //if (HttpContext.Current.Session != null && HttpContext.Current.Session["Account"] != null)
            //{
            //    string err = "";
            //    var acc1 = HttpContext.Current.Session["Account"] as TB_ACCOUNT;
            //    var acc =  AccountBusiness.Instance.GetAccountInFor(acc1.F_EMPNO, "", false, ref err);
            //    if(acc==null||!string.IsNullOrWhiteSpace(err) )
            //    {
            //        isRoled = false;
            //        return false;
            //    }

            //    if (!string.IsNullOrWhiteSpace(Role1)) //need check role
            //    {
            //        var roleNeedCheck = Role1.Split(';').Where(r=>!string.IsNullOrWhiteSpace(r)).Select(t=>t.Trim()).ToList();
            //        var roleAccount = acc.F_ROLE.Split(';').Where(r=>!string.IsNullOrWhiteSpace(r)).Select(r=>r.Trim()).ToList();
            //        if (roleAccount.Join(roleNeedCheck, x => x.ToUpper(), y => y.ToUpper(), (a, b) => a).Count() > 0)
            //        {
            //            isRoled = true;
            //            return true;
            //        }
            //        else
            //        {
            //            isRoled = false;
            //            return false;
            //        }

            //    }
            //    else return true;  // no need check role
            //}
            //else
                return false;
        }

        //protected  bool OnAuthorization(HttpContextBase httpContext)
        //{
        //    if (HttpContext.Current.Session != null && HttpContext.Current.Session["Account"] != null)
        //    {
        //        var acc = HttpContext.Current.Session["Account"] as TB_ACCOUNT;
        //        if (!string.IsNullOrWhiteSpace(Role1)) //need check role
        //        {
        //            var roleNeedCheck = Role1?.Split(';').ToList();
        //            var roleAccount = acc.F_ROLE?.Split(';').ToList();
        //            if (roleAccount.Join(roleNeedCheck, x => x.ToUpper(), y => y.ToUpper(), (a, b) => a).Count() > 0)
        //            {
        //                isRoled = true;
        //                return true;
        //            }
        //            else
        //            {
        //                isRoled = false;
        //                return false;
        //            }

        //        }
        //        else return true;  // no need check role
        //    }
        //    else
        //        return false;
        //}


        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if(isRoled==false)  // invalid role
            {
                actionContext.Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Content = new StringContent(LangHelper.Instance.Get( "Bạn không có quyền thao tác"))
                };
            }
            else
            {
                actionContext.Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("authorization has been denied for this request")
                };

            }

        }
    }

}