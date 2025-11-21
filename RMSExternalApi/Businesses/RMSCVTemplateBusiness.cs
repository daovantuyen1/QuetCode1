using Dapper;
using Newtonsoft.Json;
using RMSExternalApi.Commons;
using RMSExternalApi.Commons.DB;
using RMSExternalApi.DTO.RMS;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Businesses
{
    public class RMSCVTemplateBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSCVTemplateBusiness() { }
        private static RMSCVTemplateBusiness _instance;
        public static RMSCVTemplateBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSCVTemplateBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        #region CV template


        /// <summary>
        /// Update file attach to Cv template  (file saved to temp foler)
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        public bool UpdateTempFileAttachInforToCVTemplate(string fileId, string fileName, string mail)
        {

            int rs = DBHelper.getRMSDBConnectObj().Execute(@"
                UPDATE IE_C_CV_TEMPLEATE
                SET 
                   F_FILE_ID = :F_FILE_ID ,
                   F_FILE_NAME = :F_FILE_NAME,
                   F_UPDATE_DATE = SYSDATE ,
                   F_IS_SCAN_FILE ='N' 
                WHERE 
                LOWER(F_MAIL) = :F_MAIL
           ", new
            {
                F_FILE_ID = fileId?.Trim() + "",
                F_FILE_NAME = fileName?.Trim() + "",
                F_MAIL = mail?.Trim()?.ToLower() + "",
            });
            return rs > 0 ? true : false;
        }
        public bool SaveCVTemplate(CVTemplate cVTemplate)
        {
            var oldCV = GetCVTemplate(cVTemplate?.mail);
            var param = new
            {
                F_MAIL = cVTemplate?.mail?.Trim()?.ToLower() + "",
                F_MOBILE = cVTemplate?.mobile?.Trim()?.ToLower() + "",
                F_NAME = cVTemplate?.name?.Trim() + "",
                F_GENDER = cVTemplate?.gender?.Trim() + "",
                F_BIRTHDATE = cVTemplate?.birthday?.Trim()?.Replace("-", "/") + "",
                F_CITIZENID = cVTemplate?.citizenId?.Trim() + "",
                F_MARRIED = cVTemplate?.married?.Trim() + "",
                F_ADDRESS = cVTemplate?.address?.Trim() + "",
                F_MONTHLY_SALARY_WISH = cVTemplate?.monthlySalaryWish?.Trim() + "",
                F_POSITION_WISH = cVTemplate?.positionWish?.Trim() + "",
                F_POSTION_DATE = cVTemplate?.positionDate?.Trim() + "",
                F_EDUCATION_DETAIL = cVTemplate.educations?.Count <= 0 ? new OracleClobParameter("") : new OracleClobParameter(JsonConvert.SerializeObject(cVTemplate.educations)),
                F_JOB_EXPERIENCE = cVTemplate.jobExperiences?.Count <= 0 ? new OracleClobParameter("") : new OracleClobParameter(JsonConvert.SerializeObject(cVTemplate.jobExperiences)),
                F_PROJECT_EXPERIENCE = cVTemplate.projectExperiences?.Count <= 0 ? new OracleClobParameter("") : new OracleClobParameter(JsonConvert.SerializeObject(cVTemplate.projectExperiences)),

            };
            int rs = 0;
            using (var cnn = DBHelper.getRMSDBConnectObj())
            {
                cnn.Open();
                if (oldCV != null)
                {    //update
                    rs = cnn.Execute(
                      @"
                    UPDATE IE_C_CV_TEMPLEATE
                    SET 
                    F_MOBILE = :F_MOBILE,
                    F_NAME = :F_NAME,
                    F_GENDER = :F_GENDER,
                    F_BIRTHDATE = TO_DATE( :F_BIRTHDATE, 'YYYY/MM/DD HH24:MI:SS') ,
                    F_CITIZENID = :F_CITIZENID,
                    F_MARRIED =:F_MARRIED,
                    F_ADDRESS =:F_ADDRESS,
                    F_MONTHLY_SALARY_WISH =:F_MONTHLY_SALARY_WISH,
                    F_POSITION_WISH =:F_POSITION_WISH,
                    F_POSTION_DATE =:F_POSTION_DATE,
                    F_EDUCATION_DETAIL = :F_EDUCATION_DETAIL,
                    F_JOB_EXPERIENCE = :F_JOB_EXPERIENCE,
                    F_PROJECT_EXPERIENCE = :F_PROJECT_EXPERIENCE,
                    F_UPDATE_DATE = SYSDATE
                    WHERE 
                    LOWER(F_MAIL) = :F_MAIL
                    ", param
                     );
                }
                else
                {  //insert 
                    rs = cnn.Execute(
                     @"
                INSERT INTO IE_C_CV_TEMPLEATE(F_MAIL,F_MOBILE,F_NAME ,F_GENDER,F_BIRTHDATE,F_CITIZENID,F_MARRIED,F_ADDRESS,F_MONTHLY_SALARY_WISH,F_POSITION_WISH,F_POSTION_DATE,F_CREATE_DATE,F_UPDATE_DATE,F_EDUCATION_DETAIL,F_JOB_EXPERIENCE,F_PROJECT_EXPERIENCE )
                VALUES (:F_MAIL,:F_MOBILE,:F_NAME ,:F_GENDER,TO_DATE( :F_BIRTHDATE,'YYYY/MM/DD HH24:MI:SS' ),:F_CITIZENID,:F_MARRIED,:F_ADDRESS,:F_MONTHLY_SALARY_WISH,:F_POSITION_WISH,:F_POSTION_DATE,SYSDATE,SYSDATE,:F_EDUCATION_DETAIL,:F_JOB_EXPERIENCE,:F_PROJECT_EXPERIENCE)
                ", param);


                }

            }


            return rs > 0 ? true : false;
        }


        public CVTemplate GetCVTemplate(string mail)
        {
            var data = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<IE_C_CV_TEMPLEATE>(
                  @"
                    SELECT 
                    F_MAIL, 
                    F_MOBILE ,
                    F_NAME ,
                    F_GENDER, 
                    TO_CHAR(F_BIRTHDATE,'YYYY/MM/DD HH24:MI:SS') AS F_BIRTHDATE ,
                    F_CITIZENID ,
                    F_MARRIED ,
                    F_ADDRESS ,
                    F_MONTHLY_SALARY_WISH ,
                    F_POSITION_WISH ,
                    F_POSTION_DATE ,
                    F_EDUCATION_DETAIL ,
                    F_JOB_EXPERIENCE ,
                    F_PROJECT_EXPERIENCE ,
                    TO_CHAR(F_CREATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS F_CREATE_DATE ,
                    TO_CHAR(F_UPDATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS F_UPDATE_DATE  ,
                    F_FILE_ID	 ,
                    F_FILE_NAME	 ,
                    F_IS_SCAN_FILE
                    FROM IE_C_CV_TEMPLEATE WHERE  LOWER(F_MAIL) = :F_MAIL
                    ", new { F_MAIL = mail?.Trim()?.ToLower() + "" });
            if (data != null)
            {
                CVTemplate cVTemplate = new CVTemplate
                {
                    mail = data?.F_MAIL,
                    mobile = data?.F_MOBILE,
                    name = data?.F_NAME,
                    gender = data?.F_GENDER,
                    birthday = data?.F_BIRTHDATE,
                    citizenId = data?.F_CITIZENID,
                    married = data?.F_MARRIED,
                    address = data?.F_ADDRESS,
                    monthlySalaryWish = data?.F_MONTHLY_SALARY_WISH,
                    positionWish = data?.F_POSITION_WISH,
                    positionDate = data?.F_POSTION_DATE,
                    createDate = data?.F_CREATE_DATE,
                    updateDate = data?.F_UPDATE_DATE,
                    fileID = data?.F_FILE_ID,
                    fileName = data?.F_FILE_NAME,
                };

                cVTemplate.educations = !string.IsNullOrWhiteSpace(data?.F_EDUCATION_DETAIL) ? JsonConvert.DeserializeObject<List<Education>>(data?.F_EDUCATION_DETAIL) : new List<Education>();
                cVTemplate.jobExperiences = !string.IsNullOrWhiteSpace(data?.F_JOB_EXPERIENCE) ? JsonConvert.DeserializeObject<List<JobExperience>>(data?.F_JOB_EXPERIENCE) : new List<JobExperience>();
                cVTemplate.projectExperiences = !string.IsNullOrWhiteSpace(data?.F_PROJECT_EXPERIENCE) ? JsonConvert.DeserializeObject<List<ProjectExperience>>(data?.F_PROJECT_EXPERIENCE) : new List<ProjectExperience>();
                return cVTemplate;
            }
            return null;
        }


        public IE_C_CV_TEMPLEATE GetCVTemplateInforByFileID(string fileID)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<IE_C_CV_TEMPLEATE>(
                 @"
                    SELECT 
                    F_MAIL, 
                    F_MOBILE ,
                    F_NAME ,
                    F_GENDER, 
                    TO_CHAR(F_BIRTHDATE,'YYYY/MM/DD HH24:MI:SS') AS F_BIRTHDATE ,
                    F_CITIZENID ,
                    F_MARRIED ,
                    F_ADDRESS ,
                    F_MONTHLY_SALARY_WISH ,
                    F_POSITION_WISH ,
                    F_POSTION_DATE ,
                    F_EDUCATION_DETAIL ,
                    F_JOB_EXPERIENCE ,
                    F_PROJECT_EXPERIENCE ,
                    TO_CHAR(F_CREATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS F_CREATE_DATE ,
                    TO_CHAR(F_UPDATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS F_UPDATE_DATE  ,
                    F_FILE_ID	 ,
                    F_FILE_NAME	 ,
                    F_IS_SCAN_FILE
                    FROM IE_C_CV_TEMPLEATE WHERE  F_FILE_ID = :F_FILE_ID
                    ", new { F_FILE_ID = fileID?.Trim() + "" });
        }
        
        #endregion CV template


    }
}