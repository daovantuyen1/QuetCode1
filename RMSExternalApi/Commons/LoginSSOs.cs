using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RMSExternalApi.Commons
{
    public class LoginSSOs
    {
        #region SingelTon
        private static object lockObj = new object();
        private LoginSSOs() { }
        private static LoginSSOs _instance;
        public static LoginSSOs Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoginSSOs();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        public string GetLoginToken(string code)
        {

            //specify to use TLS 1.2 as default connection
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            StringBuilder buffer = new StringBuilder();
            buffer.AppendFormat("{0}={1}", "client_id", ConfigurationManager.AppSettings["client_id"].ToString());
            buffer.AppendFormat("{0}={1}", "&client_secret", ConfigurationManager.AppSettings["client_secret"].ToString());
            buffer.AppendFormat("{0}={1}", "&redirect_uri", ConfigurationManager.AppSettings["redirect_uri"].ToString());
            buffer.AppendFormat("{0}={1}", "&grant_type", "authorization_code");
            buffer.AppendFormat("{0}={1}", "&code", code);
            HttpContent content = new StringContent(buffer.ToString());

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpClient client = new HttpClient();
            // tokenUrl
            var Taskresponse = client.PostAsync(ConfigurationManager.AppSettings["tokenUrl"].ToString(), content);
            Task.WaitAll(Taskresponse);
            HttpResponseMessage response = Taskresponse.Result;
            var taskres = response.Content.ReadAsStringAsync();
            Task.WaitAll(taskres);
            string res = taskres.Result;
            return res;
        }
        /// <summary>
        /// sau khi dang nhap sso tren trang lh-account thanh cong se redirect den action nay cung voi parmam code
        /// </summary>
        /// <param name="code"></param>
        public string LoginSSO(string code)
        {
            OauthToken oauthToken = JsonConvert.DeserializeObject<OauthToken>(GetLoginToken(code));
            string token = oauthToken.token_type + " " + oauthToken.access_token;
            OauthUser oauthUser = JsonConvert.DeserializeObject<OauthUser>(GetOauthUser(token));
            string empno = oauthUser.username;
            return empno;
        }
        public string GetOauthUser(string token)
        {

            HttpClient client = new HttpClient();

            //specify to use TLS 1.2 as default connection
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                //profileUrl
                RequestUri = new Uri(ConfigurationManager.AppSettings["profileUrl"].ToString())
            };
            httpRequestMessage.Headers.Add("Authorization", token);
            var Taskresponse = client.SendAsync(httpRequestMessage);
            Task.WaitAll(Taskresponse);
            HttpResponseMessage response = Taskresponse.Result;
            var takres = response.Content.ReadAsStringAsync();
            Task.WaitAll(takres);
            string res = takres.Result;
            return res;
        }
    }

    public class OauthError
    {
        public string error { get; set; }

        public string error_description { get; set; }
    }


    public class OauthUser : OauthError
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string org { get; set; }
    }

    public class OauthToken : OauthError
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public List<string> scope { get; set; }
    }

}