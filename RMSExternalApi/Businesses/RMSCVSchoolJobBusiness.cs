using Dapper;
using Oracle.ManagedDataAccess.Client;
using RMSExternalApi.Commons;
using RMSExternalApi.Commons.DB;
using RMSExternalApi.DTO.RMS;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Businesses
{
    public class RMSCVSchoolJobBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSCVSchoolJobBusiness() { }
        private static RMSCVSchoolJobBusiness _instance;
        public static RMSCVSchoolJobBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSCVSchoolJobBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion



        /// <summary>
        /// tu ma trang thai don phong van sinh vien (  tb_cr_im_interview: IM_STATUS  ) 
        /// chuyen doi ma so do thanh text mo ta
        /// </summary>
        /// <param name="imStatus"></param>
        /// <returns></returns>
        public string GetImStatusInTextDesc(string imStatus)
        {

            //0 为待评价， 
            //1为评价不合格，
            //2为评价合格 ，
            //3为推荐他人,
            //4为HR提前淘汰，

            switch (imStatus)
            {
                case "0": return "为待评价";
                case "1": return "为评价不合格";
                case "2": return "为评价合格";
                case "3": return "为推荐他人";
                case "4": return "为HR提前淘汰";
                default: return "";
            }

        }

        /// <summary>
        /// Get Interview History Result of TPID
        /// </summary>
        /// <param name="TPID"></param>
        /// <returns></returns>
        public List<SchoolImProcess> GetImInterviewProcess(string TPID)
        {

            var imNoObj = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(
               @"
            SELECT IMNO, TPID  , IM_STATUS  FROM tb_cr_im_interview WHERE  TPID = :TPID  AND is_delete = 0  ORDER BY CREATE_TIME DESC
            ", new { TPID = TPID?.Trim() + "" });

            var processLs = DBHelper.getRMSDBConnectObj().Query<TB_CR_IM_PROCESS>(
             @"SELECT 
                ID, IMNO, ROLE, EMP_NO, NAME, RESULT, SIGN_COMMENT, TO_CHAR(CREATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS CREATE_DATE
                FROM TB_CR_IM_PROCESS where  IMNO = :IMNO ORDER BY CREATE_DATE ASC"
             , new { IMNO = imNoObj?.IMNO + "" }).ToList();

            return processLs?.Select(r => new SchoolImProcess { result = r.RESULT, createDate = r.CREATE_DATE })?.ToList();

        }

        /// <summary>
        /// Get trang thai cua Imno tuong ung voi TPID
        /// </summary>
        /// <param name="TPID"></param>
        /// <returns></returns>
        public string GetStatusOfImNoRefTPID(string TPID)
        {
            var statusCode = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(
              @"
            SELECT IMNO, TPID  , IM_STATUS  FROM tb_cr_im_interview WHERE  TPID = :TPID  AND is_delete = 0  ORDER BY CREATE_TIME DESC
            ", new { TPID = TPID?.Trim() + "" })?.IM_STATUS;
            
            var status1=  GetImStatusInTextDesc(statusCode);
            return  status1;
       
        }

        // jobID, jobMail
        public TB_CR_TP_TALENTPOOL GetTbCrTpTalentPoolByJobIDJobMail(string jobId, string jobMail)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_CR_TP_TALENTPOOL>(
              @"
            SELECT 
            TPID, YEAR, RECRUIT_TYPE, PSYCHOLOGY, QUALIFICATION, NAME, 
            DECODE(SEX,'男','M','F') AS  SEX, ID_NUMBER, SCHOOL, FIRST_MAJOR, MAJOR, PLANT, MOBILE, WAY, ENGLISH_LEVEL, SCORE, BUSINESS_GROUP, SPECIAL_TYPE, IS_ACCOUNT, AGE, HOMETOWN, SCHOOL_TYPE, ENGLISH_SCORE, SALARY, IS_MEMBER, EMAIL, FAMILY_PHONE, RCOM, FILE_ID, IS_DELETE,
            TO_CHAR(UPDATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS UPDATE_TIME, UPDATE_EMP, TO_CHAR(CREATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS  CREATE_TIME, CREATE_EMP, TP_STATUS, IS_THREEDIRECT, IS_TRANSCRIPT, IS_XEROX, IS_COLLECTINFO, IS_ENGLISHPROVED, IS_RECOMMANDFORM, REMARK, FAIL_REASON, FAIL_REMARK, DEGREE, GRADUATION_TIME, POSITION_NAME, ETHNIC, CCCD_DATE, CCCD_ADDRESS, BIRTH_ADDRESS, JOB_MAIL, JOB_CV_DETAIL, JOB_ID, JOB_NAME, CV_TEMP_FILE 
            ,F_POSITION_WISH ,BIRTHDAY
            FROM TB_CR_TP_TALENTPOOL WHERE  JOB_ID= :JOB_ID AND LOWER(JOB_MAIL) = :JOB_MAIL 
            ", new { JOB_ID = jobId + "", JOB_MAIL = jobMail?.Trim()?.ToLower() + "", });
        }

        /// <summary>
        /// Get Cv infor of School job
        /// </summary>
        /// <param name="tpID"></param>
        /// <returns></returns>
        public TB_CR_TP_TALENTPOOL GetTbCrTpTalentPool(string tpID)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_CR_TP_TALENTPOOL>(
              @"
           SELECT 
            TPID, YEAR, RECRUIT_TYPE, PSYCHOLOGY, QUALIFICATION, NAME,
            DECODE(SEX,'男','M','F') AS  SEX  , ID_NUMBER, SCHOOL, FIRST_MAJOR, MAJOR, PLANT, MOBILE, WAY, ENGLISH_LEVEL, SCORE, BUSINESS_GROUP, SPECIAL_TYPE, IS_ACCOUNT, AGE, HOMETOWN, SCHOOL_TYPE, ENGLISH_SCORE, SALARY, IS_MEMBER, EMAIL, FAMILY_PHONE, RCOM, FILE_ID, IS_DELETE,
            TO_CHAR(UPDATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS UPDATE_TIME, UPDATE_EMP, TO_CHAR(CREATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS  CREATE_TIME, CREATE_EMP, TP_STATUS, IS_THREEDIRECT, IS_TRANSCRIPT, IS_XEROX, IS_COLLECTINFO, IS_ENGLISHPROVED, IS_RECOMMANDFORM, REMARK, FAIL_REASON, FAIL_REMARK, DEGREE, GRADUATION_TIME, POSITION_NAME, ETHNIC, CCCD_DATE, CCCD_ADDRESS, BIRTH_ADDRESS, JOB_MAIL, JOB_CV_DETAIL, JOB_ID, JOB_NAME, CV_TEMP_FILE 
            ,F_POSITION_WISH , BIRTHDAY
            FROM TB_CR_TP_TALENTPOOL WHERE  TPID = :TPID
            ", new { TPID = tpID?.Trim() + "" });
        }

        public TB_CR_TP_TALENTPOOL GetTbCrTpTalentPoolByCVTemFile(string cvTempFile)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_CR_TP_TALENTPOOL>(
              @"
           SELECT 
            TPID, YEAR, RECRUIT_TYPE, PSYCHOLOGY, QUALIFICATION, NAME,
            DECODE(SEX,'男','M','F') AS  SEX  , ID_NUMBER, SCHOOL, FIRST_MAJOR, MAJOR, PLANT, MOBILE, WAY, ENGLISH_LEVEL, SCORE, BUSINESS_GROUP, SPECIAL_TYPE, IS_ACCOUNT, AGE, HOMETOWN, SCHOOL_TYPE, ENGLISH_SCORE, SALARY, IS_MEMBER, EMAIL, FAMILY_PHONE, RCOM, FILE_ID, IS_DELETE,
            TO_CHAR(UPDATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS UPDATE_TIME, UPDATE_EMP, TO_CHAR(CREATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS  CREATE_TIME, CREATE_EMP, TP_STATUS, IS_THREEDIRECT, IS_TRANSCRIPT, IS_XEROX, IS_COLLECTINFO, IS_ENGLISHPROVED, IS_RECOMMANDFORM, REMARK, FAIL_REASON, FAIL_REMARK, DEGREE, GRADUATION_TIME, POSITION_NAME, ETHNIC, CCCD_DATE, CCCD_ADDRESS, BIRTH_ADDRESS, JOB_MAIL, JOB_CV_DETAIL, JOB_ID, JOB_NAME, CV_TEMP_FILE 
            ,F_POSITION_WISH , BIRTHDAY
            FROM TB_CR_TP_TALENTPOOL WHERE  CV_TEMP_FILE = :CV_TEMP_FILE
            ", new { CV_TEMP_FILE = cvTempFile?.Trim() + "" });
        }


        public TB_CR_TP_TALENTPOOL GetTbCrTpTalentPoolByFileID(string fileID)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_CR_TP_TALENTPOOL>(
              @"
           SELECT 
            TPID, YEAR, RECRUIT_TYPE, PSYCHOLOGY, QUALIFICATION, NAME,
            DECODE(SEX,'男','M','F') AS  SEX  , ID_NUMBER, SCHOOL, FIRST_MAJOR, MAJOR, PLANT, MOBILE, WAY, ENGLISH_LEVEL, SCORE, BUSINESS_GROUP, SPECIAL_TYPE, IS_ACCOUNT, AGE, HOMETOWN, SCHOOL_TYPE, ENGLISH_SCORE, SALARY, IS_MEMBER, EMAIL, FAMILY_PHONE, RCOM, FILE_ID, IS_DELETE,
            TO_CHAR(UPDATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS UPDATE_TIME, UPDATE_EMP, TO_CHAR(CREATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS  CREATE_TIME, CREATE_EMP, TP_STATUS, IS_THREEDIRECT, IS_TRANSCRIPT, IS_XEROX, IS_COLLECTINFO, IS_ENGLISHPROVED, IS_RECOMMANDFORM, REMARK, FAIL_REASON, FAIL_REMARK, DEGREE, GRADUATION_TIME, POSITION_NAME, ETHNIC, CCCD_DATE, CCCD_ADDRESS, BIRTH_ADDRESS, JOB_MAIL, JOB_CV_DETAIL, JOB_ID, JOB_NAME, CV_TEMP_FILE 
            ,F_POSITION_WISH , BIRTHDAY
            FROM TB_CR_TP_TALENTPOOL WHERE  FILE_ID = :FILE_ID
            ", new { FILE_ID = fileID?.Trim() + "" });
        }


        /// <summary>
        /// Add cv job for school job
        /// </summary>
        /// <param name="cv"></param>
        /// <returns></returns>
        public bool AddCVForSchoolJob(CVForSchoolJob cv, ref string TPID)
        {

            string prefix = "SRC"; // external school recuirement
            string strSql = $@"select nvl(lpad(max(substr(tpid,12,4)) + 1,4,0),'00001') from TB_CR_TP_TALENTPOOL";
            string suffix = DBHelper.getRMSDBConnectObj().QueryFirst<string>(strSql, null);
            TPID = prefix + DateTime.Now.ToString("yyyyMMdd") + suffix;


            using (var cnn = DBHelper.getRMSDBConnectObj())
            {
                OracleTransaction trs = null;
                try
                {

                    cnn.Open();
                    trs = cnn.BeginTransaction();

                    // YEAR = Year of Now
                    // RECRUIT_TYPE = 外部招聘
                    // QUALIFICATION : trinh do Dai hoc/cao dang/trung cap
                    //ID_NUMBER: CCCD        citizenID
                    //SCHOOL : ten truong
                    //MAJOR: chuyen nganh
                    // BUSINESS_GROUP: nhom su nghiep
                    //  english_Level:    loai ngoai ngu biet
                    //english_Score:     so diem level cua ngoai ngu do
                    // Gender: M: 男: nam ,F: 女 : nu

                    string sqlStr = $@"insert into TB_CR_TP_TALENTPOOL(
                     TPID,QUALIFICATION,NAME,SEX,ID_NUMBER,SCHOOL,MAJOR,
                     MOBILE,CREATE_EMP,
                     IS_ACCOUNT,AGE,HOMETOWN,IS_MEMBER,EMAIL,
                     TP_STATUS,IS_DELETE,CREATE_TIME  , ETHNIC,
                      JOB_MAIL ,   JOB_ID ,  JOB_NAME , F_POSITION_WISH , BIRTHDAY , ENGLISH_LEVEL , ENGLISH_SCORE
                     ) values (
                     :TPID    , :QUALIFICATION ,
                     :NAME , :SEX , :ID_NUMBER , :SCHOOL , :MAJOR ,
                     :MOBILE  ,
                     :CREATE_EMP ,
                     '0', :AGE , :HOMETOWN  ,'0',
                     :EMAIL    ,
                     '待安排面试',0,sysdate  ,  :ETHNIC ,
                      :JOB_MAIL ,   :JOB_ID ,  :JOB_NAME , :F_POSITION_WISH , :BIRTHDAY , :ENGLISH_LEVEL , :ENGLISH_SCORE
                     )";


                    int rs = cnn.Execute(sqlStr, new
                    {
                        TPID = TPID + "",
                        QUALIFICATION = cv.qualification?.Trim() + "",
                        NAME = cv.name?.Trim() + "",
                        SEX = cv.gender == "M" ? "男" : "女",
                        ID_NUMBER = cv.citizenId?.Trim() + "",
                        SCHOOL = cv.school?.Trim() + "",
                        MAJOR = cv.major?.Trim() + "",
                        MOBILE = cv.mobile?.Trim() + "",
                        CREATE_EMP = cv.jobMail?.Trim()?.ToLower() + "",
                        AGE = Util.ConvertDaysToApproximateYears((DateTime.Now - DateTime.ParseExact(cv.birthday?.Replace("-", "/"), "yyyy/MM/dd", new CultureInfo("en-US"))).Days) + "",
                        HOMETOWN = cv.hometown?.Trim() + "",
                        EMAIL = cv.email?.Trim()?.ToLower() + "",
                        ETHNIC = cv.ethnic?.Trim() + "",
                        JOB_MAIL = cv.jobMail?.Trim()?.ToLower() + "",
                        JOB_ID = cv.jobID?.Trim() + "",
                        JOB_NAME = cv.jobName?.Trim() + "",
                        F_POSITION_WISH = cv.positionWish?.Trim() + "",
                        BIRTHDAY = cv.birthday?.Trim().Replace("-", "/") + "",
                        ENGLISH_LEVEL = cv.foreignLanguage?.Trim() + "",
                        ENGLISH_SCORE = cv.foreignLanguageLevel <= 0 ? 0 : cv.foreignLanguageLevel,

                    }, trs);
                    trs.Commit();
                    return rs > 0 ? true : false;

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


        /// <summary>
        /// Update cv job for school job
        /// </summary>
        /// <param name="cv"></param>
        /// <returns></returns>
        public bool UpdateCVForSchoolJob(CVForSchoolJob cv)
        {
            using (var cnn = DBHelper.getRMSDBConnectObj())
            {
                OracleTransaction trs = null;
                try
                {

                    cnn.Open();
                    trs = cnn.BeginTransaction();

                    string sqlUpdata = $@"
                       update TB_CR_TP_TALENTPOOL 
                        set 
                         QUALIFICATION = :QUALIFICATION,
                         NAME = :NAME ,
                         SEX = :SEX ,
                         ID_NUMBER = :ID_NUMBER,
                         SCHOOL = :SCHOOL ,
                         MAJOR = :MAJOR,
                         MOBILE = :MOBILE ,
                         AGE = :AGE ,
                         HOMETOWN = :HOMETOWN ,
                         EMAIL = :EMAIL ,
                         ETHNIC = :ETHNIC ,
                         F_POSITION_WISH = :F_POSITION_WISH , 
                         BIRTHDAY  = :BIRTHDAY , 
                         ENGLISH_LEVEL = :ENGLISH_LEVEL, 
                         ENGLISH_SCORE = :ENGLISH_SCORE ,
                         UPDATE_TIME = SYSDATE,
                         UPDATE_EMP  = :UPDATE_EMP
                       where TPID =  :TPID  AND  IS_DELETE = 0
                       ";
                    int rs = cnn.Execute(sqlUpdata, new
                    {
                        QUALIFICATION = cv.qualification?.Trim() + "",
                        NAME = cv.name?.Trim() + "",
                        SEX = cv.gender == "M" ? "男" : "女",
                        ID_NUMBER = cv.citizenId?.Trim() + "",
                        SCHOOL = cv.school?.Trim() + "",
                        MAJOR = cv.major?.Trim() + "",
                        MOBILE = cv.mobile?.Trim() + "",
                        AGE = Util.ConvertDaysToApproximateYears((DateTime.Now - DateTime.ParseExact(cv.birthday?.Replace("-", "/"), "yyyy/MM/dd", new CultureInfo("en-US"))).Days) + "",
                        HOMETOWN = cv.hometown?.Trim() + "",
                        EMAIL = cv.email?.Trim() + "",
                        ETHNIC = cv.ethnic?.Trim() + "",
                        F_POSITION_WISH = cv.positionWish?.Trim() + "",
                        BIRTHDAY = cv.birthday?.Trim().Replace("-", "/") + "",
                        ENGLISH_LEVEL = cv.foreignLanguage?.Trim() + "",
                        ENGLISH_SCORE = cv.foreignLanguageLevel <= 0 ? 0 : cv.foreignLanguageLevel,
                        UPDATE_EMP = cv.jobMail?.Trim()?.ToLower() + "",
                        TPID = cv.TPID + "",
                    }, trs);
                    trs.Commit();
                    return rs > 0 ? true : false;

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
        /// Delete cv job for School job
        /// </summary>
        /// <param name="tpID"></param>
        /// <returns></returns>
        public bool DeleteCVForSchoolJob(string tpID)
        {

            using (var cnn = DBHelper.getRMSDBConnectObj())
            {
                OracleTransaction trs = null;
                try
                {
                    cnn.Open();
                    trs = cnn.BeginTransaction();
                    cnn.Execute(@" DELETE FROM TB_CR_TP_TALENTPOOL  WHERE TPID = :TPID ", new { TPID = tpID?.Trim() + "" }, trs);
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
        /// Get detail cv job for school job
        /// </summary>
        /// <param name="tpID"></param>
        /// <returns></returns>
        public CVForSchoolJob GetCVForSchoolJob(string tpID)
        {
            var talent = GetTbCrTpTalentPool(tpID);
            var status = GetStatusOfImNoRefTPID(tpID);
            var schoolImProcees = GetImInterviewProcess(tpID);

            
            var total = new CVForSchoolJob
            {
                TPID = talent.TPID,
                jobID = talent.JOB_ID,
                jobName = talent.JOB_NAME,
                name = talent.NAME,
                gender = talent.SEX == "男" ? "M" : "F",
                ethnic = talent.ETHNIC,
                mobile = talent.MOBILE,
                school = talent.SCHOOL,
                qualification = talent.QUALIFICATION,
                citizenId = talent.ID_NUMBER,
                birthday = talent.BIRTHDAY,
                major = talent.MAJOR,
                hometown = talent.HOMETOWN,
                email = talent.EMAIL,
                positionWish = talent.F_POSITION_WISH,
                foreignLanguage = talent.ENGLISH_LEVEL,
                foreignLanguageLevel = string.IsNullOrWhiteSpace(talent.ENGLISH_SCORE) ? 0 : int.Parse(talent.ENGLISH_SCORE),
                createDate = talent.CREATE_TIME,
                updateDate = talent.UPDATE_TIME,
                tpStatus = talent.TP_STATUS,
                status = !string.IsNullOrWhiteSpace(status)? status: "Submited and waiting",
                fileID = !string.IsNullOrWhiteSpace(talent.FILE_ID) ? talent.FILE_ID : talent.CV_TEMP_FILE,
                schoolImProcesses= schoolImProcees,
                isAllowEdit= (talent.TP_STATUS == "待安排面试" && talent.IS_DELETE == "0")?true:false,

            };
            return total;
        }



        public bool UpdateCVTempFileNameForSchoolJob(string TPID, string CVFileName, string jobMail)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(@"
                UPDATE TB_CR_TP_TALENTPOOL 
                SET  CV_TEMP_FILE = :CV_TEMP_FILE ,
                    UPDATE_EMP = :UPDATE_EMP,
                    UPDATE_TIME = SYSDATE
                WHERE 
                TPID = :TPID 
                AND LOWER(JOB_MAIL) = :JOB_MAIL
                ", new
            {
                CV_TEMP_FILE = CVFileName?.Trim() + "",
                UPDATE_EMP = jobMail?.Trim()?.ToLower() + "",
                TPID = TPID?.Trim() + "",
                JOB_MAIL = jobMail?.Trim()?.ToLower() + "",
            });
            return rs > 0 ? true : false;
        }


        /// <summary>
        /// Get thong tin file attach cua school job , ma file do da move sang server file
        /// </summary>
        /// <param name="TPID"></param>
        public TB_CR_TP_TALENTPOOL GetFileAttachInforOfSchoolJobMovedToServerFile(string TPID)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<TB_CR_TP_TALENTPOOL>(
                  @" SELECT FILE_ID FROM TB_CR_TP_TALENTPOOL  WHERE TPID = :TPID
            ", new { TPID = TPID?.Trim() + "" });

        }

        public int UpdateFileTransferedToServerFileForSchoolJob(string TPID, string FileId)
        {
            string strsql = $@"select count(1) as CC from TB_CR_TP_TALENTPOOL where tpid = :TPID";
            if (Convert.ToInt32(DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(strsql, new { TPID =  TPID + "" })?.CC) > 0)
            {

                strsql = $@"update TB_CR_TP_TALENTPOOL set FILE_ID = :FILEID where tpid = :TPID";
                return DBHelper.getRMSDBConnectObj().Execute(strsql, new
                {
                    TPID =  TPID + "",
                    FILEID = FileId + "",
                });
            }
            return 0;
        }


        public bool DeleteFileAttachInforOfSchoolJobMovedToServerFile(string TPID)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(
              "update TB_CR_TP_TALENTPOOL set FILE_ID = ''  where tpid = :TPID "
              , new { TPID = TPID?.Trim() + "" });
            return rs > 0 ? true : false;
        }



        public List<TB_CR_TP_TALENTPOOL> GetTbCrTpTalentPoolLsOFJobMail(string jobMail)
        {
            return DBHelper.getRMSDBConnectObj().Query<TB_CR_TP_TALENTPOOL>(
              @"
            SELECT 
            TPID, YEAR, RECRUIT_TYPE, PSYCHOLOGY, QUALIFICATION, NAME, 
            DECODE(SEX,'男','M','F') AS  SEX, ID_NUMBER, SCHOOL, FIRST_MAJOR, MAJOR, PLANT, MOBILE, WAY, ENGLISH_LEVEL, SCORE, BUSINESS_GROUP, SPECIAL_TYPE, IS_ACCOUNT, AGE, HOMETOWN, SCHOOL_TYPE, ENGLISH_SCORE, SALARY, IS_MEMBER, EMAIL, FAMILY_PHONE, RCOM, FILE_ID, IS_DELETE,
            TO_CHAR(UPDATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS UPDATE_TIME, UPDATE_EMP, TO_CHAR(CREATE_TIME, 'YYYY/MM/DD HH24:MI:SS')  AS  CREATE_TIME, CREATE_EMP, TP_STATUS, IS_THREEDIRECT, IS_TRANSCRIPT, IS_XEROX, IS_COLLECTINFO, IS_ENGLISHPROVED, IS_RECOMMANDFORM, REMARK, FAIL_REASON, FAIL_REMARK, DEGREE, GRADUATION_TIME, POSITION_NAME, ETHNIC, CCCD_DATE, CCCD_ADDRESS, BIRTH_ADDRESS, JOB_MAIL, JOB_CV_DETAIL, JOB_ID, JOB_NAME, CV_TEMP_FILE 
            ,F_POSITION_WISH ,BIRTHDAY
            FROM TB_CR_TP_TALENTPOOL WHERE  LOWER(JOB_MAIL) = :JOB_MAIL 
            ", new { JOB_MAIL = jobMail?.Trim()?.ToLower() + "" }).ToList();
        }

        /// <summary>
        /// Get list cv job for school job applied of job mail
        /// </summary>
        /// <param name="jobMail"></param>
        /// <returns></returns>
        public List<CVSchoolJobBaseModel> GetCVSchoolJobLsOfJobMail(string jobMail)
        {
            var cvJobLs = GetTbCrTpTalentPoolLsOFJobMail(jobMail);
            return cvJobLs?.Select(r =>
            {
                var status = GetStatusOfImNoRefTPID(r.TPID);
                var schoolImProcessLs = GetImInterviewProcess(r.TPID);
                return new CVSchoolJobBaseModel
                {
                    tpID = r.TPID,
                    tpStatusId = r.TP_STATUS,
                    createDate = r.CREATE_TIME,
                    updateDate = r.UPDATE_TIME,
                    jobMail = r.JOB_MAIL,
                    jobName = r.JOB_NAME,
                    jobID = r.JOB_ID,
                    status = !string.IsNullOrWhiteSpace(status)? status: "Submited and waiting",
                    schoolImProcesses = schoolImProcessLs ,
                    isAllowEdit = (r.TP_STATUS == "待安排面试" && r.IS_DELETE == "0") ? true : false,
                };
            }
            )?.OrderBy(r => r.createDate + "_" + r.tpStatusId)?.ToList();

        }


    }
}