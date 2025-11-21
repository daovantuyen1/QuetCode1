using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS.Requests
{
    public class JobOfSeekRecruitmentReq : ElTableReq
    {
        /// <summary>
        /// 社会招募/校园招聘
        /// </summary>
        public string RecruitmentName { set; get; }
        /// <summary>
        /// VN/EN/CN
        /// </summary>
        public string LanguageId { set; get; }
        /// <summary>
        /// DMZ/INTERNAL
        /// </summary>
        public string Call { set; get; }
        public string Position { set; get; }
        /// <summary>
        /// Example:2024051611195900451189
        /// </summary>
        public string CountryID { set; get; }
        public string JobCategory { set; get; }
        public string Experience { set; get; }
        public string Education { set; get; }
    }


    public class JobOfSeekRecruitmentNewestReq
    {
        /// <summary>
        ///  VN/EN/CN
        /// </summary>
        public string LanguageId { set; get; }
        /// <summary>
        ///  社会招募/校园招聘
        /// </summary>
        public string RecruitmentName { set; get; }

        /// <summary>
        /// DMZ/INTERNAL
        /// </summary>
        public string Call { set; get; }

        /// <summary>
        /// Quantity jobs need to show
        /// </summary>
        public int Count { set; get; }
    }
}