using RMSExternalApi.Commons;
using RMSExternalApi.Models;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RMSExternalApi.Businesses
{
    public class RMSConfigDataBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSConfigDataBusiness() { }
        private static RMSConfigDataBusiness _instance;
        public static RMSConfigDataBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSConfigDataBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        /// <summary>
        /// Get eduction level list
        /// </summary>
        /// <returns></returns>

        public CusResponse1<List<ConfigData>> GetEducationLevelLs()
        {
            var eduLs = new List<ConfigData>() {
              new ConfigData { value = "博士后", textVn = "Nghiên cứu sinh sau tiến sĩ", textCn = "博士后", textEn = "Postdoctoral Researcher" },
              new ConfigData { value = "博士", textVn = "Tiến sĩ", textCn = "博士", textEn = "PhD" },
              new ConfigData { value = "硕士", textVn = "Thạc sĩ", textCn = "硕士", textEn = "Master" },
              new ConfigData { value = "硕士A", textVn = "Thạc sĩ A", textCn = "硕士A", textEn = "Master A" },
              new ConfigData { value = "硕士B", textVn = "Thạc sĩ B", textCn = "硕士B", textEn = "Master B" },
              new ConfigData { value = "硕士B", textVn = "Thạc sĩ B", textCn = "硕士B", textEn = "Master B" },
              new ConfigData { value = "EMBA/MBA", textVn = "EMBA/MBA", textCn = "EMBA/MBA", textEn = "EMBA/MBA" },
              new ConfigData { value = "本科", textVn = "Đại học", textCn = "本科", textEn = "University" },
              new ConfigData { value = "本科A", textVn = "Đại học A", textCn = "本科A", textEn = "University A" },
              new ConfigData { value = "本科B", textVn = "Đại học B", textCn = "本科B", textEn = "University B" },
              new ConfigData { value = "大专", textVn = "Cao đẳng", textCn = "大专", textEn = "College" },
              new ConfigData { value = "大专A", textVn = "Cao đẳng A", textCn = "大专A", textEn = "College A" },
              new ConfigData { value = "大专B", textVn = "Cao đẳng B", textCn = "大专B", textEn = "College B" },
              new ConfigData { value = "高技", textVn = "Cao đẳng kỹ thuật", textCn = "高技", textEn = "Technical College" },
              new ConfigData { value = "中技", textVn = "Trung cấp kỹ thuật", textCn = "中技", textEn = "Technical Secondary School" },
              new ConfigData { value = "中专", textVn = "Trung cấp chuyên nghiệp", textCn = "中专", textEn = "Vocational Secondary School" },
              new ConfigData { value = "高中", textVn = "Trung học Phổ thông", textCn = "高中", textEn = "Trung học Phổ thông" },
              new ConfigData { value = "初中及以下", textVn = "Trung học cơ sở trở xuống", textCn = "初中及以下", textEn = "Junior high school and below" },
              new ConfigData { value = "不限", textVn = "Không xác định", textCn = "不限", textEn = "Unknown" },

            };


            return new CusResponse1<List<ConfigData>>
            {
                status = StatusType.success.ToString(),
                message = StatusType.success.ToString(),
                data = eduLs
            };


        }

        /// <summary>
        /// Get factory list
        /// </summary>
        /// <returns></returns>

        public CusResponse1<List<ConfigData>> GetFactoryLs()
        {
            try
            {

                var dat = RMSEmployeeBusiness.Instance.getNodeTreeBySysNameAndFatherNode("Plant", "RMS");
                var dat1 = dat?.Select(r =>
                {
                    if (r.NODE_NAME == "越南") return null;
                    string textVn = "";
                    string textEn = "";
                    switch (r.NODE_NAME)
                    {
                        case "桂武工業區": textVn = "Khu công nghiệp Quế Võ"; textEn = "Que Vo Industrial Zone"; break;
                        case "光州工業區": textVn = "Khu công nghiệp Quang Châu"; textEn = "Quang Chau Industrial Zone"; break;
                        case "黃田工業區": textVn = "Khu công nghiệp Đồng Vàng"; textEn = "Dong Vang Industrial Zone"; break;
                    }

                    return new ConfigData { value = r?.NODE_NAME, textVn = textVn, textCn = r?.NODE_NAME, textEn = textEn };

                }).Where(r => r != null).ToList();

                return new CusResponse1<List<ConfigData>>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dat1
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510311447] " + ex.Message + ex.StackTrace);
                return new CusResponse1<List<ConfigData>>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510311447]"   // ex.Message
                };
            }


        }




        /// <summary>
        /// Get gender list
        /// </summary>
        /// <returns></returns>

        public CusResponse1<List<ConfigData>> GetGenderLs()
        {
            var genderLs = new List<ConfigData>()
            {
                 new ConfigData{ value="M", textVn="Nam",textCn ="男" , textEn ="Male"  },
                  new ConfigData{ value="F", textVn="Nữ",textCn ="女" , textEn ="Female"  },

            };

            return new CusResponse1<List<ConfigData>>
            {
                status = StatusType.success.ToString(),
                message = StatusType.success.ToString(),
                data = genderLs
            };


        }


    }
}