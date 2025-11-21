using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    public class TB_TP_TALENTPOOL
    {
        public string TPID { set; get; }
        public string NAME { set; get; }
        public string SEX { set; get; }
        public string AGE { set; get; }
        public string ENGLISHNAME { set; get; }
        public string NATIONALITY { set; get; }
        public string PASSPORT { set; get; }
        public string MOBILE { set; get; }
        public string MARRIED { set; get; }
        public string ENGLISHLEVEL { set; get; }
        public string WORKINGLIFE { set; get; }
        public string EMAIL { set; get; }
        public string OTHERFL { set; get; }
        public string SALARYCM { set; get; }
        public string SALARYCY { set; get; }
        public string DOMICILE { set; get; }
        public string SALARYEM { set; get; }
        public string SALARYEY { set; get; }
        public string POSITIONE { set; get; }
        public string SITEE { set; get; }
        public string HUNTING { set; get; }
        public string ONBOARDGAP { set; get; }
        public string SKILLS { set; get; }
        public string SOURCEID { set; get; }
        public string RESUMECOMMENT { set; get; }
        public string MAILINGDATE { set; get; }
        public string RCOM { set; get; }
        public string TPSTATUSID { set; get; }
        public string STATUSDETAIL { set; get; }
        public string CREATEBY { set; get; }
        public string CREATEDATE { set; get; }
        public string UPDATEBY { set; get; }
        public string UPDATEDATE { set; get; }
        public string DEMANDNO { set; get; }
        public string MARK { set; get; }
        public string EDITSTATUS { set; get; }
        public string LANGUAGE { set; get; }
        public string PLANT { set; get; }
        public string COMBINE { set; get; }
        public string ISDELETE { set; get; }
        public string ISFAIL { set; get; }
        public string LASTIMTIME { set; get; }
        public string RESUMELABEL { set; get; }
        public string AVATAR_IMG { set; get; }
        public string VIETNAMESENAME { set; get; }
        public string ETHNIC { set; get; }
        public string CITIZENIDOLD { set; get; }
        public string CITIZENID { set; get; }
        public string CITIZENIDDATE { set; get; }
        public string CITIZENIDADDRESS { set; get; }
        public string HEIGHT { set; get; }
        public string WEIGHT { set; get; }
        public string PERMANENTADDRESS { set; get; }
        public string HOMETOWN { set; get; }
        public string FAMILYMEMBERINFOR { set; get; }
        public string WITNESSERATOLDCOMPANY { set; get; }
        public string APPLYPOSITIONINFOR { set; get; }
        public string RECRUITINFORCHANNEL { set; get; }
        public string REFERRERINFOR { set; get; }
        public string COMMITMENT { set; get; }
        public string EMERCONTACTINFOR { set; get; }
        public string ADDRESSAFTERWORK { set; get; }
        public string FRIENDINFORAFTERWORK { set; get; }
        public string INSURANCEBOOKINFOR { set; get; }
        public string BIRTHDAY { set; get; }
        public string TRANSPORTMETHOD { set; get; }
        public string PLACEBIRTH { set; get; }
        public string JOB_MAIL { set; get; }
        public string JOB_CV_DETAIL { set; get; }
        public string JOB_ID { set; get; }
        public string JOB_NAME { set; get; }
        public string CV_TEMP_FILE { set; get; }
        /// <summary>
        /// Trang thai don ky trong luu trinh cua HR
        /// </summary>
        public string RESULT { set; get; }


        


    }



    public class TB_TP_WORKINGLIFE_TP
    {
        public string ID { set; get; }
        public string TPID { set; get; }
        public string BEGINTIME { set; get; }
        public string ENDTIME { set; get; }
        public string COMPANY { set; get; }
        public string POSITION { set; get; }
        public string RETERENCE { set; get; }
        public string LANGUAGE { set; get; }
        public string PLANT { set; get; }
        public string WORKDESCRIPTION { set; get; }
        public string SCALE { set; get; }
        public string PROPERTY { set; get; }
        public string TRADE { set; get; }
        public string CREATE_TIME { set; get; }
        public string CREATE_EMP { set; get; }
        public string NATIONOWNCOMPANY { set; get; }
        public string REASONLEAVE { set; get; }
        public string TIMELEAVE { set; get; }
    }



    public class TB_TP_EDUCATION_TP
    {
        public string ID { set; get; }
        public string TPID { set; get; }
        public string BEGINTIME { set; get; }
        public string ENDTIME { set; get; }
        public string SCHOOL { set; get; }
        public string MAJOR { set; get; }
        public string QUALIFICATION { set; get; }
        public string LANGUAGE { set; get; }
        public string PLANT { set; get; }
        public string CREATE_TIME { set; get; }
        public string CREATE_EMP { set; get; }
        public string EDUCATION_FORM { set; get; }
        public string BACHELOR_SCIENCE { set; get; }
    }

    public class TB_TP_PROJECT_EXPERIENCE
    {
        public string ROW_ID { set; get; }
        public string TP_ID { set; get; }
        public string COMPANY_NAME { set; get; }
        public string BEGIN_DATE { set; get; }
        public string END_DATE { set; get; }
        public string EXPERIENCE_DETAIL { set; get; }
        public string CREATE_EMP { set; get; }
        public string CREATE_DATE { set; get; }
        public string ASSESSMENTTRAININGITEM { set; get; }
        public string EVALUATIONTRAININGUNIT { set; get; }
        public string CERTIFICATENAME { set; get; }
    }
}