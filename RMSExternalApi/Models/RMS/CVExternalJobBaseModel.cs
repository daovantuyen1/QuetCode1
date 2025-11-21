using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{

    public class CVJobBaseModel   
    {

        public string tpID { set; get; }
        public string createDate { set; get; }
        public string updateDate { set; get; }
        /// <summary>
        /// Status of TPID 
        /// </summary>
        public string tpStatusId { set; get; }
        public string jobMail { set; get; }

        public string jobName { set; get; }
        public string jobID { set; get; }

        /// <summary>
        /// Status of doc no follow HR sign process : Deleted/Draff/Submited and waiting/ etc ..  (Readonly)
        /// </summary>
        public string status { set; get; }


        /// <summary>
        /// Allow edit your CV job ? Flag to check allow show Edit CV job (Readonly)
        /// </summary>
        public bool isAllowEdit { set; get; }

    }

    public class CVExternalJobBaseModel: CVJobBaseModel
    {
        /// <summary>
        /// List interview history result of External job
        /// </summary>
        public List<AmProcess> amProcesses { set; get; }

        /// <summary>
        /// List result history of External job
        /// </summary>
        public List<ResultHistory> resultHistories { set; get; }

    }

    public class CVSchoolJobBaseModel: CVJobBaseModel
    {
        /// <summary>
        /// List interview history result of school job
        /// </summary>
        public List<SchoolImProcess> schoolImProcesses { set; get; }
    }
}