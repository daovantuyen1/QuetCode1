using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMSExternalApi.Commons;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RMSExternalApi.Businesses
{
    public class RMSMailBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSMailBusiness() { }
        private static RMSMailBusiness _instance;
        public static RMSMailBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSMailBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion



        /// <summary>
        /// Send mail OTP to user
        /// </summary>
        /// <param name="OTP"></param>
        /// <param name="mailTo"></param>
        public bool SendMailOTPCodeToUser(string OTP, string mailTo)
        {
            string temPlateBodyMail = @"
                <div>
                 <h1 style='color:red;text-align: center;'>Mail gửi xác nhận mã OTP</h1>
                <div style='padding-left: 26px;'>
                   <p> Chào bạn, </p>
                    <div>Mã OTP của bạn là : <span style='font-weight: bold;color: red;'>@OTP</span></div>
                <p style='color:red'>Mã OTP sẽ hết hạn trong 10 phút </p>
                </div>
                 </div>
                <div>
                 <h1 style='color:red;text-align: center;'>Email sent to confirm OTP code</h1>
                <div style='padding-left: 26px;'>
                   <p> Hi you, </p>
                    <div>Your OTP code is : <span style='font-weight: bold;color: red;'>@OTP</span></div>
                <p style='color:red'>OTP code will expire in 10 minutes </p>
                </div>
                 </div>
                ";

            string mailBody = temPlateBodyMail.Replace("@OTP", OTP);
            var maildata = new MailDataView()
            {
                KeyRowIDMail = Constant.KEY_MAIL_FOR_ROW_ID + mailTo?.Trim()?.ToLower(),
                MAIL_FROM = "vn-it-app@mail.foxconn.com",
                MAIL_TO = mailTo?.Trim()?.ToLower(),
                IMPORTANT_LEVEL = "HIGH",
                IS_HTML = "YES",
                MAIL_SUBJECT = "Foxconn-Hệ thống đăng ký tuyển dụng/Foxconn-Recruitment System",
                MAIL_BODY = mailBody,
                UserName = Constant.SEND_MAIL_USER,
                PassWord = Constant.SEND_MAIL_PASS,
            };
            return SendMailOTP(maildata);

        }

        private bool SendMailOTP(MailDataView maildata)
        {
            try
            {
                if (Constant.IS_SEND_MAIL == "Y")
                {
                    if (maildata != null)
                    {

                        Util.ByPassHttps();

                        var httpClient = new HttpClient();
                        if (string.IsNullOrWhiteSpace(maildata.MAIL_CC))
                        {
                            maildata.MAIL_CC = "";
                        }

                        //    https://vn-webportal-cns.myfiinet.com/SendMailApi/api/Mailer/SendMailOTP
                        var json = JsonConvert.SerializeObject(maildata);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var responseTask = httpClient.PostAsync(Constant.API_SEND_MAIL + "/SendMailOTP", content);
                        Task.WaitAll(responseTask);
                        var response = responseTask.Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var taskrs = response.Content.ReadAsStringAsync();
                            Task.WaitAll(taskrs);
                            string rs = taskrs.Result;
                            LoggingLocal.SaveLog(LogType.Info, $"Send mail success,mailData: MAIL_TO:{maildata.MAIL_TO} , MAIL_CC:{maildata.MAIL_CC}, MAIL_SUBJECT:{maildata.MAIL_SUBJECT}");
                            var rsObj = JObject.Parse(rs);
                            if (rsObj["status"]?.ToString() == "success")
                                return true;
                        }
                        else
                        {
                            LoggingLocal.SaveLog(LogType.Info, $"Send mail fail,mailData: MAIL_TO:{maildata.MAIL_TO} , MAIL_CC:{maildata.MAIL_CC}, MAIL_SUBJECT:{maildata.MAIL_SUBJECT}");

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Info, $"Send mail error,mailData: MAIL_TO:{maildata.MAIL_TO} , MAIL_CC:{maildata.MAIL_CC}, MAIL_SUBJECT:{maildata.MAIL_SUBJECT}, exception:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

    }
}