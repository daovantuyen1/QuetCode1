using Dapper;
using Oracle.ManagedDataAccess.Client;
using RMSExternalApi.Commons;
using RMSExternalApi.Commons.DB;
using RMSExternalApi.DTO;
using RMSExternalApi.DTO.RMS;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Businesses
{
    public class RMSCVExternalJobBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSCVExternalJobBusiness() { }
        private static RMSCVExternalJobBusiness _instance;
        public static RMSCVExternalJobBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSCVExternalJobBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Manage CV for External job

        public bool AddCVForExternalJob(CVForExternalJob cv, ref string TPID)
        {
            string strsql = @"select node_value from C_NODE_VALUE where FATHER_NODE = 'TpPrefix' and NODE_NAME = :plant";
            string prefix = "ERC"; // External recuirement
            strsql = "select nvl(lpad(max(substr(tpid,12,5)) + 1,5,0),'00001') from tb_tp_talentpool where to_char(createdate, 'yyyy/mm/dd') = to_char(sysdate,'yyyy/mm/dd')";
            cv.TPID = prefix + DateTime.Now.ToString("yyyyMMdd") + DBHelper.getRMSDBConnectObj().QueryFirst<string>(strsql, null);
            TPID = cv.TPID;
            //DOMICILE: dia chi cu tru, 
            //SALARYEM: thu nhap thang mong muon 
            // POSITIONE: Vị trí mục tiêu 
            //ONBOARDGAP:  Thời gian có thể làm việc 
            // EDITSTATUS: -1 
            //  MARK = 0;
            //  IsFail = 0;
            //      JOB_MAIL(DE BIET CV CUA AI)
            //JOB_CV_DETAIL: 'BASE 64 OF IMG'
            // JOB_ID: '',// MA ID CUA JOB CAN UNG TUYEN
            // JOB_NAME: '', //TEN JOB CAN UNG TUYEN

            //未婚:chưa kết hôn
            //  已婚: Đã cưới
            using (var cnn = DBHelper.getRMSDBConnectObj())
            {
                OracleTransaction trs = null;
                try
                {

                    cnn.Open();
                    trs = cnn.BeginTransaction();
                    strsql = $@"
                     INSERT INTO TB_TP_TALENTPOOL(TPID,NAME,SEX,AGE,MOBILE,MARRIED,EMAIL,DOMICILE,SALARYEM,
                     POSITIONE,ONBOARDGAP,CREATEBY,CREATEDATE,EDITSTATUS,MARK,ISFAIL,CITIZENID,JOB_MAIL,JOB_ID,JOB_NAME, BIRTHDAY,SKILLS) 
                     VALUES (:TPID,:NAME,:SEX,:AGE,:MOBILE,:MARRIED,:EMAIL,:DOMICILE,:SALARYEM,
                     :POSITIONE,:ONBOARDGAP,:CREATEBY,SYSDATE,:EDITSTATUS,:MARK,:ISFAIL,:CITIZENID,:JOB_MAIL,:JOB_ID,:JOB_NAME,:BIRTHDAY,:SKILLS)
                  ";
                    cnn.Execute(strsql, new
                    {
                        TPID = cv.TPID + "",
                        NAME = cv.name?.Trim() + "",
                        SEX = cv.gender?.Trim() + "",
                        AGE = Util.ConvertDaysToApproximateYears((DateTime.Now - DateTime.ParseExact(cv.birthday?.Replace("-", "/"), "yyyy/MM/dd", new CultureInfo("en-US"))).Days) + "",
                        MOBILE = cv.mobile?.Trim()?.ToLower() + "",
                        MARRIED = cv.married == "Y" ? "已婚" : "未婚",
                        EMAIL = cv.mail?.Trim()?.ToLower() + "",
                        DOMICILE = cv.address?.Trim() + "",
                        SALARYEM = cv.monthlySalaryWish?.Trim() + "",
                        POSITIONE = cv.positionWish?.Trim() + "",
                        ONBOARDGAP = cv.positionDate?.Trim() + "",
                        CREATEBY = cv.jobMail?.Trim()?.ToLower() + "",
                        EDITSTATUS = -1,
                        MARK = 0,
                        ISFAIL = 0,
                        CITIZENID = cv.citizenId?.Trim() + "",
                        JOB_MAIL = cv.jobMail?.Trim()?.ToLower() + "",
                        JOB_ID = cv.jobID?.Trim() + "",
                        JOB_NAME = cv.jobName?.Trim() + "",
                        BIRTHDAY = cv.birthday?.Trim()?.Replace("-", "/") + "",
                        SKILLS = cv?.skills?.Trim() + "",

                    }, trs);


                    //Qua trinh hoc tap
                    if (cv.educations?.Count > 0)
                    {
                        foreach (var t in cv.educations)
                        {
                            strsql = $@"insert into  tb_tp_education_tp(id,tpid,begintime,endtime,SCHOOL,MAJOR,QUALIFICATION,Create_Emp,Create_Time)
                                    values(GET_ROW_ID,:Tpid,:Begintime,:Endtime,:School,:Major,:Qualification,:Create_Emp,SYSDATE)";
                            cnn.Execute(strsql, new
                            {
                                Tpid = cv.TPID + "",
                                Begintime = t.startTime?.Trim()?.Replace("/", "-") + "",
                                Endtime = t.endTime?.Trim()?.Replace("/", "-") + "",
                                School = t.school?.Trim() + "",
                                Major = t.major?.Trim() + "",
                                Qualification = t.eduQualify?.Trim() + "",
                                Create_Emp = cv.jobMail?.Trim()?.ToLower() + "",
                            }, trs);
                        }
                    }

                    // kinh nghiem lam viec
                    if (cv.jobExperiences?.Count > 0)
                    {
                        foreach (var t in cv.jobExperiences)
                        {

                            strsql = $@"insert into tb_tp_workinglife_tp(id,tpid,begintime,endtime,COMPANY,POSITION,RETERENCE,WORKDESCRIPTION,SCALE,PROPERTY,TRADE,Create_Emp,Create_Time)
                                    values(GET_ROW_ID,:Tpid,:Begintime,:Endtime,:Company,:Position,:ReteRence,:WorkDescription,:Scale,:Property,:Trade,:Create_Emp,SYSDATE)";
                            cnn.Execute(strsql, new
                            {
                                Tpid = cv.TPID + "",
                                Begintime = t.startTime?.Trim()?.Replace("/", "-") + "",
                                Endtime = t.endTime?.Trim()?.Replace("/", "-") + "",
                                Company = t.company?.Trim() + "",
                                Position = t.position?.Trim() + "",
                                ReteRence = "",
                                WorkDescription = t.description?.Trim() + "",
                                Scale = "",
                                Property = "",
                                Trade = "",
                                Create_Emp = cv.jobMail?.Trim()?.ToLower() + "",

                            }, trs);
                        }
                    }


                    // kinh nghiem du an
                    if (cv.projectExperiences.Count > 0)
                    {
                        foreach (var t in cv.projectExperiences)
                        {

                            strsql = @"insert into TB_TP_PROJECT_EXPERIENCE(ROW_ID,TP_ID,COMPANY_NAME,BEGIN_DATE,END_DATE,EXPERIENCE_DETAIL,CREATE_EMP,CREATE_DATE)
                           values(GET_ROW_ID,:TPID,:COMPANYNAME,:BEGINDATE,:ENDDATE,:EXPERIENCEDETAIL,:CREATEEMP,SYSDATE)";
                            cnn.Execute(strsql, new
                            {
                                TPID = cv.TPID + "",
                                COMPANYNAME = t.name?.Trim() + "",
                                BEGINDATE = t.startTime?.Trim()?.Replace("/", "-") + "",
                                ENDDATE = t.endTime?.Trim()?.Replace("/", "-") + "",
                                EXPERIENCEDETAIL = t.description?.Trim() + "",
                                CREATEEMP = cv.jobMail?.Trim()?.ToLower() + "",
                            }, trs);
                        }
                    }


                    strsql = $@"update tb_tp_talentpool set combine = tpid || ';' ||
                               name || ';' ||
                               decode(sex, 'M', '男', '女') || ';' ||
                               age || ';' ||
                               positione || ';' ||
                               (select qualification || ';' || SCHOOL || ';' || major from tb_tp_education_tp
                                where tpid = :tpid and begintime = (select max(begintime) from tb_tp_education_tp where tpid = :tpid and rownum = 1)and rownum = 1) || ';' ||
                                ENGLISHLEVEL || ';' ||
                                workinglife || ';' ||
                                TPSTATUSID || ';' ||
                                SALARYEM || ';' ||
                                GET_EMP_NAME(rcom) || ';' || RESUMELABEL
                               where tpid = :tpid
                   ";
                    cnn.Execute(strsql, new { TPID = cv.TPID + "" }, trs);
                    trs.Commit();
                    return true;
                }
                catch (Exception ex)
                {

                    try
                    {
                        if (trs != null)
                            trs.Rollback();
                    }
                    catch
                    {
                    }
                    throw ex;
                   
                }

            }
            return false;

        }

        public bool UpdateCVForExternalJob(CVForExternalJob cv)
        {

            using (var cnn = DBHelper.getRMSDBConnectObj())
            {
                OracleTransaction trs = null;
                try
                {
                    cnn.Open();
                    trs = cnn.BeginTransaction();
                    //editStatus : -1
                    //STATUSDETAIL="",
                    // TPSTATUSID="",

                    string strsql = $@"
                      update tb_tp_talentpool set NAME = :NAME,SEX = :SEX,AGE = :AGE,
                      MOBILE = :MOBILE,MARRIED = :MARRIED,
                      EMAIL = :EMAIL,
                      DOMICILE = :DOMICILE,SALARYEM = :SALARYEM,
                      POSITIONE = :POSITIONE,
                      ONBOARDGAP = :ONBOARDGAP,
                      TPSTATUSID =:TPSTATUSID,
                      STATUSDETAIL = :STATUSDETAIL,UPDATEBY = :UPDATEBY,UPDATEDATE = sysdate,EDITSTATUS = :EDITSTATUS,
                      CITIZENID = :CITIZENID ,
                      BIRTHDAY = :BIRTHDAY ,
                      SKILLS = :SKILLS
                      where tpid = :TPID AND  ISDELETE = 0  ";
                    cnn.Execute(strsql, new
                    {

                        TPID = cv.TPID + "",
                        NAME = cv.name?.Trim() + "",
                        SEX = cv.gender?.Trim() + "",
                        AGE = Util.ConvertDaysToApproximateYears((DateTime.Now - DateTime.ParseExact(cv.birthday?.Replace("-", "/"), "yyyy/MM/dd", new CultureInfo("en-US"))).Days) + "",
                        MOBILE = cv.mobile?.Trim()?.ToLower() + "",
                        MARRIED = cv.married == "Y" ? "已婚" : "未婚",
                        EMAIL = cv.mail?.Trim()?.ToLower() + "",
                        DOMICILE = cv.address?.Trim() + "",
                        SALARYEM = cv.monthlySalaryWish?.Trim() + "",
                        POSITIONE = cv.positionWish?.Trim() + "",
                        ONBOARDGAP = cv.positionDate?.Trim() + "",
                        CREATEBY = cv.jobMail?.Trim()?.ToLower() + "",
                        EDITSTATUS = -1,
                        TPSTATUSID = "",
                        STATUSDETAIL = "",
                        UPDATEBY = cv.jobMail?.Trim()?.ToLower() + "",
                        CITIZENID = cv.citizenId?.Trim() + "",
                        BIRTHDAY = cv.birthday?.Trim()?.Replace("-", "/") + "",
                        SKILLS = cv?.skills?.Trim() + "",
                    }, trs);


                    // kinh nghiem lam viec
                    strsql = $@"delete from tb_tp_workinglife_tp where tpid = :tpid";
                    cnn.Execute(strsql, new { tpid = cv.TPID + "" }, trs);

                    if (cv.jobExperiences?.Count > 0)
                    {
                        foreach (var t in cv.jobExperiences)
                        {

                            strsql = $@"insert into tb_tp_workinglife_tp(id,tpid,begintime,endtime,COMPANY,POSITION,RETERENCE,WORKDESCRIPTION,SCALE,PROPERTY,TRADE,Create_Emp,Create_Time)
                                    values(GET_ROW_ID,:Tpid,:Begintime,:Endtime,:Company,:Position,:ReteRence,:WorkDescription,:Scale,:Property,:Trade,:Create_Emp,SYSDATE)";
                            cnn.Execute(strsql, new
                            {
                                Tpid = cv.TPID + "",
                                Begintime = t.startTime?.Trim()?.Replace("/", "-") + "",
                                Endtime = t.endTime?.Trim()?.Replace("/", "-") + "",
                                Company = t.company?.Trim() + "",
                                Position = t.position?.Trim() + "",
                                ReteRence = "",
                                WorkDescription = t.description?.Trim() + "",
                                Scale = "",
                                Property = "",
                                Trade = "",
                                Create_Emp = cv.jobMail?.Trim()?.ToLower() + "",

                            }, trs);
                        }
                    }



                    //qua trinh hoc tap
                    strsql = $@"delete from  tb_tp_education_tp where tpid = :tpid";
                    cnn.Execute(strsql, new { tpid = cv.TPID + "" }, trs);

                    if (cv.educations?.Count > 0)
                    {
                        foreach (var t in cv.educations)
                        {
                            strsql = $@"insert into  tb_tp_education_tp(id,tpid,begintime,endtime,SCHOOL,MAJOR,QUALIFICATION,Create_Emp,Create_Time)
                                    values(GET_ROW_ID,:Tpid,:Begintime,:Endtime,:School,:Major,:Qualification,:Create_Emp,SYSDATE)";
                            cnn.Execute(strsql, new
                            {
                                Tpid = cv.TPID + "",
                                Begintime = t.startTime?.Trim()?.Replace("/", "-") + "",
                                Endtime = t.endTime?.Trim()?.Replace("/", "-") + "",
                                School = t.school?.Trim() + "",
                                Major = t.major?.Trim() + "",
                                Qualification = t.eduQualify?.Trim() + "",
                                Create_Emp = cv.jobMail?.Trim()?.ToLower() + "",
                            }, trs);
                        }
                    }



                    //update項目經驗 刪掉，重新插
                    strsql = $@"delete from  TB_TP_PROJECT_EXPERIENCE where tp_id = :tpid";
                    cnn.Execute(strsql, new { tpid = cv.TPID + "" }, trs);
                    if (cv.projectExperiences.Count > 0)
                    {
                        foreach (var t in cv.projectExperiences)
                        {

                            strsql = @"insert into TB_TP_PROJECT_EXPERIENCE(ROW_ID,TP_ID,COMPANY_NAME,BEGIN_DATE,END_DATE,EXPERIENCE_DETAIL,CREATE_EMP,CREATE_DATE)
                           values(GET_ROW_ID,:TPID,:COMPANYNAME,:BEGINDATE,:ENDDATE,:EXPERIENCEDETAIL,:CREATEEMP,SYSDATE)";
                            cnn.Execute(strsql, new
                            {
                                TPID = cv.TPID + "",
                                COMPANYNAME = t.name?.Trim() + "",
                                BEGINDATE = t.startTime?.Trim()?.Replace("/", "-") + "",
                                ENDDATE = t.endTime?.Trim()?.Replace("/", "-") + "",
                                EXPERIENCEDETAIL = t.description?.Trim() + "",
                                CREATEEMP = cv.jobMail?.Trim()?.ToLower() + "",
                            }, trs);
                        }
                    }



                    strsql = $@"update tb_tp_talentpool set combine = tpid || ';' ||
                     name || ';' ||
                     decode(sex, 'M', '男', '女') || ';' ||
                     age || ';' ||
                     positione || ';' ||
                     (select qualification || ';' || SCHOOL || ';' || major from tb_tp_education_tp
                      where tpid = :tpid and begintime = (select max(begintime) from tb_tp_education_tp where tpid = :tpid and rownum = 1)and rownum = 1) || ';' ||
                      ENGLISHLEVEL || ';' ||
                      workinglife || ';' ||
                      TPSTATUSID || ';' ||
                      SALARYEM || ';' ||
                      GET_EMP_NAME(rcom) || ';' || RESUMELABEL
                     where tpid = :tpid
            ";

                    cnn.Execute(strsql, new { tpid = cv.TPID + "" }, trs);
                    trs.Commit();
                    return true;


                }
                catch (Exception ex)
                {
                    try
                    {
                        if (trs != null)
                            trs.Rollback();
                    }
                    catch
                    {
                    }
                    throw ex;
                }
                return false;

            }




        }

        public bool DeleteCVForExternalJob(string tpID)
        {

            using (var cnn = DBHelper.getRMSDBConnectObj())
            {
                OracleTransaction trs = null;
                try
                {
                    cnn.Open();
                    trs = cnn.BeginTransaction();
                    cnn.Execute(@" DELETE FROM TB_TP_TALENTPOOL  WHERE TPID = :TPID ", new { TPID = tpID?.Trim() + "" }, trs);
                    cnn.Execute($@"delete from tb_tp_workinglife_tp where tpid = :tpid", new { tpid = tpID?.Trim() + "" }, trs);
                    cnn.Execute($@"delete from  tb_tp_education_tp where tpid = :tpid", new { tpid = tpID?.Trim() + "" }, trs);
                    cnn.Execute($@"delete from  TB_TP_PROJECT_EXPERIENCE where tp_id = :tpid", new { tpid = tpID?.Trim() + "" }, trs);
                    trs.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (trs != null)
                            trs.Rollback();
                    }
                    catch
                    {
                    }
                    throw ex;
                }
                return false;
            }
        }


        /// <summary>
        /// Get result history of TPID of External job
        /// </summary>
        /// <param name="TPID"></param>
        /// <returns></returns>
        public List<ResultHistory> GetResultHistoryOfTPID(string TPID)
        {
            var imNoObj = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(
           @"
            SELECT 
            IMNO ,TPID , GET_STATUS(RESULT) AS RESULT 
            FROM  TB_IM_INTERVIEW 
            WHERE TPID = :TPID ORDER BY ADDEDTIME DESC
            ", new { TPID = TPID?.Trim() + "" });

            var resultLs = DBHelper.getRMSDBConnectObj().Query<TB_IM_INTERVIEW_RESULT_HISTORY>(
              @"
            SELECT 
            IMNO, TPID, RESULT, RESULT_TEXT, 
            TO_CHAR(CREATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS CREATE_DATE
            FROM 
            TB_IM_INTERVIEW_RESULT_HISTORY 
            WHERE IMNO = :IMNO ORDER BY  CREATE_DATE ASC
            ", new { IMNO = imNoObj?.IMNO + "" })?.ToList();

            return resultLs?.Select(r => new ResultHistory { result = r.RESULT_TEXT, createDate = r.CREATE_DATE })?.ToList();

        }

        /// <summary>
        /// Get list interview history result of external job
        /// </summary>
        /// <param name="TPID"></param>
        /// <returns></returns>
        public List<AmProcess> GetAmProcessLsOfTPID(string TPID)
        {
            var imNoObj = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(
            @"
            SELECT 
            IMNO ,TPID , GET_STATUS(RESULT) AS RESULT 
            FROM  TB_IM_INTERVIEW 
            WHERE TPID = :TPID ORDER BY ADDEDTIME DESC
            ", new { TPID = TPID?.Trim() + "" });

            var amProcessLs = DBHelper.getRMSDBConnectObj().Query<TB_AM_PROCESS>(
                @"
                SELECT 
                 ID, IM_NO, ROLE, EMP_NO, NAME, RESULT,
                 TO_CHAR(CREATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS CREATE_DATE,
                 SIGN_COMMENT
                FROM TB_AM_PROCESS  WHERE   IM_NO = :IM_NO ORDER BY CREATE_DATE ASC
                ", new { IM_NO = imNoObj?.IMNO + "" })?.ToList();

            return amProcessLs?.Select(r => new AmProcess { result = r.RESULT, createDate = r.CREATE_DATE })?.ToList();

        }

        /// <summary>
        /// Get trang thai ma dau don phong van tuong ung voi ma don nhan tai TPID
        /// </summary>
        /// <param name="TPID"></param>
        /// <returns></returns>
        public string GetStatusOfImNoRefTPID(string TPID)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(
            @"
            SELECT 
            IMNO ,TPID , GET_STATUS(RESULT) AS RESULT 
            FROM  TB_IM_INTERVIEW 
            WHERE TPID = :TPID ORDER BY ADDEDTIME DESC
            ", new { TPID = TPID?.Trim() + "" })?.RESULT;

        }


        // jobID, jobMail
        public TB_TP_TALENTPOOL GetTbTpTalentPoolByJobIDJobMail(string jobId, string jobMail)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_TP_TALENTPOOL>(
              @"
            SELECT 
            TPID, NAME, SEX, AGE, ENGLISHNAME, NATIONALITY, PASSPORT, MOBILE, MARRIED, ENGLISHLEVEL, WORKINGLIFE, EMAIL, OTHERFL, SALARYCM, SALARYCY, DOMICILE, SALARYEM, SALARYEY, POSITIONE, SITEE, HUNTING, ONBOARDGAP, SKILLS, SOURCEID, RESUMECOMMENT, MAILINGDATE, RCOM, TPSTATUSID, STATUSDETAIL, CREATEBY, 
            TO_CHAR(CREATEDATE,'YYYY/MM/DD HH24:MI:SS') AS CREATEDATE, UPDATEBY, TO_CHAR(UPDATEDATE,'YYYY/MM/DD HH24:MI:SS' ) AS  UPDATEDATE, DEMANDNO, MARK, EDITSTATUS, LANGUAGE, PLANT, COMBINE, ISDELETE, ISFAIL, LASTIMTIME, RESUMELABEL, AVATAR_IMG, VIETNAMESENAME, ETHNIC, CITIZENIDOLD, CITIZENID, CITIZENIDDATE, CITIZENIDADDRESS, HEIGHT, WEIGHT, PERMANENTADDRESS, HOMETOWN, FAMILYMEMBERINFOR, WITNESSERATOLDCOMPANY, APPLYPOSITIONINFOR, RECRUITINFORCHANNEL, REFERRERINFOR, COMMITMENT, EMERCONTACTINFOR, ADDRESSAFTERWORK, FRIENDINFORAFTERWORK, INSURANCEBOOKINFOR, BIRTHDAY, TRANSPORTMETHOD, PLACEBIRTH, JOB_MAIL, JOB_CV_DETAIL, JOB_ID, JOB_NAME
            FROM TB_TP_TALENTPOOL WHERE JOB_ID = :JOB_ID AND LOWER(JOB_MAIL) = :JOB_MAIL    
            ", new { JOB_ID = jobId + "", JOB_MAIL = jobMail?.Trim()?.ToLower() + "", });
        }


        /// <summary>
        /// Get Infor Cv for external job
        /// </summary>
        /// <param name="tpID"></param>
        /// <returns></returns>
        public TB_TP_TALENTPOOL GetTbTpTalentPool(string tpID)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_TP_TALENTPOOL>(
              @"
            SELECT 
            TPID, NAME, SEX, AGE, ENGLISHNAME, NATIONALITY, PASSPORT, MOBILE, MARRIED, ENGLISHLEVEL, WORKINGLIFE, EMAIL, OTHERFL, SALARYCM, SALARYCY, DOMICILE, SALARYEM, SALARYEY, POSITIONE, SITEE, HUNTING, ONBOARDGAP, SKILLS, SOURCEID, RESUMECOMMENT, MAILINGDATE, RCOM, TPSTATUSID, STATUSDETAIL, CREATEBY, 
            TO_CHAR(CREATEDATE,'YYYY/MM/DD HH24:MI:SS') AS CREATEDATE, UPDATEBY, TO_CHAR(UPDATEDATE,'YYYY/MM/DD HH24:MI:SS' ) AS  UPDATEDATE, DEMANDNO, MARK, EDITSTATUS, LANGUAGE, PLANT, COMBINE, ISDELETE, ISFAIL, LASTIMTIME, RESUMELABEL, AVATAR_IMG, VIETNAMESENAME, ETHNIC, CITIZENIDOLD, CITIZENID, CITIZENIDDATE, CITIZENIDADDRESS, HEIGHT, WEIGHT, PERMANENTADDRESS, HOMETOWN, FAMILYMEMBERINFOR, WITNESSERATOLDCOMPANY, APPLYPOSITIONINFOR, RECRUITINFORCHANNEL, REFERRERINFOR, COMMITMENT, EMERCONTACTINFOR, ADDRESSAFTERWORK, FRIENDINFORAFTERWORK, INSURANCEBOOKINFOR, BIRTHDAY, TRANSPORTMETHOD, PLACEBIRTH, JOB_MAIL, JOB_CV_DETAIL, JOB_ID, JOB_NAME , CV_TEMP_FILE
            FROM TB_TP_TALENTPOOL WHERE TPID = :TPID
            ", new { TPID = tpID?.Trim() + "" });
        }


        public TB_TP_TALENTPOOL GetTbTpTalentPoolByCvTempFileId(string cvTempFileId)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_TP_TALENTPOOL>(
              @"
            SELECT 
            TPID, NAME, SEX, AGE, ENGLISHNAME, NATIONALITY, PASSPORT, MOBILE, MARRIED, ENGLISHLEVEL, WORKINGLIFE, EMAIL, OTHERFL, SALARYCM, SALARYCY, DOMICILE, SALARYEM, SALARYEY, POSITIONE, SITEE, HUNTING, ONBOARDGAP, SKILLS, SOURCEID, RESUMECOMMENT, MAILINGDATE, RCOM, TPSTATUSID, STATUSDETAIL, CREATEBY, 
            TO_CHAR(CREATEDATE,'YYYY/MM/DD HH24:MI:SS') AS CREATEDATE, UPDATEBY, TO_CHAR(UPDATEDATE,'YYYY/MM/DD HH24:MI:SS' ) AS  UPDATEDATE, DEMANDNO, MARK, EDITSTATUS, LANGUAGE, PLANT, COMBINE, ISDELETE, ISFAIL, LASTIMTIME, RESUMELABEL, AVATAR_IMG, VIETNAMESENAME, ETHNIC, CITIZENIDOLD, CITIZENID, CITIZENIDDATE, CITIZENIDADDRESS, HEIGHT, WEIGHT, PERMANENTADDRESS, HOMETOWN, FAMILYMEMBERINFOR, WITNESSERATOLDCOMPANY, APPLYPOSITIONINFOR, RECRUITINFORCHANNEL, REFERRERINFOR, COMMITMENT, EMERCONTACTINFOR, ADDRESSAFTERWORK, FRIENDINFORAFTERWORK, INSURANCEBOOKINFOR, BIRTHDAY, TRANSPORTMETHOD, PLACEBIRTH, JOB_MAIL, JOB_CV_DETAIL, JOB_ID, JOB_NAME , CV_TEMP_FILE
            FROM TB_TP_TALENTPOOL WHERE CV_TEMP_FILE = :CV_TEMP_FILE
            ", new { CV_TEMP_FILE = cvTempFileId?.Trim() + "" });
        }


        /// <summary>
        /// Get job experience
        /// </summary>
        /// <param name="tpID"></param>
        /// <returns></returns>
        public List<TB_TP_WORKINGLIFE_TP> GetTbTpWorkingLife(string tpID)
        {
            return DBHelper.getRMSDBConnectObj().Query<TB_TP_WORKINGLIFE_TP>(
           @"
             select 
                ID, TPID, BEGINTIME, ENDTIME, COMPANY, POSITION, RETERENCE, LANGUAGE, PLANT, WORKDESCRIPTION, SCALE, PROPERTY, TRADE, 
                TO_CHAR(CREATE_TIME,'YYYY/MM/DD HH24:MI:SS') AS CREATE_TIME, CREATE_EMP, NATIONOWNCOMPANY, REASONLEAVE, TIMELEAVE
                from tb_tp_workinglife_tp  where tpid= :TPID ORDER BY ID ASC 
                ", new { TPID = tpID?.Trim() + "" }).ToList();

        }

        /// <summary>
        /// Get education process
        /// </summary>
        /// <param name="tpID"></param>
        /// <returns></returns>
        public List<TB_TP_EDUCATION_TP> GetTbTpEduction(string tpID)
        {
            return DBHelper.getRMSDBConnectObj().Query<TB_TP_EDUCATION_TP>(
           @"
            SELECT 
                ID, TPID, BEGINTIME, ENDTIME, SCHOOL, MAJOR, QUALIFICATION, LANGUAGE, PLANT,
                TO_CHAR(CREATE_TIME,'YYYY/MM/DD HH24:MI:SS') AS CREATE_TIME, CREATE_EMP, EDUCATION_FORM, BACHELOR_SCIENCE
                FROM TB_TP_EDUCATION_TP   WHERE TPID= :TPID  ORDER BY ID ASC
                ", new { TPID = tpID?.Trim() + "" }).ToList();

        }



        /// <summary>
        /// Get project experience
        /// </summary>
        /// <param name="tpID"></param>
        /// <returns></returns>
        public List<TB_TP_PROJECT_EXPERIENCE> GetTbTpProjectExperience(string tpID)
        {
            return DBHelper.getRMSDBConnectObj().Query<TB_TP_PROJECT_EXPERIENCE>(
           @"
                SELECT
                ROW_ID, TP_ID, COMPANY_NAME, BEGIN_DATE, END_DATE, EXPERIENCE_DETAIL, CREATE_EMP,
                TO_CHAR(CREATE_DATE,'YYYY/MM/DD HH24:MI:SS')  AS CREATE_DATE, ASSESSMENTTRAININGITEM, EVALUATIONTRAININGUNIT, CERTIFICATENAME
                FROM TB_TP_PROJECT_EXPERIENCE WHERE TP_ID=  :TPID  ORDER BY ROW_ID ASC           
                 ", new { TPID = tpID?.Trim() + "" }).ToList();

        }




        public CVForExternalJob GetCVForExternalJob(string tpID)
        {
            var talentPool = GetTbTpTalentPool(tpID);
            var workingLife = GetTbTpWorkingLife(tpID);
            var educations = GetTbTpEduction(tpID);
            var projetEx = GetTbTpProjectExperience(tpID);
            var fileAttached = GetFileAttachInforOfExternalJobMovedToServerFile(tpID);
            var tempstatus = GetStatusOfImNoRefTPID(tpID);
            var amProcess = GetAmProcessLsOfTPID(tpID);
            var resultHistories = GetResultHistoryOfTPID(tpID);

            var status = "";
            if (talentPool.ISDELETE == "1")
                status = "Deleted";
            else if (!string.IsNullOrWhiteSpace(tempstatus))
                status = tempstatus;
            else if (talentPool.EDITSTATUS == "-1" && talentPool.ISDELETE == "0")
                status = "Draff";
            else if (talentPool.EDITSTATUS == "0" && talentPool.ISDELETE == "0")
                status = "Submited and waiting";
               

            var total = new CVForExternalJob
            {
                TPID = talentPool.TPID,
                mail = talentPool.EMAIL,
                mobile = talentPool.MOBILE,
                name = talentPool.NAME,
                gender = talentPool.SEX,
                birthday = talentPool.BIRTHDAY,
                citizenId = talentPool.CITIZENID,
                married = talentPool.MARRIED == "已婚" ? "Y" : "N",//cv.married == "Y" ? "已婚" : "未婚"
                address = talentPool.DOMICILE,
                monthlySalaryWish = talentPool.SALARYEM,
                positionWish = talentPool.POSITIONE,
                positionDate = talentPool.ONBOARDGAP,
                tpStatusId = talentPool.TPSTATUSID,
                educations = educations?.Count <= 0 ? new List<Education>() : educations.Select(r => new Education { startTime = r.BEGINTIME, endTime = r.ENDTIME, school = r.SCHOOL, major = r.MAJOR, eduQualify = r.QUALIFICATION }).ToList(),
                projectExperiences = projetEx?.Count <= 0 ? new List<ProjectExperience>() : projetEx.Select(r => new ProjectExperience { startTime = r.BEGIN_DATE, endTime = r.END_DATE, name = r.COMPANY_NAME, description = r.EXPERIENCE_DETAIL }).ToList(),
                jobExperiences = workingLife?.Count <= 0 ? new List<JobExperience>() : workingLife.Select(r => new JobExperience { startTime = r.BEGINTIME, endTime = r.ENDTIME, company = r.COMPANY, position = r.POSITION, description = r.WORKDESCRIPTION }).ToList(),
                createDate = talentPool.CREATEDATE,
                updateDate = talentPool.UPDATEDATE,
                isFileCVAttachedTransfered = fileAttached != null ? true : false,
                jobID = talentPool.JOB_ID,
                jobName = talentPool.JOB_NAME,
                status = status,
                fileID = fileAttached != null ? fileAttached.FILEID : talentPool.CV_TEMP_FILE,
                skills = talentPool.SKILLS,
                amProcesses = amProcess,
                resultHistories = resultHistories,
                isAllowEdit = (talentPool.EDITSTATUS == "-1" && talentPool.ISDELETE == "0") ? true : false,


            };
            return total;

        }


        public bool UpdateCVTempFileNameForExternalJob(string TPID, string CVFileName, string jobMail)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(@"
                UPDATE TB_TP_TALENTPOOL 
                SET  CV_TEMP_FILE = :CV_TEMP_FILE ,
                    UPDATEBY = :UPDATEBY,
                    UPDATEDATE = SYSDATE
                WHERE 
                TPID = :TPID 
                AND LOWER(JOB_MAIL) = :JOB_MAIL
                ", new
            {
                CV_TEMP_FILE = CVFileName?.Trim() + "",
                UPDATEBY = jobMail?.Trim()?.ToLower() + "",
                TPID = TPID?.Trim() + "",
                JOB_MAIL = jobMail?.Trim()?.ToLower() + "",
            });
            return rs > 0 ? true : false;
        }


        /// <summary>
        /// Get thong tin file attach cua external job , ma file do da move sang server file
        /// </summary>
        /// <param name="TPID"></param>
        public TB_TP_ATTACHMENT GetFileAttachInforOfExternalJobMovedToServerFile(string TPID)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_TP_ATTACHMENT>(
                  @" select 
                    ATTID, TPID, FILEID, FILEPATH, CONTENTTYPE, 
                    TO_CHAR(CREATE_TIME , 'YYYY/MM/DD HH24:MI:SS')  AS  CREATE_TIME, CREATE_EMP
                    from TB_TP_ATTACHMENT WHERE TPID= :TPID
            ", new { TPID = TPID?.Trim() + "" });

        }


        public TB_TP_ATTACHMENT GetFileAttachInforOfExternalJobMovedToServerFileByFileID(string fileID)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_TP_ATTACHMENT>(
                  @" select 
                    ATTID, TPID, FILEID, FILEPATH, CONTENTTYPE, 
                    TO_CHAR(CREATE_TIME , 'YYYY/MM/DD HH24:MI:SS')  AS  CREATE_TIME, CREATE_EMP
                    from TB_TP_ATTACHMENT WHERE FILEID = :FILEID
            ", new { FILEID = fileID?.Trim() + "" });

        }




        /// <summary>
        /// Update file transfered to  Server file for External job success
        /// </summary>
        /// <param name="TPID"></param>
        /// <param name="FileId"></param>
        /// <param name="jobMail"></param>
        /// <returns></returns>

        public int UpdateFileTransferedToServerFileForExternalJob(string TPID, string FileId, string jobMail)
        {
            string strsql = $@"select count(1) as CC from tb_tp_attachment where tpid = :TPID";
            if (Convert.ToInt32(DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(strsql, new { TPID = TPID + "" })?.CC) == 0)
            {
                strsql = $@"insert into tb_tp_attachment(ATTID,TPID,FILEID,CREATE_TIME,CREATE_EMP) values(get_row_id,:TPID,:FILEID,sysdate,:CREATE_EMP)";
            }
            else
            {
                strsql = $@"update tb_tp_attachment set FILEID = :FILEID,CREATE_TIME = sysdate,CREATE_EMP = :CREATE_EMP where tpid = :TPID";
            }
            return DBHelper.getRMSDBConnectObj().Execute(strsql, new
            {
                TPID= TPID + "",
                FILEID = FileId + "",
                CREATE_EMP = jobMail + ""
            });
        }

        /// <summary>
        /// Delete infor of file attached moved to server file for External job 
        /// </summary>
        /// <param name="TPID"></param>
        /// <param name="FileID"></param>
        /// <returns></returns>

        public bool DeleteFileAttachInforOfExternalJobMovedToServerFile(string TPID, string FileID)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(
              "DELETE  FROM tb_tp_attachment WHERE  TPID = :TPID AND FILEID = :FILEID"
              , new { TPID = TPID?.Trim() + "", FILEID = FileID?.Trim() + "" });
            return rs > 0 ? true : false;
        }




        public List<TB_TP_TALENTPOOL> GetTbTpTalentPoolLsOFJobMail(string jobMail)
        {
            return DBHelper.getRMSDBConnectObj().Query<TB_TP_TALENTPOOL>(
              @"
            WITH STATUS_TB AS 
            (
                SELECT 
                B1.TPID , B1.IMNO , GET_STATUS(B1.RESULT) AS RESULT
                FROM (
                SELECT B.TPID,MAX(B.IMNO) AS IMNO  FROM  TB_IM_INTERVIEW B GROUP BY B.TPID
                )  A1, 
                TB_IM_INTERVIEW B1
                WHERE  A1.TPID = B1.TPID AND A1.IMNO =B1.IMNO  
            )
            SELECT 
            AA.TPID, AA.NAME, AA.SEX, AA.AGE, AA.ENGLISHNAME, AA.NATIONALITY, AA.PASSPORT, AA.MOBILE, AA.MARRIED, AA.ENGLISHLEVEL, AA.WORKINGLIFE, AA.EMAIL, AA.OTHERFL, AA.SALARYCM, AA.SALARYCY, AA.DOMICILE, AA.SALARYEM, AA.SALARYEY, AA.POSITIONE, AA.SITEE, AA.HUNTING, AA.ONBOARDGAP, AA.SKILLS, AA.SOURCEID, AA.RESUMECOMMENT,AA.MAILINGDATE, AA.RCOM, AA.TPSTATUSID, AA.STATUSDETAIL, AA.CREATEBY, 
            TO_CHAR(AA.CREATEDATE,'YYYY/MM/DD HH24:MI:SS') AS CREATEDATE, AA.UPDATEBY, TO_CHAR(AA.UPDATEDATE,'YYYY/MM/DD HH24:MI:SS' ) AS  UPDATEDATE, AA.DEMANDNO, AA.MARK, AA.EDITSTATUS, AA.LANGUAGE, AA.PLANT, AA.COMBINE, AA.ISDELETE, AA.ISFAIL, AA.LASTIMTIME, AA.RESUMELABEL, AA.AVATAR_IMG, AA.VIETNAMESENAME, AA.ETHNIC, AA.CITIZENIDOLD, AA.CITIZENID, AA.CITIZENIDDATE, AA.CITIZENIDADDRESS, AA.HEIGHT,AA.WEIGHT, AA.PERMANENTADDRESS, AA.HOMETOWN, AA.FAMILYMEMBERINFOR, AA.WITNESSERATOLDCOMPANY, AA.APPLYPOSITIONINFOR, AA.RECRUITINFORCHANNEL, AA.REFERRERINFOR, AA.COMMITMENT, AA.EMERCONTACTINFOR, AA.ADDRESSAFTERWORK, AA.FRIENDINFORAFTERWORK, AA.INSURANCEBOOKINFOR, AA.BIRTHDAY, AA.TRANSPORTMETHOD, AA.PLACEBIRTH, AA.JOB_MAIL, AA.JOB_CV_DETAIL, AA.JOB_ID, AA.JOB_NAME
            , BB.RESULT
            FROM TB_TP_TALENTPOOL AA 
            LEFT JOIN STATUS_TB BB
            ON AA.TPID= BB.TPID
            WHERE   LOWER(AA.JOB_MAIL) = :JOB_MAIL 
            ", new { JOB_MAIL = jobMail?.Trim()?.ToLower() + "", }).ToList();
        }


        /// <summary>
        /// Get list cv job for external job applied of job mail
        /// </summary>
        /// <param name="jobMail"></param>
        /// <returns></returns>
        public List<CVExternalJobBaseModel> GetCVExternalJobLsOfJobMail(string jobMail)
        {
            var cvJobLs = GetTbTpTalentPoolLsOFJobMail(jobMail);
            return cvJobLs?.Select(r => {


                var status = "";
                if (r.ISDELETE == "1")
                    status = "Deleted";
                else if (!string.IsNullOrWhiteSpace(r.RESULT))
                    status = r.RESULT;
                else if (r.EDITSTATUS == "-1" && r.ISDELETE == "0")
                    status = "Draff";
                else if (r.EDITSTATUS == "0" && r.ISDELETE == "0")
                    status = "Submited and waiting";

                return new CVExternalJobBaseModel
                {
                    tpID = r.TPID,
                    tpStatusId = r.TPSTATUSID,
                    createDate = r.CREATEDATE,
                    updateDate = r.UPDATEDATE,
                    jobMail = r.JOB_MAIL,
                    jobName = r.JOB_NAME,
                    jobID = r.JOB_ID,
                    status = status,
                    amProcesses = GetAmProcessLsOfTPID(r.TPID),
                    resultHistories = GetResultHistoryOfTPID(r.TPID),
                    isAllowEdit = (r.EDITSTATUS == "-1" && r.ISDELETE == "0") ? true : false,
                };
            }
            ).Where(r=>r.status!= "Deleted")?.OrderBy(r => r.createDate + "_" + r.tpStatusId)?.ToList();

        }



        /// <summary>
        /// Update status of TPID of external job into sign process
        /// </summary>
        /// <param name="TPID"></param>
        /// <param name="jobMail"></param>
        /// <returns></returns>
        public bool UpdateStatusOfCVExternalJobToSubmit(string TPID, string jobMail)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(
                  @"
                UPDATE 
                TB_TP_TALENTPOOL
                SET 
                 EDITSTATUS = 0, 
                 UPDATEDATE = SYSDATE ,
                 UPDATEBY= :UPDATEBY
                WHERE TPID = :TPID
                ", new
                  {
                      UPDATEBY = jobMail?.Trim()?.ToLower() + "",
                      TPID = TPID?.Trim() + "",
                  });
            return rs > 0 ? true : false;

        }
        #endregion Manage CV for External job


    }
}