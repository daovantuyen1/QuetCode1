using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Commons
{
    public class LangHelper
    {
        private JObject viObj;
        private JObject enObj;
        private JObject cnObj;
        #region SingelTon
        private static object lockObj = new object();
        private LangHelper()
        {
            try
            {
                string phisicPath = HttpContext.Current.Server.MapPath("~/Lang/");
                string viPath = phisicPath + "vn.json";
                if (System.IO.File.Exists(viPath))
                {
                    string viStr = System.IO.File.ReadAllText(viPath);
                    if (!string.IsNullOrWhiteSpace(viStr))
                        viObj = JObject.Parse(viStr);
                }

                string enPath = phisicPath + "en.json";
                if (System.IO.File.Exists(enPath))
                {
                    string enStr = System.IO.File.ReadAllText(enPath);
                    if (!string.IsNullOrWhiteSpace(enStr))
                        enObj = JObject.Parse(enStr);
                }

                string cnPath = phisicPath + "cn.json";
                if (System.IO.File.Exists(cnPath))
                {
                    string cnStr = System.IO.File.ReadAllText(cnPath);
                    if (!string.IsNullOrWhiteSpace(cnStr))
                        cnObj = JObject.Parse(cnStr);
                }

            }
            catch (Exception ex)
            {

            }

        }
        private static LangHelper _instance;
        public static LangHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new LangHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        public string Get(string key, params object[] paramsLs)
        {
            try
            {

               
                string requestLang = HttpContext.Current.Request.Headers["language"]?.ToString();
                if (string.IsNullOrWhiteSpace(requestLang)
                    || new List<string> { "VN", "EN", "CN" }.Contains(requestLang) == false)
                    requestLang = "VN";
                string result = "";
                switch (requestLang)
                {
                    case "EN": result = enObj[key].ToString(); break;
                    case "CN": result = cnObj[key].ToString(); break;
                    case "VN": 
                    default:
                        result = viObj[key].ToString();
                        break;
                }
                if (string.IsNullOrWhiteSpace(result))
                    result = key;

                if (paramsLs != null && paramsLs.Count() > 0)
                {
                    result = string.Format(result, paramsLs);
                }

                return result;

            }
            catch (Exception ex)
            {
                string result = key;
                if (paramsLs != null && paramsLs.Count() > 0)
                {
                    result = string.Format(result, paramsLs);
                }
                return result;
            }



        }


    }
}