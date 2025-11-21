using RMSExternalApi.Businesses;
using RMSExternalApi.Commons;
using RMSExternalApi.Models;
using RMSExternalApi.Models.RMS;
using RMSExternalApi.Models.RMS.Requests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RMSExternalApi.Controllers
{

    public class RMSAccountController : RMSAPIBaseController
    {

        #region Login, manage account


        /// <summary>
        /// Check jwt authen valid  ➔ if valid  ➔ return user infor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public CusResponse1<UserInfor> CheckLoginValid()
        {
            try
            {

                var dat = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();

                return new CusResponse1<UserInfor>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = new UserInfor { mail = dat?.F_MAIL, name = dat?.F_NAME, mobile = dat?.F_MOBILE }
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510220946] " + ex.Message + ex.StackTrace);
                return new CusResponse1<UserInfor>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510220946]"   // ex.Message
                };

            }

        }


        /// <summary>
        /// Check email valid  ➔ send OTP to email
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public CusResponse1<object> LoginWithEmail(LoginWithEmail req)
        {
            try
            {
                if (Util.IsValidEmail(req?.email) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Email invalid")
                    };

                // email hop le -> tao ma OTP-> gui den mail cua user

                string randomOTP = Util.GetRandomStringOTP();

                if (RMSAccountBusiness.Instance.SaveOTP(req?.email, randomOTP))
                {
                    // gui mail OTP den user

                    if (RMSMailBusiness.Instance.SendMailOTPCodeToUser(randomOTP, req?.email) == false)
                        return new CusResponse1<object>
                        {
                            status = StatusType.success.ToString(),
                            message =LangHelper.Instance.Get( "Send OTP fail")
                        };
                    //
                    else
                        return new CusResponse1<object>
                        {
                            status = StatusType.success.ToString(),
                            message = LangHelper.Instance.Get( "Created OTP success and sended mail to user")
                        };
                }
                else
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Create OTP fail")
                    };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510211503] " + ex.Message + ex.StackTrace);

                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510211503]"   // ex.Message
                };

            }


        }



        /// <summary>
        /// Check phone valid  ➔ send OTP to phone
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public CusResponse1<object> LoginWithPhone(LoginWithPhone req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(req?.phone))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Please input phone")
                    };

                // email hop le -> tao ma OTP-> gui den mail cua user

                string randomOTP = Util.GetRandomStringOTP();

                if (RMSAccountBusiness.Instance.SaveOTP(req?.phone, randomOTP))
                {
                    // cho gui  OTP den phone cua user


                    //
                    return new CusResponse1<object>
                    {
                        status = StatusType.success.ToString(),
                        message = LangHelper.Instance.Get( "Created OTP success and sended OTP to phone of user")
                    };
                }
                else
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Create OTP fail")
                    };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510290928] " + ex.Message + ex.StackTrace);

                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510290928]"   // ex.Message
                };

            }


        }



        /// <summary>
        /// Send OTP and Email and confirm it valid  ➔ if valid return JWT token for authen 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public CusResponse1<string> ConfirmLoginOTPForEmail(ConfirmOTPForEmail req)
        {
            try
            {

                if (Util.IsValidEmail(req?.email) == false)
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Email invalid")
                    };
                if (string.IsNullOrWhiteSpace(req?.OTP))
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "OTP is empty")
                    };
                var curOTP = RMSAccountBusiness.Instance.GetOTPInfor(req?.email, req.OTP);
                if (curOTP == null)
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "OTP invalid")
                    };
                var otpCreatedTime = DateTime.ParseExact(curOTP.F_SYSDATE, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"));
                if ((DateTime.Now - otpCreatedTime).TotalMinutes > 10)  // thoi gian ma OTP hop le la <=10 phut
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("OTP has expired")
                    };

                // da check OTP hop le-> tao jwt cho authen
                var accInfor = RMSAccountBusiness.Instance.GetAccountInforByMail(req?.email);
                if (accInfor == null)
                {
                    if (RMSAccountBusiness.Instance.AddAccount(req?.email, "", "") == false)
                        return new CusResponse1<string>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get( "Confirm fail")
                        };
                }
                accInfor = RMSAccountBusiness.Instance.GetAccountInforByMail(req?.email);
                string jwtToken = AuthenToken.GetToken(accInfor?.F_NAME, accInfor.F_MAIL, accInfor.F_MOBILE);
                if (string.IsNullOrWhiteSpace(jwtToken))
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Error when create token")
                    };


                // confirm OTP success-> return JWT 
                RMSAccountBusiness.Instance.AddRLoginSession(jwtToken, !string.IsNullOrWhiteSpace(accInfor?.F_MAIL) ? accInfor?.F_MAIL : accInfor?.F_MOBILE);

                return new CusResponse1<string>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = "Bearer " + jwtToken,
                };

                //
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510211552] " + ex.Message + ex.StackTrace);
                return new CusResponse1<string>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510211552]"   // ex.Message
                };

            }




        }



        /// <summary>
        /// Send OTP and Phone and confirm it valid  ➔ if valid return JWT token for authen 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public CusResponse1<string> ConfirmLoginOTPForPhone(ConfirmOTPForPhone req)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(req?.phone))
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Phone is empty")
                    };
                if (string.IsNullOrWhiteSpace(req?.OTP))
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("OTP is empty")
                    };
                var curOTP = RMSAccountBusiness.Instance.GetOTPInfor(req?.phone, req.OTP);
                if (curOTP == null)
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("OTP invalid")
                    };
                var otpCreatedTime = DateTime.ParseExact(curOTP.F_SYSDATE, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"));
                if ((DateTime.Now - otpCreatedTime).TotalMinutes > 10)  // thoi gian ma OTP hop le la <=10 phut
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("OTP has expired")
                    };

                // da check OTP hop le-> tao jwt cho authen
                var accInfor = RMSAccountBusiness.Instance.GetAccountInforByPhone(req?.phone);
                if (accInfor == null)
                {
                    if (RMSAccountBusiness.Instance.AddAccount("", "", req?.phone) == false)
                        return new CusResponse1<string>
                        {
                            status = StatusType.error.ToString(),
                            message =LangHelper.Instance.Get( "Confirm fail")
                        };
                }
                accInfor = RMSAccountBusiness.Instance.GetAccountInforByPhone(req?.phone);
                string jwtToken = AuthenToken.GetToken(accInfor?.F_NAME, accInfor.F_MAIL, accInfor.F_MOBILE);
                if (string.IsNullOrWhiteSpace(jwtToken))
                    return new CusResponse1<string>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Error when create token")
                    };

                // confirm OTP success-> return JWT 
                return new CusResponse1<string>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = "Bearer " + jwtToken,
                };

                //
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510290941] " + ex.Message + ex.StackTrace);
                return new CusResponse1<string>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510290941]"   // ex.Message
                };

            }




        }



        /// <summary>
        /// Update Email for your account , This method Need confirm OTP  ,Each Email only allows link only one times
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public CusResponse1<object> UpdateEmailForAccount(UpdateEmailForAccount req)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(req?.mail))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Please input your email")
                    };

                if (Util.IsValidEmail(req?.mail) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please input valid email")
                    };
                if (string.IsNullOrWhiteSpace(req?.OTP))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please input OTP")
                    };


                // cho check valid OTP

                var curOTP = RMSAccountBusiness.Instance.GetOTPInfor(req?.mail, req.OTP);
                if (curOTP == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("OTP invalid")
                    };
                var otpCreatedTime = DateTime.ParseExact(curOTP.F_SYSDATE, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"));
                if ((DateTime.Now - otpCreatedTime).TotalMinutes > 10)  // thoi gian ma OTP hop le la <=10 phut
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("OTP has expired")
                    };



                //

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                if (string.IsNullOrWhiteSpace(curAcc?.F_MOBILE))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Account invalid")
                    };

                // email chi dk phep su dung 1 lan - chi dk lien ket 1 email vs 1 tk 1 lan duy nhat.
                var otherMail = RMSAccountBusiness.Instance.GetAccountInforByMail(req?.mail);
                if (otherMail != null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Each email only is linked only once times , This email used, please use an other email")
                    };

                if (RMSAccountBusiness.Instance.UpdateAccountWithKeyIsMobile(req?.mail, curAcc.F_MOBILE) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Update fail")
                    };
                else
                    return new CusResponse1<object>
                    {
                        status = StatusType.success.ToString(),
                        message = StatusType.success.ToString(),
                    };

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510290841] " + ex.Message + ex.StackTrace);

                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510290841]"   // ex.Message
                };


            }


        }


        /// <summary>
        /// Update phone for your account, This method Need confirm OTP  ,Each phone only allows link only one times
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public CusResponse1<object> UpdatePhoneForAccount(UpdatePhoneForAccount req)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(req?.phone))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Please input your phone")
                    };

                if (string.IsNullOrWhiteSpace(req?.OTP))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Please input OTP")
                    };


                // cho check OTP valid

                var curOTP = RMSAccountBusiness.Instance.GetOTPInfor(req?.phone, req.OTP);
                if (curOTP == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("OTP invalid")
                    };
                var otpCreatedTime = DateTime.ParseExact(curOTP.F_SYSDATE, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"));
                if ((DateTime.Now - otpCreatedTime).TotalMinutes > 10)  // thoi gian ma OTP hop le la <=10 phut
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("OTP has expired")
                    };
                //

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                if (string.IsNullOrWhiteSpace(curAcc?.F_MAIL))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message =  LangHelper.Instance.Get("Account invalid")
                    };

                // phone chi dk phep su dung 1 lan - chi dk lien ket 1 phone vs 1 tk 1 lan duy nhat.
                var otherPhone = RMSAccountBusiness.Instance.GetAccountInforByPhone(req?.phone);
                if (otherPhone != null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Each phone only is linked only once times , This phone used, please use an other phone")
                    };


                if (RMSAccountBusiness.Instance.UpdateAccountWithKeyIsEmail(curAcc?.F_MAIL, req?.phone) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Update fail")
                    };
                else
                    return new CusResponse1<object>
                    {
                        status = StatusType.success.ToString(),
                        message = StatusType.success.ToString(),
                    };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510290840] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510290840]"   // ex.Message
                };
            }

        }


        /// <summary>
        /// Update name  for your account 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        [HttpPost]
        public CusResponse1<object> UpdateNameForAccount(UpdateAccount req)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(req?.name))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message =LangHelper.Instance.Get(  "Please input your name")
                    };



                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();


                if (RMSAccountBusiness.Instance.UpdateNameForAccount(req?.name, curAcc?.F_MAIL, curAcc?.F_MOBILE) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Update account fail")
                    };
                return new CusResponse1<object>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510211646] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510211646]"   // ex.Message
                };
            }

        }


#if DEBUG
        /// <summary>
        /// Get current OTP of mail/phone (Only use for dev process)
        /// </summary>
        /// <param name="key">email/phone</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public CusResponse1<object> GetCurrentOTP(string key)
        {

            try
            {
                var dat = RMSAccountBusiness.Instance.GetOTPInforByKey(key);
                return new CusResponse1<object>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dat?.F_OTP,
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510301641] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510301641]"   // ex.Message
                };
            }


        }
#endif

        [HttpGet]
        public CusResponse1<object> Logout()
        {
            try
            {
                var BearerToken = HttpContext.Current.Request.Headers["Authorization"]?.ToString();
                BearerToken = BearerToken?.Replace("Bearer", "")?.Trim();
                RMSAccountBusiness.Instance.DeleteTokenRLoginSession(BearerToken);
                return new CusResponse1<object>
                {
                    status = StatusType.success.ToString(),
                    message = LangHelper.Instance.Get( "Logout success")
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202511011607] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202511011607]"   // ex.Message
                };

            }

        }
        #endregion Login, manage account


    }
}
