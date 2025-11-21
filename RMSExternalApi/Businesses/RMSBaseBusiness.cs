using Dapper;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using RMSExternalApi.Commons;
using RMSExternalApi.Commons.DB;
using RMSExternalApi.Controllers;
using RMSExternalApi.DTO.RMS;
using RMSExternalApi.Models.RMS;
using RMSExternalApi.Models.RMS.Requests;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RMSExternalApi.Businesses
{
    public class RMSBaseBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSBaseBusiness() { }
        private static RMSBaseBusiness _instance;
        public static RMSBaseBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSBaseBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        public string GetNewRowID()
        {
           
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<dynamic>(
             @" SELECT GET_ROW_ID() AS ROW_ID FROM DUAL  ")?.ROW_ID;

        }


        public List<IE_C_COUNTRY_CONFIG> GetCountryConfigLs()
        {

            return DBHelper.getRMSDBConnectObj().Query<IE_C_COUNTRY_CONFIG>(@"
                    SELECT ROW_ID, LANGUAGE_KEY, VN_DESC, EN_DESC, CN_DESC ,MAIN_LANG  FROM IE_C_COUNTRY_CONFIG
                    ").ToList();

        }

        public List<ImgSlider> GetHeaderPoster(string LanguageId, string KeyPageHeader, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                     .Query(@"
                        SELECT 
                          A.LANGUAGE_ID ,
                          A.DESCRIPTION,
                          A.APPLY_TYPE,
                          A.APPLY_NO,
                          A.SORT,
                          A.STATUS,
                          A.IS_DELETE,
                          B.FILE_NAME ,
                          B.FILE_EXPANDED_NAME,
                          B.FILE_PATH,
                           B.FILE_SIZE ,
                           B.FILE_DESC
                          FROM  
                          IE_C_COMPANY_POSTER A, 
                          IE_C_FILE_PATH_DATA B
                          WHERE 
                          A.LANGUAGE_ID= :LANGUAGE_ID
                          AND A.APPLY_TYPE= :APPLY_TYPE
                          AND A.STATUS= 'Public'
                          AND A.IS_DELETE='N'
                          AND A.FILE_ID  = B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim()?.ToUpper(), APPLY_TYPE = KeyPageHeader?.Trim() }).ToList();

                var ImgSliderLs = dataLs.Select(r =>
                  {
                      int tempSort;
                      return new ImgSlider
                      {
                          sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                          imageName = r.FILE_NAME,
                          imagePath = Constant.DMZ_LINK + r.FILE_PATH,
                          imageDesc = r.FILE_DESC,

                      };
                  }
               ).ToList();
                ImgSliderLs = ImgSliderLs.OrderBy(r => r.sort).ToList();
                return ImgSliderLs;

            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                    .Query(@"
                        SELECT 
                          A.LANGUAGE_ID ,
                          A.DESCRIPTION,
                          A.APPLY_TYPE,
                          A.APPLY_NO,
                          A.SORT,
                          A.STATUS,
                          A.IS_DELETE,
                          B.FILE_NAME ,
                          B.FILE_EXPANDED_NAME,
                          B.FILE_PATH,
                           B.FILE_SIZE ,
                           B.FILE_DESC
                          FROM  
                          IE_C_COMPANY_POSTER A, 
                          IE_C_FILE_PATH_DATA B
                          WHERE 
                          A.LANGUAGE_ID= :LANGUAGE_ID
                          AND A.APPLY_TYPE= :APPLY_TYPE
                          AND A.IS_DELETE='N'
                          AND A.FILE_ID  = B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim()?.ToUpper(), APPLY_TYPE = KeyPageHeader?.Trim() }).ToList();

                var ImgSliderLs = dataLs.Select(r =>
                {
                    int tempSort;
                    return new ImgSlider
                    {
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC,

                    };
                }
               ).ToList();
                ImgSliderLs = ImgSliderLs.OrderBy(r => r.sort).ToList();
                return ImgSliderLs;

            }
            return new List<ImgSlider>();


        }


        public List<CompanyDevRoadMap> GetCompanyDevRoadMap(string LanguageId, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                    .Query(@"
                    SELECT 
                    A.YEAR ,
                    A.APPLY_TYPE,
                    A.APPLY_NO,
                    A.IMPORTANT_DATA,
                    A.STATUS,
                    B.FILE_NAME,
                    B.FILE_EXPANDED_NAME,
                    B.FILE_PATH,
                    B.FILE_SIZE,
                    B.FILE_DESC

                    FROM 
                    IE_C_COMPANY_PROGRESS A,
                    IE_C_FILE_PATH_DATA B
                    WHERE 
                    A.LANGUAGE_ID = :LANGUAGE_ID
                    AND A.STATUS='Public'
                    AND A.IS_DELETE='N'
                    AND A.FILE_ID = B.FILE_ID
                    ", new
                    {
                        LANGUAGE_ID = LanguageId?.Trim()?.ToUpper()
                    });
                var CompanyDevRoadMapLs = dataLs.Select(r =>
                {
                    return new CompanyDevRoadMap
                    {
                        year = r.YEAR,
                        detailData = r.IMPORTANT_DATA,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.DMZ_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC
                    };
                }).ToList();
                CompanyDevRoadMapLs = CompanyDevRoadMapLs.OrderBy(r => r.year).ToList();
                return CompanyDevRoadMapLs;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                .Query(@"
                    SELECT 
                    A.YEAR ,
                    A.APPLY_TYPE,
                    A.APPLY_NO,
                    A.IMPORTANT_DATA,
                    A.STATUS,
                    B.FILE_NAME,
                    B.FILE_EXPANDED_NAME,
                    B.FILE_PATH,
                    B.FILE_SIZE,
                    B.FILE_DESC

                    FROM 
                    IE_C_COMPANY_PROGRESS A,
                    IE_C_FILE_PATH_DATA B
                    WHERE 
                    A.LANGUAGE_ID = :LANGUAGE_ID
                    AND A.IS_DELETE='N'
                    AND A.FILE_ID = B.FILE_ID
                    ", new
                {
                    LANGUAGE_ID = LanguageId?.Trim()?.ToUpper()
                });
                var CompanyDevRoadMapLs = dataLs.Select(r =>
                {
                    return new CompanyDevRoadMap
                    {
                        year = r.YEAR,
                        detailData = r.IMPORTANT_DATA,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC
                    };
                }).ToList();
                CompanyDevRoadMapLs = CompanyDevRoadMapLs.OrderBy(r => r.year).ToList();
                return CompanyDevRoadMapLs;
            }
            return new List<CompanyDevRoadMap>();
        }


        public List<CompanyIntroduction> GetCompanyIntroduction(string LanguageId, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {

                var dataLs = DBHelper.getRMSDBConnectObj()
                    .Query(@"
                        select 
                        A.TEMPLATE_CONTENT,
                        A.LANGUAGE_ID,
                        A.COUNTRY,
                        A.TEMPPLATE_HIGHLIGHT,
                        A.SORT,
                        B.FILE_ID ,
                        B.FILE_NAME,
                        B.FILE_EXPANDED_NAME,
                        B.FILE_PATH ,
                        B.FILE_DESC
                        from 
                        IE_C_COMPANY_PROFILE A,
                        IE_C_FILE_PATH_DATA B
                        WHERE 
                        A.LANGUAGE_ID= :LANGUAGE_ID
                        AND A.IS_DELETE='N'
                        AND A.STATUS='Public'
                        AND A.FILE_ID= B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim()?.ToUpper() }).ToList();

                var companyIntroduceLs = dataLs.Select(r =>
                {
                    int tempSort;
                    return new CompanyIntroduction
                    {
                        templateContent = r.TEMPLATE_CONTENT,
                        country = r.COUNTRY,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        templateHightLight = r.TEMPPLATE_HIGHLIGHT,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.DMZ_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC

                    };
                }
             ).ToList();

                companyIntroduceLs = companyIntroduceLs.OrderBy(r => r.sort).ToList();
                return companyIntroduceLs;

            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                 .Query(@"
                        select 
                        A.TEMPLATE_CONTENT,
                        A.LANGUAGE_ID,
                        A.COUNTRY,
                        A.TEMPPLATE_HIGHLIGHT,
                        A.SORT,
                        B.FILE_ID ,
                        B.FILE_NAME,
                        B.FILE_EXPANDED_NAME,
                        B.FILE_PATH ,
                        B.FILE_DESC
                        from 
                        IE_C_COMPANY_PROFILE A,
                        IE_C_FILE_PATH_DATA B
                        WHERE 
                        A.LANGUAGE_ID= :LANGUAGE_ID
                        AND A.IS_DELETE='N'
                        AND A.FILE_ID= B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim()?.ToUpper() }).ToList();

                var companyIntroduceLs = dataLs.Select(r =>
                {
                    int tempSort;
                    return new CompanyIntroduction
                    {
                        templateContent = r.TEMPLATE_CONTENT,
                        country = r.COUNTRY,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        templateHightLight = r.TEMPPLATE_HIGHLIGHT,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC

                    };
                }
             ).ToList();

                companyIntroduceLs = companyIntroduceLs.OrderBy(r => r.sort).ToList();
                return companyIntroduceLs;
            }
            return new List<CompanyIntroduction>();
        }


        public List<AboutUs1> GetAboutUs1(string LanguageId, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                 .Query(@"
                        select 
                        A.CONTENT,
                        A.LANGUAGE_ID,
                        A.STATUS,
                        A.SORT, 
                        B.FILE_NAME ,
                        B.FILE_EXPANDED_NAME,
                        B.FILE_PATH,
                        B.FILE_DESC
                        FROM
                        IE_C_ABOUT_US A,
                        IE_C_FILE_PATH_DATA B
                        WHERE 
                        A.LANGUAGE_ID= :LANGUAGE_ID
                        AND A.STATUS= 'Public'
                        AND A.IS_DELETE='N'
                        AND A.FILE_ID =B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim()?.ToUpper() }).ToList();

                var aboutUs1Ls = dataLs.Select(r =>
                {
                    int tempSort;
                    return new AboutUs1
                    {
                        content = r.CONTENT,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.DMZ_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC

                    };
                }
                ).ToList();
                aboutUs1Ls = aboutUs1Ls.OrderBy(r => r.sort).ToList();
                return aboutUs1Ls;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
               .Query(@"
                        select 
                        A.CONTENT,
                        A.LANGUAGE_ID,
                        A.STATUS,
                        A.SORT, 
                        B.FILE_NAME ,
                        B.FILE_EXPANDED_NAME,
                        B.FILE_PATH,
                        B.FILE_DESC
                        FROM
                        IE_C_ABOUT_US A,
                        IE_C_FILE_PATH_DATA B
                        WHERE 
                        A.LANGUAGE_ID= :LANGUAGE_ID
                        AND A.IS_DELETE='N'
                        AND A.FILE_ID =B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim()?.ToUpper() }).ToList();

                var aboutUs1Ls = dataLs.Select(r =>
                {
                    int tempSort;
                    return new AboutUs1
                    {
                        content = r.CONTENT,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC

                    };
                }
                ).ToList();
                aboutUs1Ls = aboutUs1Ls.OrderBy(r => r.sort).ToList();
                return aboutUs1Ls;
            }
            return new List<AboutUs1>();
        }


        public List<AboutUs2> GetAboutUs2(string LanguageId, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                 .Query(@"
                        select 
                            A.TITLE,
                            A.CONTENT,
                            A.SORT,
                            B.FILE_NAME,
                            B.FILE_EXPANDED_NAME,
                            B.FILE_PATH,
                            B.FILE_DESC
                            FROM  
                            IE_C_OUR_BUSINESS A,
                            IE_C_FILE_PATH_DATA B
                            WHERE 
                            A.LANGUAGE_ID= :LANGUAGE_ID
                            AND A.STATUS=  'Public'
                            AND  A.IS_DELETE='N'
                            AND A.FILE_ID = B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim()?.ToUpper() }).ToList();

                var aboutUs2Ls = dataLs.Select(r =>
                {
                    int tempSort;
                    return new AboutUs2
                    {
                        title = r.TITLE,
                        content = r.CONTENT,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.DMZ_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC

                    };
                }
                ).ToList();
                aboutUs2Ls = aboutUs2Ls.OrderBy(r => r.sort).ToList();
                return aboutUs2Ls;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                   .Query(@"
                        select 
                            A.TITLE,
                            A.CONTENT,
                            A.SORT,
                            B.FILE_NAME,
                            B.FILE_EXPANDED_NAME,
                            B.FILE_PATH,
                            B.FILE_DESC
                            FROM  
                            IE_C_OUR_BUSINESS A,
                            IE_C_FILE_PATH_DATA B
                            WHERE 
                            A.LANGUAGE_ID= :LANGUAGE_ID
                            AND  A.IS_DELETE='N'
                            AND A.FILE_ID = B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim()?.ToUpper() }).ToList();

                var aboutUs2Ls = dataLs.Select(r =>
                {
                    int tempSort;
                    return new AboutUs2
                    {
                        title = r.TITLE,
                        content = r.CONTENT,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC

                    };
                }
                ).ToList();
                aboutUs2Ls = aboutUs2Ls.OrderBy(r => r.sort).ToList();
                return aboutUs2Ls;
            }
            return new List<AboutUs2>();
        }



        public List<ContactInfor> GetContactInfor(string LanguageId)
        {
            var dataLs = DBHelper.getRMSDBConnectObj()
                .Query(@"
                    select 
                    OWNER_NAME, CONTACT, EMAIL ,SORT
                    FROM    IE_C_CONTACT_US WHERE LANGUAGE_ID = :LANGUAGE_ID
                    AND IS_DELETE='N'
                    ", new { LANGUAGE_ID = LanguageId?.Trim() }).ToList();
            var contactLs = dataLs.Select(r =>
             {
                 int tempSort;
                 return new ContactInfor
                 {
                     contactName = r.OWNER_NAME,
                     contactPhone = r.CONTACT,
                     contactMail = r.EMAIL,
                     sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0
                 };
             }).ToList();
            contactLs = contactLs.OrderBy(r => r.sort).ToList();
            return contactLs;

        }

        public List<CountryOfWorkerRecruitment> GetCountryLsOfWorkerRecruitment(string LanguageId, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                         .Query(@"
                            select DISTINCT COUNTRY  from  IE_C_WORKER_WEL_RECRUITMENT
                            WHERE 
                            LANGUAGE_ID= :LANGUAGE_ID
                            AND IS_DELETE='N'
                            AND STATUS='Public'
                            AND RECRUITMENT_NAME='技术工'
                            ", new { LANGUAGE_ID = LanguageId?.Trim() }).ToList();
                var countryLs = dataLs.Select(r =>
                {
                    return new CountryOfWorkerRecruitment
                    {
                        country = r.COUNTRY
                    };
                }).ToList();

                var countryLs1 = new List<CountryOfWorkerRecruitment>();
                countryLs1.Add(countryLs.FirstOrDefault(r => r.country == "越南"));
                countryLs1.AddRange(countryLs.Where(r => r.country != "越南").OrderBy(r => r.country).ToList());

                return countryLs1;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                        .Query(@"
                            select DISTINCT COUNTRY  from  IE_C_WORKER_WEL_RECRUITMENT
                            WHERE 
                            LANGUAGE_ID= :LANGUAGE_ID
                            AND IS_DELETE='N'
                            AND RECRUITMENT_NAME='技术工'
                            ", new { LANGUAGE_ID = LanguageId?.Trim() }).ToList();
                var countryLs = dataLs.Select(r =>
                {
                    return new CountryOfWorkerRecruitment
                    {
                        country = r.COUNTRY
                    };
                }).ToList();

                var countryLs1 = new List<CountryOfWorkerRecruitment>();
                countryLs1.Add(countryLs.FirstOrDefault(r => r.country == "越南"));
                countryLs1.AddRange(countryLs.Where(r => r.country != "越南").OrderBy(r => r.country).ToList());
                return countryLs1;

            }
            return new List<CountryOfWorkerRecruitment>();

        }


        public List<ContentFactoryOfWorkerRecruitment> GetContentFactoryOfWorkerRecruitment(string LanguageId, string Call, string Country)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
               .Query(@"
                            SELECT 
                            A.SITE ,
                            A.CONTENT,
                            A.SORT,
                            A.COUNTRY,
                            B.FILE_NAME,
                            B.FILE_EXPANDED_NAME,
                            B.FILE_PATH,
                            B.FILE_DESC
                            FROM 
                            IE_C_WORKER_WEL_RECRUITMENT A,
                            IE_C_FILE_PATH_DATA B
                            WHERE 
                            A.LANGUAGE_ID= :LANGUAGE_ID
                            AND A.IS_DELETE='N'
                            AND A.STATUS='Public'
                            AND A.RECRUITMENT_NAME='技术工'
                            AND A.COUNTRY=  :COUNTRY
                            AND A.FILE_ID = B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim(), COUNTRY = Country?.Trim() }).ToList();
                var ContentFactoryOfWorkerRecruitmentLs = dataLs.Select(r =>
                {
                    int tempSort;
                    return new ContentFactoryOfWorkerRecruitment
                    {
                        factory = r.SITE,
                        content = r.CONTENT,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        country = r.COUNTRY,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.DMZ_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC
                    };
                }).ToList();

                ContentFactoryOfWorkerRecruitmentLs = ContentFactoryOfWorkerRecruitmentLs.OrderBy(r => r.sort).ToList();
                return ContentFactoryOfWorkerRecruitmentLs;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
              .Query(@"
                            SELECT 
                            A.SITE ,
                            A.CONTENT,
                            A.SORT,
                            A.COUNTRY,
                            B.FILE_NAME,
                            B.FILE_EXPANDED_NAME,
                            B.FILE_PATH,
                            B.FILE_DESC
                            FROM 
                            IE_C_WORKER_WEL_RECRUITMENT A,
                            IE_C_FILE_PATH_DATA B
                            WHERE 
                            A.LANGUAGE_ID= :LANGUAGE_ID
                            AND A.IS_DELETE='N'
                            AND A.RECRUITMENT_NAME='技术工'
                            AND A.COUNTRY=  :COUNTRY
                            AND A.FILE_ID = B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim(), COUNTRY = Country?.Trim() }).ToList();
                var ContentFactoryOfWorkerRecruitmentLs = dataLs.Select(r =>
                {
                    int tempSort;
                    return new ContentFactoryOfWorkerRecruitment
                    {
                        factory = r.SITE,
                        content = r.CONTENT,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        country = r.COUNTRY,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC
                    };
                }).ToList();

                ContentFactoryOfWorkerRecruitmentLs = ContentFactoryOfWorkerRecruitmentLs.OrderBy(r => r.sort).ToList();
                return ContentFactoryOfWorkerRecruitmentLs;
            }
            return new List<ContentFactoryOfWorkerRecruitment>();

        }


        public List<ContentSlider2WelfareRecruitment> GetContentSlider2WelfareRecruitment(string LanguageId, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
               .Query(@"
                           SELECT 
                            A.SITE ,
                            A.CONTENT,
                            A.SORT,
                            B.FILE_NAME,
                            B.FILE_EXPANDED_NAME,
                            B.FILE_PATH,
                            B.FILE_DESC
                            FROM 
                            IE_C_WORKER_WEL_RECRUITMENT A,
                            IE_C_FILE_PATH_DATA B
                            WHERE 
                            A.LANGUAGE_ID= :LANGUAGE_ID
                            AND A.IS_DELETE='N'
                            AND A.STATUS='Public'
                            AND A.RECRUITMENT_NAME='公益招聘'
                            AND A.FILE_ID = B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim() }).ToList();
                var ContentSlider2WelfareRecruitmentLS = dataLs.Select(r =>
                {
                    int tempSort;
                    return new ContentSlider2WelfareRecruitment
                    {
                        site = r.SITE,
                        content = r.CONTENT,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.DMZ_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC
                    };

                }).ToList();

                ContentSlider2WelfareRecruitmentLS = ContentSlider2WelfareRecruitmentLS.OrderBy(r => r.sort).ToList();
                return ContentSlider2WelfareRecruitmentLS;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
             .Query(@"
                           SELECT 
                            A.SITE ,
                            A.CONTENT,
                            A.SORT,
                            B.FILE_NAME,
                            B.FILE_EXPANDED_NAME,
                            B.FILE_PATH,
                            B.FILE_DESC
                            FROM 
                            IE_C_WORKER_WEL_RECRUITMENT A,
                            IE_C_FILE_PATH_DATA B
                            WHERE 
                            A.LANGUAGE_ID= :LANGUAGE_ID
                            AND A.IS_DELETE='N'
                            AND A.RECRUITMENT_NAME='公益招聘'
                            AND A.FILE_ID = B.FILE_ID
                        ", new { LANGUAGE_ID = LanguageId?.Trim() }).ToList();
                var ContentSlider2WelfareRecruitmentLS = dataLs.Select(r =>
                {
                    int tempSort;
                    return new ContentSlider2WelfareRecruitment
                    {
                        site = r.SITE,
                        content = r.CONTENT,
                        sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC
                    };

                }).ToList();

                ContentSlider2WelfareRecruitmentLS = ContentSlider2WelfareRecruitmentLS.OrderBy(r => r.sort).ToList();
                return ContentSlider2WelfareRecruitmentLS;
            }
            return new List<ContentSlider2WelfareRecruitment>();
        }


        public Content1WelfareRecruitment GetContent1WelfareRecruitment(string LanguageId, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                      .Query(@"
                        SELECT 
                                  A.TITLE,
                                  A.CONTENT,
                                  B.FILE_NAME ,
                                  B.FILE_EXPANDED_NAME,
                                  B.FILE_PATH,
                                  B.FILE_DESC
                                  FROM 
                                  IE_C_WELFARE_PUBLIC A,
                                  IE_C_FILE_PATH_DATA B
                                  WHERE
                                  A.LANGUAGE_ID =  :LANGUAGE_ID
                                  AND A.STATUS ='Public'
                                  AND A.IS_DELETE ='N'
                                  AND A.FILE_ID = B.FILE_ID
                                  ORDER BY A.ROW_ID DESC
                ", new { LANGUAGE_ID = LanguageId?.Trim() }).ToList();

                var content1WelfareRecruitment = dataLs.Select(r =>
                {
                    return new Content1WelfareRecruitment
                    {
                        title = r.TITLE,
                        content = r.CONTENT,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.DMZ_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC
                    };

                }).FirstOrDefault();
                return content1WelfareRecruitment;

            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                     .Query(@"
                        SELECT 
                                  A.TITLE,
                                  A.CONTENT,
                                  B.FILE_NAME ,
                                  B.FILE_EXPANDED_NAME,
                                  B.FILE_PATH,
                                  B.FILE_DESC
                                  FROM 
                                  IE_C_WELFARE_PUBLIC A,
                                  IE_C_FILE_PATH_DATA B
                                  WHERE
                                  A.LANGUAGE_ID =  :LANGUAGE_ID
                                  AND A.IS_DELETE ='N'
                                  AND A.FILE_ID = B.FILE_ID
                                  ORDER BY A.ROW_ID DESC
                ", new { LANGUAGE_ID = LanguageId?.Trim() }).ToList();

                var content1WelfareRecruitment = dataLs.Select(r =>
                {
                    return new Content1WelfareRecruitment
                    {
                        title = r.TITLE,
                        content = r.CONTENT,
                        imageName = r.FILE_NAME,
                        imagePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                        imageDesc = r.FILE_DESC
                    };

                }).FirstOrDefault();
                return content1WelfareRecruitment;
            }
            return null;
        }


        public List<CountryOfSeekRecruitment> GetCountryLsOfSeekRecruitment(string RecruitmentName, string LanguageId, string Call)
        {

            var dataLs1 = DBHelper.getRMSDBConnectObj()
                   .Query<IE_C_COUNTRY_CONFIG>(@"
                           select  
                                ROW_ID, LANGUAGE_KEY, VN_DESC, EN_DESC, CN_DESC , MAIN_LANG
                                from   IE_C_COUNTRY_CONFIG  
                                ORDER BY ROW_ID ASC
                            ").ToList();
            var dataLs = new List<IE_C_COUNTRY_CONFIG>();
            dataLs.Add(dataLs1.FirstOrDefault(r => r.MAIN_LANG == "VN"));
            dataLs.AddRange(dataLs1.Where(r => r.MAIN_LANG != "VN").OrderBy(r => r.EN_DESC).ToList());

            if (Call.ToUpper() == CallType.DMZ.ToString())
            {

                var countryOfSeekRecruitment = dataLs.Select(r =>
                {

                    return new CountryOfSeekRecruitment
                    {
                        countryId = r.ROW_ID,
                        countryName = LanguageId == "EN" ? r.EN_DESC : (LanguageId == "VN" ? r.VN_DESC : r.CN_DESC),
                    };
                }).ToList();

                return countryOfSeekRecruitment;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {

                var countryOfSeekRecruitment = dataLs.Select(r =>
                {

                    return new CountryOfSeekRecruitment
                    {
                        countryId = r.ROW_ID,
                        countryName = LanguageId == "EN" ? r.EN_DESC : (LanguageId == "VN" ? r.VN_DESC : r.CN_DESC),
                    };
                }).ToList();

                return countryOfSeekRecruitment;
            }
            return new List<CountryOfSeekRecruitment>();





            //if (Call.ToUpper() == CallType.DMZ.ToString())
            //{
            //    var dataLs = new OracleConnection(getRMSDBConnect())
            //        .Query(@"
            //                SELECT 
            //                DISTINCT 
            //                B.ROW_ID ,
            //                B.LANGUAGE_KEY, 
            //                B.VN_DESC,
            //                B.EN_DESC,
            //                B.CN_DESC
            //                FROM IE_C_RECRUITMENT_DATA A ,
            //                IE_C_COUNTRY_CONFIG B
            //                WHERE
            //                A.RECRUITMENT_NAME = :RECRUITMENT_NAME
            //                AND A.LANGUAGE_ID LIKE  :LANGUAGE_ID    
            //                AND A.STATUS='Public'
            //                AND A.IS_DELETE='N'
            //                AND A.COUNTRY =B.ROW_ID
            //                ",
            //                new
            //                {
            //                    RECRUITMENT_NAME = RecruitmentName.Trim(),
            //                    LANGUAGE_ID = string.Format("%{0}%", LanguageId.Trim().ToUpper())
            //                }).ToList();
            //    var countryOfSeekRecruitment = dataLs.Select(r =>
            //    {

            //        return new CountryOfSeekRecruitment
            //        {
            //            countryId = r.ROW_ID,
            //            countryName = LanguageId == "EN" ? r.EN_DESC : (LanguageId == "VN" ? r.VN_DESC : r.CN_DESC),
            //        };
            //    }).ToList();
            //    countryOfSeekRecruitment = countryOfSeekRecruitment.OrderBy(r => r.countryName).ToList();
            //    return countryOfSeekRecruitment;


            //}
            //else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            //{
            //    var dataLs = new OracleConnection(getRMSDBConnect())
            //       .Query(@"
            //                SELECT 
            //                DISTINCT 
            //                B.ROW_ID ,
            //                B.LANGUAGE_KEY, 
            //                B.VN_DESC,
            //                B.EN_DESC,
            //                B.CN_DESC
            //                FROM IE_C_RECRUITMENT_DATA A ,
            //                IE_C_COUNTRY_CONFIG B
            //                WHERE
            //                A.RECRUITMENT_NAME = :RECRUITMENT_NAME
            //                AND A.LANGUAGE_ID LIKE  :LANGUAGE_ID   
            //                AND A.IS_DELETE='N'
            //                AND A.COUNTRY =B.ROW_ID
            //                ",
            //               new
            //               {
            //                   RECRUITMENT_NAME = RecruitmentName.Trim(),
            //                   LANGUAGE_ID = string.Format("%{0}%", LanguageId.Trim().ToUpper())
            //               }).ToList();
            //    var countryOfSeekRecruitment = dataLs.Select(r =>
            //    {

            //        return new CountryOfSeekRecruitment
            //        {
            //            countryId = r.ROW_ID,
            //            countryName = LanguageId == "EN" ? r.EN_DESC : (LanguageId == "VN" ? r.VN_DESC : r.CN_DESC),
            //        };
            //    }).ToList();
            //    countryOfSeekRecruitment = countryOfSeekRecruitment.OrderBy(r => r.countryName).ToList();
            //    return countryOfSeekRecruitment;
            //}
            //return new List<CountryOfSeekRecruitment>();



        }

        public List<CatergoryOfSeekRecruitment> GetJobCatergoryLsOfSeekRecruitment(string RecruitmentName, string LanguageId, string CountryId, string Call)
        {
            //var mainLang = new OracleConnection(getRMSDBConnect()).QueryFirstOrDefault<IE_C_COUNTRY_CONFIG>(@"
            //select * from ie_c_country_config WHERE  ROW_ID = :ROW_ID
            //", new { ROW_ID = CountryId?.Trim()?.ToUpper() })?.MAIN_LANG;

            //countryID + LANGid

            if (Call.ToUpper() == CallType.DMZ.ToString())
            {

                var dataLs = DBHelper.getRMSDBConnectObj()
                   .Query(@"
                            SELECT DISTINCT A.JOB_CATEGORIES
                            FROM 
                            IE_C_RECRUITMENT_DATA A
                            WHERE 
                            A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                            AND (  
                                ( :COUNTRY  IS NULL OR :COUNTRY ='' OR A.COUNTRY =:COUNTRY )    AND   A.LANGUAGE_ID LIKE :LANGUAGE_ID
                             )
                            AND A.STATUS= 'Public'
                            AND A.IS_DELETE='N'
                            ORDER BY  A.JOB_CATEGORIES ASC
                            ",
                           new
                           {
                               RECRUITMENT_NAME = RecruitmentName?.Trim(),
                               LANGUAGE_ID = string.Format("%{0}%", LanguageId?.Trim()?.ToUpper()),
                               COUNTRY = string.IsNullOrWhiteSpace(CountryId) || CountryId.Trim().ToUpper() == "ALL" ? "" : CountryId?.Trim()?.ToUpper(),
                           }).ToList();

                var CatergoryOfSeekRecruitmentLS = dataLs.Select(r =>
                 {
                     return new CatergoryOfSeekRecruitment
                     {
                         category = r.JOB_CATEGORIES,
                     };
                 }).ToList();

                CatergoryOfSeekRecruitmentLS.Insert(0, new CatergoryOfSeekRecruitment { category = LanguageId == "VN" ? "Tất cả" : (LanguageId == "EN" ? "All" : "全部") });

                return CatergoryOfSeekRecruitmentLS;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                .Query(@"
                            SELECT DISTINCT A.JOB_CATEGORIES
                            FROM 
                            IE_C_RECRUITMENT_DATA A
                            WHERE 
                            A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                            AND (  
                               (   :COUNTRY  IS NULL OR :COUNTRY ='' OR A.COUNTRY =:COUNTRY   )   AND   A.LANGUAGE_ID LIKE :LANGUAGE_ID
                             )
                            AND A.IS_DELETE='N'
                            ORDER BY  A.JOB_CATEGORIES ASC
                            ",
                        new
                        {
                            RECRUITMENT_NAME = RecruitmentName?.Trim(),
                            LANGUAGE_ID = string.Format("%{0}%", LanguageId?.Trim()?.ToUpper()),
                            COUNTRY = string.IsNullOrWhiteSpace(CountryId) || CountryId.Trim().ToUpper() == "ALL" ? "" : CountryId?.Trim()?.ToUpper(),
                        }).ToList();


                var CatergoryOfSeekRecruitmentLS = dataLs.Select(r =>
                {
                    return new CatergoryOfSeekRecruitment
                    {
                        category = r.JOB_CATEGORIES,
                    };
                }).ToList();
                CatergoryOfSeekRecruitmentLS.Insert(0, new CatergoryOfSeekRecruitment { category = LanguageId == "VN" ? "Tất cả" : (LanguageId == "EN" ? "All" : "全部") });

                return CatergoryOfSeekRecruitmentLS;
            }
            return new List<CatergoryOfSeekRecruitment>();
        }



        public List<ExperienceOfSeekRecruitment> GetExperienceLsOfSeekRecruitment(string RecruitmentName, string LanguageId, string CountryId, string Call)
        {
            //var mainLang = new OracleConnection(getRMSDBConnect()).QueryFirstOrDefault<IE_C_COUNTRY_CONFIG>(@"
            //select * from ie_c_country_config WHERE  ROW_ID = :ROW_ID
            //", new { ROW_ID = CountryId?.Trim()?.ToUpper() })?.MAIN_LANG;


            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                   .Query(@"
                            SELECT DISTINCT A.EXPERIENCE
                            FROM 
                            IE_C_RECRUITMENT_DATA A
                            WHERE 
                            A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                            AND (  
                               (  :COUNTRY IS NULL OR  :COUNTRY='' OR A.COUNTRY = :COUNTRY  )    AND   A.LANGUAGE_ID LIKE :LANGUAGE_ID
                             )
                            AND A.STATUS= 'Public'
                            AND A.IS_DELETE='N'
                            ORDER BY  A.EXPERIENCE ASC
                            ",
                           new
                           {
                               RECRUITMENT_NAME = RecruitmentName?.Trim(),
                               LANGUAGE_ID = string.Format("%{0}%", LanguageId?.Trim()?.ToUpper()),
                               COUNTRY = string.IsNullOrWhiteSpace(CountryId) || CountryId.Trim().ToUpper() == "ALL" ? "" : CountryId?.Trim()?.ToUpper(),
                           }).ToList();
                var ExperienceOfSeekRecruitmentLs = dataLs.Select(r =>
                {
                    return new ExperienceOfSeekRecruitment
                    {
                        experience = r.EXPERIENCE,
                    };
                }).ToList();

                ExperienceOfSeekRecruitmentLs.Insert(0, new ExperienceOfSeekRecruitment { experience = LanguageId == "VN" ? "Tất cả" : (LanguageId == "EN" ? "All" : "全部") });



                return ExperienceOfSeekRecruitmentLs;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                    .Query(@"
                            SELECT DISTINCT A.EXPERIENCE
                            FROM 
                            IE_C_RECRUITMENT_DATA A
                            WHERE 
                            A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                            AND (  
                               (  :COUNTRY IS NULL OR  :COUNTRY='' OR  A.COUNTRY =:COUNTRY)    AND   A.LANGUAGE_ID LIKE :LANGUAGE_ID
                             )
                            AND A.IS_DELETE='N'
                            ORDER BY  A.EXPERIENCE ASC
                            ",
                            new
                            {
                                RECRUITMENT_NAME = RecruitmentName?.Trim(),
                                LANGUAGE_ID = string.Format("%{0}%", LanguageId?.Trim()?.ToUpper()),
                                COUNTRY = string.IsNullOrWhiteSpace(CountryId) || CountryId.Trim().ToUpper() == "ALL" ? "" : CountryId?.Trim()?.ToUpper(),
                            }).ToList();
                var ExperienceOfSeekRecruitmentLs = dataLs.Select(r =>
                {
                    return new ExperienceOfSeekRecruitment
                    {
                        experience = r.EXPERIENCE,
                    };
                }).ToList();
                ExperienceOfSeekRecruitmentLs.Insert(0, new ExperienceOfSeekRecruitment { experience = LanguageId == "VN" ? "Tất cả" : (LanguageId == "EN" ? "All" : "全部") });

                return ExperienceOfSeekRecruitmentLs;
            }
            return new List<ExperienceOfSeekRecruitment>();
        }



        public List<EducationOfSeekRecruitment> GetEducationLsOfSeekRecruitment(string RecruitmentName, string LanguageId, string CountryId, string Call)
        {
            //var mainLang = new OracleConnection(getRMSDBConnect()).QueryFirstOrDefault<IE_C_COUNTRY_CONFIG>(@"
            //select * from ie_c_country_config WHERE  ROW_ID = :ROW_ID
            //", new { ROW_ID = CountryId?.Trim()?.ToUpper() })?.MAIN_LANG;


            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                   .Query(@"
                            SELECT DISTINCT A.MINIMUM_EDUCATION
                            FROM 
                            IE_C_RECRUITMENT_DATA A
                            WHERE 
                            A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                            AND (  
                              ( :COUNTRY  IS NULL OR :COUNTRY ='' OR  A.COUNTRY =:COUNTRY   )   AND   A.LANGUAGE_ID LIKE :LANGUAGE_ID
                             )
                            AND A.STATUS= 'Public'
                            AND A.IS_DELETE='N'
                            ORDER BY  A.MINIMUM_EDUCATION ASC
                            ",
                           new
                           {
                               RECRUITMENT_NAME = RecruitmentName?.Trim(),
                               LANGUAGE_ID = string.Format("%{0}%", LanguageId?.Trim()?.ToUpper()),
                               COUNTRY = string.IsNullOrWhiteSpace(CountryId) || CountryId.Trim().ToUpper() == "ALL" ? "" : CountryId?.Trim()?.ToUpper(),
                           }).ToList();
                var EducationOfSeekRecruitmentLs = dataLs.Select(r =>
                {
                    return new EducationOfSeekRecruitment
                    {
                        education = r.MINIMUM_EDUCATION,
                    };
                }).ToList();

                EducationOfSeekRecruitmentLs.Insert(0, new EducationOfSeekRecruitment { education = LanguageId == "VN" ? "Tất cả" : (LanguageId == "EN" ? "All" : "全部") });



                return EducationOfSeekRecruitmentLs;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                     .Query(@"
                            SELECT DISTINCT A.MINIMUM_EDUCATION
                            FROM 
                            IE_C_RECRUITMENT_DATA A
                            WHERE 
                            A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                            AND (  
                             (   :COUNTRY  IS NULL OR :COUNTRY =''  OR A.COUNTRY =:COUNTRY   )    AND   A.LANGUAGE_ID LIKE :LANGUAGE_ID
                             )
                            AND A.IS_DELETE='N'
                            ORDER BY  A.MINIMUM_EDUCATION ASC
                            ",
                             new
                             {
                                 RECRUITMENT_NAME = RecruitmentName?.Trim(),
                                 LANGUAGE_ID = string.Format("%{0}%", LanguageId?.Trim()?.ToUpper()),
                                 COUNTRY = string.IsNullOrWhiteSpace(CountryId) || CountryId.Trim().ToUpper() == "ALL" ? "" : CountryId?.Trim()?.ToUpper(),
                             }).ToList();
                var EducationOfSeekRecruitmentLs = dataLs.Select(r =>
                {
                    return new EducationOfSeekRecruitment
                    {
                        education = r.MINIMUM_EDUCATION,
                    };
                }).ToList();

                EducationOfSeekRecruitmentLs.Insert(0, new EducationOfSeekRecruitment { education = LanguageId == "VN" ? "Tất cả" : (LanguageId == "EN" ? "All" : "全部") });



                return EducationOfSeekRecruitmentLs;
            }
            return new List<EducationOfSeekRecruitment>();
        }



        public List<HotJob> GetHotJobLs(string RecruitmentName, string LanguageId, string Call)
        {
            var countryIdLs = DBHelper.getRMSDBConnectObj().Query<IE_C_COUNTRY_CONFIG>(@"
            select * from ie_c_country_config  WHERE  MAIN_LANG = :MAIN_LANG
            ", new { MAIN_LANG = LanguageId?.Trim()?.ToUpper() }).Select(r => r.ROW_ID).ToList();

            string formatDt = LanguageId == "VN" ? "DD/MM/YYYY" : "YYYY/MM/DD";

            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                 .Query(
                        $@"
                          SELECT 
                                    A.ROW_ID,
                                    A.POSITIONS ,
                                    A.PLACE, 
                                    A.JOB_CATEGORIES ,
                                    A.MINIMUM_EDUCATION,
                                    A.EXPERIENCE ,
                                    A.QTY ,
                                    TO_CHAR( DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ) ,'{formatDt}') AS UPDATE_TIME,
                                    A.SALARY ,
                                    A.IS_HOTJOB  ,
                                    B.VN_DESC,
                                    B.EN_DESC, 
                                    B.CN_DESC
                                    FROM 
                                    IE_C_RECRUITMENT_DATA A  
                                    LEFT JOIN  IE_C_COUNTRY_CONFIG B ON   A.COUNTRY = B.ROW_ID
                                    WHERE 
                                    A.IS_HOTJOB='Y'
                                    AND A.STATUS='Public' 
                                    AND A.IS_DELETE='N' 
                                    AND (  A.COUNTRY IN ({string.Join(",", countryIdLs.Select(r => "'" + r + "'").ToList())}) 
                                           OR   A.LANGUAGE_ID LIKE  :LANGUAGE_ID     )
                                    AND A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                                    AND ROWNUM<= 8
                                    ORDER BY  TO_CHAR( DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ) ,'{formatDt}')   DESC
                        "
                        ,
                             new
                             {
                                 RECRUITMENT_NAME = RecruitmentName?.Trim(),
                                 LANGUAGE_ID = string.Format("%{0}%", LanguageId?.Trim()?.ToUpper()),
                             }).ToList();


                var HotJobLs = dataLs.Select(r =>
                {
                    return new HotJob
                    {
                        jobId = r.ROW_ID,
                        jobPosition = r.POSITIONS,
                        jobCategory = r.JOB_CATEGORIES,
                        jobEducation = r.MINIMUM_EDUCATION,
                        jobExperience = r.EXPERIENCE,
                        jobQty = int.Parse(r.QTY.ToString()),
                        jobPublicDate = r.UPDATE_TIME,
                        jobSalary = r.SALARY,
                        isHotJob = r.IS_HOTJOB == "Y" ? true : false,
                        jobPlace = r.PLACE,
                        jobFullPlace = LanguageId.Trim().ToUpper() == "EN" ? r.EN_DESC + "-" + r.PLACE : (LanguageId.Trim().ToUpper() == "VN" ? r.VN_DESC + "-" + r.PLACE : r.CN_DESC + "-" + r.PLACE)

                    };
                }).ToList();
                return HotJobLs;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                  .Query(
                         $@"
                          SELECT 
                                    A.ROW_ID,
                                    A.POSITIONS ,
                                    A.PLACE, 
                                    A.JOB_CATEGORIES ,
                                    A.MINIMUM_EDUCATION,
                                    A.EXPERIENCE ,
                                    A.QTY ,
                                    TO_CHAR( DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ) ,'{formatDt}') AS UPDATE_TIME,
                                    A.SALARY ,
                                    A.IS_HOTJOB  ,
                                    B.VN_DESC,
                                    B.EN_DESC, 
                                    B.CN_DESC
                                    FROM 
                                    IE_C_RECRUITMENT_DATA A  
                                    LEFT JOIN  IE_C_COUNTRY_CONFIG B ON   A.COUNTRY = B.ROW_ID
                                    WHERE 
                                    A.IS_HOTJOB='Y'
                                    AND A.IS_DELETE='N' 
                                    AND (  A.COUNTRY IN ({string.Join(",", countryIdLs.Select(r => "'" + r + "'").ToList())}) 
                                           OR   A.LANGUAGE_ID LIKE  :LANGUAGE_ID )
                                    AND A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                                    AND ROWNUM<= 8
                                    ORDER BY TO_CHAR( DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ) ,'{formatDt}')  DESC
                        "
                         ,
                              new
                              {
                                  RECRUITMENT_NAME = RecruitmentName?.Trim(),
                                  LANGUAGE_ID = string.Format("%{0}%", LanguageId?.Trim()?.ToUpper()),
                              }).ToList();


                var HotJobLs = dataLs.Select(r =>
                {
                    return new HotJob
                    {
                        jobId = r.ROW_ID,
                        jobPosition = r.POSITIONS,
                        jobCategory = r.JOB_CATEGORIES,
                        jobEducation = r.MINIMUM_EDUCATION,
                        jobExperience = r.EXPERIENCE,
                        jobQty = int.Parse(r.QTY.ToString()),
                        jobPublicDate = r.UPDATE_TIME,
                        jobSalary = r.SALARY,
                        isHotJob = r.IS_HOTJOB == "Y" ? true : false,
                        jobPlace = r.PLACE,
                        jobFullPlace = LanguageId.Trim().ToUpper() == "EN" ? r.EN_DESC + "-" + r.PLACE : (LanguageId.Trim().ToUpper() == "VN" ? r.VN_DESC + "-" + r.PLACE : r.CN_DESC + "-" + r.PLACE)
                    };
                }).ToList();
                return HotJobLs;
            }
            return new List<HotJob>();
        }


        public DetailJob GetDetailJob(string JobID, string Call, string LanguageId)
        {


            string formatDt = LanguageId == "VN" ? "DD/MM/YYYY" : "YYYY/MM/DD";

            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                 .Query(
                          $@"
                              SELECT 
                                A.POSITIONS,
                                A.JOB_CATEGORIES,
                                A.PLACE,
                                A.MINIMUM_EDUCATION,
                                A.EXPERIENCE, 
                                A.QTY,
                                A.JOB_DESC ,
                                TO_CHAR(A.UPDATE_TIME ,'{formatDt}') AS UPDATE_TIME ,
                                A.SALARY ,
                                A.IS_HOTJOB
                                FROM
                                IE_C_RECRUITMENT_DATA   A
                                
                                WHERE 
                                A.ROW_ID= :ROW_ID
                                --  AND A.STATUS='Public'  -- tam comment for debug
                                AND A.IS_DELETE='N'
                             
                        "
                        ,
                             new
                             {
                                 ROW_ID = JobID?.Trim(),
                             }).ToList();
                var detailJob = dataLs.Select(r =>
                {
                    return new DetailJob
                    {
                        position = r.POSITIONS,
                        category = r.JOB_CATEGORIES,
                        place = r.PLACE,
                        education = r.MINIMUM_EDUCATION,
                        experience = r.EXPERIENCE,
                        qty = int.Parse(r.QTY.ToString()),
                        description = r.JOB_DESC,
                        publicDate = r.UPDATE_TIME,
                        salary = r.SALARY,
                        isHotJob = r.IS_HOTJOB == "Y" ? true : false,

                    };

                }).FirstOrDefault();
                return detailJob;


            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dataLs = DBHelper.getRMSDBConnectObj()
                .Query(
                         $@"
                                  SELECT 
                                A.POSITIONS,
                                A.JOB_CATEGORIES,
                                A.PLACE,
                                A.MINIMUM_EDUCATION,
                                A.EXPERIENCE, 
                                A.QTY,
                                A.JOB_DESC ,
                                TO_CHAR(A.UPDATE_TIME ,'{formatDt}') AS UPDATE_TIME ,
                                A.SALARY ,
                                A.IS_HOTJOB
                                FROM
                                IE_C_RECRUITMENT_DATA   A
                                WHERE 
                                A.ROW_ID= :ROW_ID
                                AND A.IS_DELETE='N'
                        "
                       ,
                            new
                            {
                                ROW_ID = JobID?.Trim(),
                            }).ToList();
                var detailJob = dataLs.Select(r =>
                {
                    return new DetailJob
                    {
                        position = r.POSITIONS,
                        category = r.JOB_CATEGORIES,
                        place = r.PLACE,
                        education = r.MINIMUM_EDUCATION,
                        experience = r.EXPERIENCE,
                        qty = int.Parse(r.QTY.ToString()),
                        description = r.JOB_DESC,
                        publicDate = r.UPDATE_TIME,
                        salary = r.SALARY,
                        isHotJob = r.IS_HOTJOB == "Y" ? true : false,

                    };

                }).FirstOrDefault();
                return detailJob;
            }
            return null;
        }


        public ElTableRes GetJobLsOfSeekRecruitment(JobOfSeekRecruitmentReq data)
        {

            //var mainLang = new OracleConnection(getRMSDBConnect()).QueryFirstOrDefault<IE_C_COUNTRY_CONFIG>(@"
            //select * from ie_c_country_config WHERE  ROW_ID = :ROW_ID
            //", new { ROW_ID = data.CountryID?.Trim()?.ToUpper() })?.MAIN_LANG;





            string formatDt = data.LanguageId == "VN" ? "DD/MM/YYYY" : "YYYY/MM/DD";

            string columns = $@"
                                    A.ROW_ID,
                                    A.POSITIONS ,
                                    A.PLACE, 
                                    A.JOB_CATEGORIES ,
                                    A.MINIMUM_EDUCATION,
                                    A.EXPERIENCE ,
                                    A.QTY ,
                                    TO_CHAR(DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ),'{formatDt}') AS UPDATE_TIME,
                                    A.SALARY ,
                                    A.IS_HOTJOB  ,
                                    B.VN_DESC,
                                    B.EN_DESC, 
                                    B.CN_DESC ,
                                    A.COUNTRY , 
                                    A.LANGUAGE_ID
                                ";

            int star_rownum = (data.page * data.pageSize) - data.pageSize + 1;
            int end_rownum = data.page * data.pageSize;
            string sql = "";

            if (data.Call.ToUpper() == CallType.DMZ.ToString())
            {
                sql = $@"  SELECT  DISTINCT
                                    {columns}
                                    FROM 
                                    IE_C_RECRUITMENT_DATA A  
                                    LEFT JOIN  IE_C_COUNTRY_CONFIG B  ON  A.COUNTRY = B.ROW_ID
                                    WHERE 
                                        A.STATUS='Public' 
                                    AND A.IS_DELETE='N' 
                                    AND A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                                    AND (
                                          ( :POSITIONS IS NULL OR :POSITIONS='' 
                                          OR  LOWER( A.POSITIONS )  LIKE :POSITIONS  
                                          OR  LOWER( A.JOB_CATEGORIES )  LIKE :POSITIONS 
                                          OR  LOWER( A.PLACE   )  LIKE :POSITIONS 
                                          OR  LOWER( A.MINIMUM_EDUCATION    )  LIKE :POSITIONS 
                                          OR  LOWER( A.JOB_DESC      )  LIKE :POSITIONS 
                                          OR  LOWER( A.EXPERIENCE        )  LIKE :POSITIONS 
                                          OR  LOWER( A.EXPERIENCE        )  LIKE :POSITIONS 
                                          OR  LOWER( A.SALARY        )  LIKE :POSITIONS 
                                         )

                                       AND ( (:COUNTRY IS NULL OR :COUNTRY ='' OR A.COUNTRY =:COUNTRY) AND ( :LANGUAGE_ID IS NULL OR :LANGUAGE_ID = '' OR  A.LANGUAGE_ID LIKE  :LANGUAGE_ID ) )

                                       AND ( :JOBCATEGORIES IS NULL OR :JOBCATEGORIES='' OR  LOWER ( A.JOB_CATEGORIES )  LIKE :JOBCATEGORIES ) 
                                       AND ( :EXPERIENCE IS NULL OR :EXPERIENCE='' OR  LOWER( A.EXPERIENCE )  LIKE :EXPERIENCE  )
                                       AND ( :EDUCATION IS NULL OR :EDUCATION ='' OR  LOWER( A.MINIMUM_EDUCATION )  LIKE :EDUCATION )
                                    )
                                    ORDER BY       TO_CHAR(DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ),'{formatDt}')   DESC";




            }
            else if (data.Call.ToUpper() == CallType.INTERNAL.ToString())
            {

                sql = $@"  SELECT  DISTINCT
                                    {columns}
                                    FROM 
                                    IE_C_RECRUITMENT_DATA A  
                                    LEFT JOIN  IE_C_COUNTRY_CONFIG B  ON  A.COUNTRY = B.ROW_ID
                                    WHERE 
                                     A.IS_DELETE='N' 
                                     AND A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                                     AND (
                                          ( :POSITIONS IS NULL OR :POSITIONS='' 
                                          OR  LOWER( A.POSITIONS )  LIKE :POSITIONS  
                                          OR  LOWER( A.JOB_CATEGORIES )  LIKE :POSITIONS 
                                          OR  LOWER( A.PLACE   )  LIKE :POSITIONS 
                                          OR  LOWER( A.MINIMUM_EDUCATION    )  LIKE :POSITIONS 
                                          OR  LOWER( A.JOB_DESC      )  LIKE :POSITIONS 
                                          OR  LOWER( A.EXPERIENCE        )  LIKE :POSITIONS 
                                          OR  LOWER( A.SALARY        )  LIKE :POSITIONS 
                                         )

                                       AND ( (:COUNTRY IS NULL OR :COUNTRY ='' OR A.COUNTRY =:COUNTRY) AND ( :LANGUAGE_ID IS NULL OR :LANGUAGE_ID = '' OR  A.LANGUAGE_ID LIKE  :LANGUAGE_ID ) )

                                       AND ( :JOBCATEGORIES IS NULL OR :JOBCATEGORIES='' OR  LOWER ( A.JOB_CATEGORIES )  LIKE :JOBCATEGORIES ) 
                                       AND ( :EXPERIENCE IS NULL OR :EXPERIENCE='' OR  LOWER( A.EXPERIENCE )  LIKE :EXPERIENCE  )
                                       AND ( :EDUCATION IS NULL OR :EDUCATION ='' OR  LOWER( A.MINIMUM_EDUCATION )  LIKE :EDUCATION )
                                    )
                                    ORDER BY  TO_CHAR(DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ),'{formatDt}')  DESC";
            }

            string sql1 = $@"
                   SELECT 
                    ROW_ID, POSITIONS, PLACE, JOB_CATEGORIES, MINIMUM_EDUCATION, EXPERIENCE, QTY, UPDATE_TIME, SALARY, IS_HOTJOB, VN_DESC, EN_DESC, CN_DESC , COUNTRY ,  LANGUAGE_ID     
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY  ROW_ID DESC) AS ROW_NUMBER, K.*
                          FROM (
						      {sql}
						  )  K ) E
                    WHERE E.ROW_NUMBER BETWEEN {star_rownum} AND {end_rownum} ORDER BY UPDATE_TIME DESC
              ";

            int totalCount = 0;
            var paramObj = new
            {
                LANGUAGE_ID = string.IsNullOrWhiteSpace(data.LanguageId) ? "" : string.Format("%{0}%", data.LanguageId?.Trim()?.ToUpper()),   //  string.Format("%{0}%", data.LanguageId.Trim().ToUpper()),
                RECRUITMENT_NAME = data.RecruitmentName.Trim(),
                POSITIONS = string.IsNullOrWhiteSpace(data.Position) ? "" : string.Format("%{0}%", string.Join("%", Regex.Split(data.Position, @"\W+").Select(r => r?.Trim()?.ToLower()).ToList())),
                COUNTRY = string.IsNullOrWhiteSpace(data.CountryID) || data.CountryID.Trim().ToUpper() == "ALL" ? "" : data.CountryID.Trim(),
                JOBCATEGORIES = string.IsNullOrWhiteSpace(data.JobCategory) || (new[] { "Tất cả", "All", "全部" }.Contains(data.JobCategory)) ? "" : string.Format("%{0}%", data.JobCategory?.Trim()?.ToLower()),
                EXPERIENCE = string.IsNullOrWhiteSpace(data.Experience) || (new[] { "Tất cả", "All", "全部" }.Contains(data.Experience)) ? "" : string.Format("%{0}%", data.Experience?.Trim()?.ToLower()),
                EDUCATION = string.IsNullOrWhiteSpace(data.Education) || (new[] { "Tất cả", "All", "全部" }.Contains(data.Education)) ? "" : string.Format("%{0}%", data.Education?.Trim()?.ToLower()),
            };

            var tempCount = DBHelper.InstanceRMSDB.QueryFirstOrDefault($"SELECT COUNT(1) AS CC FROM ( {sql} ) "
                   , paramObj
                   );
            if (tempCount != null)
            {

                totalCount = (int)tempCount.CC;
            }

            var items = DBHelper.InstanceRMSDB.Query(
                 sql1
                , paramObj).ToList();

            var vnKey =DBHelper.InstanceRMSDB.QueryFirstOrDefault("select ROW_ID from ie_c_country_config WHERE MAIN_LANG = 'VN' ");

            var totalItem = new List<dynamic>();
            // if countryid=All => default order VN job first
            if (string.IsNullOrWhiteSpace(data.CountryID) || data.CountryID.Trim().ToUpper() == "ALL")
            {
                var vnJobLs = items.Where(r => ((string)(r.LANGUAGE_ID)).Contains("VN") || r.COUNTRY == vnKey?.ROW_ID).ToList();
                var otherJobLS = items.Where(r => vnJobLs.Select(t => t.ROW_ID).ToList().Contains(r.ROW_ID) == false).ToList();
                totalItem.AddRange(vnJobLs);
                totalItem.AddRange(otherJobLS);
            }
            else
                totalItem = items;

            var items1 = totalItem.Select(r =>
           {
               return new JobOfSeekRecruitment
               {
                   jobId = r.ROW_ID,
                   position = r.POSITIONS,
                   category = r.JOB_CATEGORIES,
                   education = r.MINIMUM_EDUCATION,
                   experience = r.EXPERIENCE,
                   qty = int.Parse(r.QTY.ToString()),
                   publicDate = r.UPDATE_TIME,
                   salary = r.SALARY,
                   isHotJob = r.IS_HOTJOB == "Y" ? true : false,
                   jobFullPlace = data.LanguageId.Trim().ToUpper() == "EN" ? r.EN_DESC + "-" + r.PLACE : (data.LanguageId.Trim().ToUpper() == "VN" ? r.VN_DESC + "-" + r.PLACE : r.CN_DESC + "-" + r.PLACE)

               };
           }).ToList();

            return new ElTableRes()
            {
                items = items1,
                totalCount = totalCount
            };



        }




        public List<JobOfSeekRecruitment> GetJobLsOfSeekRecruitmentNewest(JobOfSeekRecruitmentNewestReq data)
        {

            var countryIdLs = DBHelper.getRMSDBConnectObj().Query<IE_C_COUNTRY_CONFIG>(@"
              select * from ie_c_country_config WHERE  MAIN_LANG = :MAIN_LANG
            ", new { MAIN_LANG = data.LanguageId?.Trim()?.ToUpper() }).Select(r => r.ROW_ID).ToList();

            string formatDt = data.LanguageId == "VN" ? "DD/MM/YYYY" : "YYYY/MM/DD";

            if (data.Call.ToUpper() == CallType.DMZ.ToString())
            {
                var datLs = DBHelper.getRMSDBConnectObj().Query($@"  
                                 SELECT * FROM (
                                 SELECT   
                                    A.ROW_ID,
                                    A.POSITIONS ,
                                    A.PLACE, 
                                    A.JOB_CATEGORIES ,
                                    A.MINIMUM_EDUCATION,
                                    A.EXPERIENCE ,
                                    A.QTY ,
                                    TO_CHAR( DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ) ,'{formatDt}') AS UPDATE_TIME,
                                    A.SALARY ,
                                    A.IS_HOTJOB  ,
                                    B.VN_DESC,
                                    B.EN_DESC, 
                                    B.CN_DESC

                                    FROM 
                                    IE_C_RECRUITMENT_DATA A  
                                    LEFT JOIN IE_C_COUNTRY_CONFIG B ON  A.COUNTRY = B.ROW_ID
                                    WHERE 
                                        A.STATUS='Public' 
                                    AND A.IS_DELETE='N' 
                                    AND (  A.COUNTRY IN ({string.Join(",", countryIdLs.Select(r => "'" + r + "'").ToList())}) 
                                          OR   A.LANGUAGE_ID LIKE :LANGUAGE_ID     )

                                    AND A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                                    ORDER BY  TO_CHAR( DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ) ,'{formatDt}') DESC
                                    )
                                    WHERE ROWNUM<= {data.Count}
                               ", new
                {
                    LANGUAGE_ID = string.Format("%{0}%", data.LanguageId?.Trim()?.ToUpper()),
                    RECRUITMENT_NAME = data.RecruitmentName?.Trim(),
                });


                return datLs.Select(r => new JobOfSeekRecruitment
                {
                    jobId = r.ROW_ID,
                    position = r.POSITIONS,
                    category = r.JOB_CATEGORIES,
                    education = r.MINIMUM_EDUCATION,
                    experience = r.EXPERIENCE,
                    qty = int.Parse(r.QTY.ToString()),
                    publicDate = r.UPDATE_TIME,
                    salary = r.SALARY,
                    isHotJob = r.IS_HOTJOB == "Y" ? true : false,
                    jobFullPlace = data.LanguageId.Trim().ToUpper() == "EN" ? r.EN_DESC + "-" + r.PLACE : (data.LanguageId.Trim().ToUpper() == "VN" ? r.VN_DESC + "-" + r.PLACE : r.CN_DESC + "-" + r.PLACE)

                }).ToList();


            }
            else if (data.Call.ToUpper() == CallType.INTERNAL.ToString())
            {

                var datLs = DBHelper.getRMSDBConnectObj().Query($@"  
                                 SELECT * FROM (
                                 SELECT   
                                    A.ROW_ID,
                                    A.POSITIONS ,
                                    A.PLACE, 
                                    A.JOB_CATEGORIES ,
                                    A.MINIMUM_EDUCATION,
                                    A.EXPERIENCE ,
                                    A.QTY ,
                                    TO_CHAR( DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ) ,'{formatDt}') AS UPDATE_TIME,
                                    A.SALARY ,
                                    A.IS_HOTJOB  ,
                                    B.VN_DESC,
                                    B.EN_DESC, 
                                    B.CN_DESC

                                    FROM 
                                    IE_C_RECRUITMENT_DATA A  
                                    LEFT JOIN IE_C_COUNTRY_CONFIG B ON  A.COUNTRY = B.ROW_ID
                                    WHERE 
                                    A.IS_DELETE='N' 
                                    AND (  A.COUNTRY IN ({string.Join(",", countryIdLs.Select(r => "'" + r + "'").ToList())}) 
                                           OR   A.LANGUAGE_ID LIKE :LANGUAGE_ID    )
                                    AND A.RECRUITMENT_NAME= :RECRUITMENT_NAME
                                    ORDER BY   TO_CHAR( DECODE(A.UPDATE_TIME,NULL,A.CREATE_TIME,A.UPDATE_TIME ) ,'{formatDt}')  DESC
                                    )
                                    WHERE ROWNUM<= {data.Count}
                               ", new
                {
                    LANGUAGE_ID = string.Format("%{0}%", data.LanguageId?.Trim()?.ToUpper()),
                    RECRUITMENT_NAME = data.RecruitmentName?.Trim(),
                });


                return datLs.Select(r => new JobOfSeekRecruitment
                {
                    jobId = r.ROW_ID,
                    position = r.POSITIONS,
                    category = r.JOB_CATEGORIES,
                    education = r.MINIMUM_EDUCATION,
                    experience = r.EXPERIENCE,
                    qty = int.Parse(r.QTY.ToString()),
                    publicDate = r.UPDATE_TIME,
                    salary = r.SALARY,
                    isHotJob = r.IS_HOTJOB == "Y" ? true : false,
                    jobFullPlace = data.LanguageId.Trim().ToUpper() == "EN" ? r.EN_DESC + "-" + r.PLACE : (data.LanguageId.Trim().ToUpper() == "VN" ? r.VN_DESC + "-" + r.PLACE : r.CN_DESC + "-" + r.PLACE)

                }).ToList();

            }
            return new List<JobOfSeekRecruitment>();

        }





        public ElTableRes GetCompanyPosterAppLs(string applyNo, string applyEmp, string signEmp, int page, int pageSize)
        {
            string columns = @"
                                A.LANGUAGE_ID,
                                A.DESCRIPTION,
                                A.APPLY_TYPE,
                                A.APPLY_NO,
                                A.SORT,
                                A.STATUS AS PUBLIC_STATUS,
                                A.CREATE_EMP AS CREATED_APP_EMP,
                                TO_CHAR( A.CREATE_TIME,'YYYY/MM/DD HH24:MI:SS' ) AS CREATED_APP_DATE, 
                                A.IS_DELETE, 
                                B.APPLY_EMP,
                                B.APPLY_NAME ,
                                TO_CHAR( B.APPLY_TIME , 'YYYY/MM/DD HH24:MI:SS') AS APPLY_TIME ,
                                B.STATUS AS APP_STATUS,
                                B.SIGN_STATION ,
                                B.SIGN_STATION_NO AS FLOW_ID,
                                B.SIGN_EMP ,
                                D.SIGN_NAME ,
                                D.SIGN_MIAL , 
                                C.FILE_ID ,
                                C.FILE_NAME,
                                C.FILE_EXPANDED_NAME,
                                C.FILE_PATH,
                                C.FILE_SIZE,
                                C.FILE_DESC
                                ";
            int star_rownum = (page * pageSize) - pageSize + 1;
            int end_rownum = page * pageSize;
            string sql = "";

            sql = @" 
                        SELECT * FROM (
                        SELECT 
                                    {0} 
                                FROM   
                                IE_C_COMPANY_POSTER A,
                                IE_R_APPLY_ORDER B ,
                                IE_C_FILE_PATH_DATA C ,
                                IE_R_SIGN_LINKED D
                                WHERE 
                                A.IS_DELETE='N' 
                                AND A.APPLY_NO =  B.APPLY_NO(+)
                                AND A.FILE_ID = C.FILE_ID(+)
                                AND B.SIGN_STATION_NO = D.ROW_ID(+)
                         ) WHERE 
                            (    :APPLY_NO  IS NULL  OR APPLY_NO LIKE   :APPLY_NO  )
                            AND ( :APPLY_EMP IS NULL  OR APPLY_EMP  LIKE   :APPLY_EMP )
                            AND ( :SIGN_EMP IS NULL  OR SIGN_EMP  LIKE  :SIGN_EMP )
                          ";


            string sql1 = string.Format(@"
                   SELECT 
                    LANGUAGE_ID, DESCRIPTION, APPLY_TYPE, APPLY_NO, SORT, PUBLIC_STATUS, CREATED_APP_EMP, CREATED_APP_DATE, IS_DELETE, APPLY_EMP, APPLY_NAME, APPLY_TIME, APP_STATUS, SIGN_STATION, FLOW_ID, SIGN_EMP, SIGN_NAME, SIGN_MIAL, FILE_ID, FILE_NAME, FILE_EXPANDED_NAME, FILE_PATH, FILE_SIZE, FILE_DESC
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY  APPLY_NO ASC) AS ROW_NUMBER, K.*
                          FROM (
						      {0}
						  )  K ) E
                    WHERE E.ROW_NUMBER BETWEEN {1} AND {2} ORDER BY APPLY_TIME DESC
              ", string.Format(sql, columns), star_rownum, end_rownum);

            int totalCount = 0;
            var paramObj = new
            {
                APPLY_NO = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyNo) ? "" : applyNo?.Trim()?.ToUpper()),
                APPLY_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyEmp) ? "" : applyEmp?.Trim()?.ToUpper()),
                SIGN_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(signEmp) ? "" : signEmp?.Trim()?.ToUpper()),
            };

            var tempCount = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(string.Format("SELECT COUNT(1) AS CC FROM ({0}) ", string.Format(sql, columns))
                   , paramObj
                   );
            if (tempCount != null)
            {

                totalCount = (int)tempCount.CC;
            }

            var items = DBHelper.getRMSDBConnectObj().Query(
                 sql1
                , paramObj).ToList();

            var items1 = items.Select(r =>
            {
                int sortTemp;
                return new CompanyPosterApp
                {
                    LangId = r.LANGUAGE_ID,
                    description = r.DESCRIPTION,
                    applyType = r.APPLY_TYPE,
                    applyNo = r.APPLY_NO,
                    sort = int.TryParse(r.SORT.ToString(), out sortTemp) ? sortTemp : (int?)null,
                    publicStatus = r.PUBLIC_STATUS,
                    createdAppEmp = r.CREATED_APP_EMP,
                    createdAppDate = r.CREATED_APP_DATE,
                    isDelete = r.IS_DELETE,
                    applyEmp = r.APPLY_EMP,
                    applyName = r.APPLY_NAME,
                    applyTime = r.APPLY_TIME,
                    applyStatus = r.APP_STATUS,
                    signStationName = r.SIGN_STATION,
                    FlowId = r.FLOW_ID,
                    signEmp = r.SIGN_EMP,
                    signName = r.SIGN_NAME,
                    signMail = r.SIGN_MIAL,
                    fileId = r.FILE_ID,
                    fileName = r.FILE_NAME,
                    fileExpandedName = r.FILE_EXPANDED_NAME,
                    filePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                    fileSize = r.FILE_SIZE,
                    fileDesc = r.FILE_DESC,

                };
            }).ToList();

            return new ElTableRes()
            {
                items = items1,
                totalCount = totalCount
            };

        }


        public ElTableRes GetCompanyProfileAppLs(string applyNo, string applyEmp, string signEmp, int page, int pageSize)
        {
            string columns = @"
                                A.LANGUAGE_ID,
                                A.TEMPLATE_CONTENT,
                                A.TEMPPLATE_HIGHLIGHT,
                                A.COUNTRY,
                                A.APPLY_TYPE,
                                A.APPLY_NO,
                                A.SORT,
                                A.STATUS AS PUBLIC_STATUS,
                                A.CREATE_EMP AS CREATED_APP_EMP,
                                TO_CHAR( A.CREATE_TIME,'YYYY/MM/DD HH24:MI:SS' ) AS CREATED_APP_DATE, 
                                A.IS_DELETE, 
                                B.APPLY_EMP,
                                B.APPLY_NAME ,
                                TO_CHAR( B.APPLY_TIME , 'YYYY/MM/DD HH24:MI:SS') AS APPLY_TIME ,
                                B.STATUS AS APP_STATUS,
                                B.SIGN_STATION ,
                                B.SIGN_STATION_NO AS FLOW_ID,
                                B.SIGN_EMP ,
                                D.SIGN_NAME ,
                                D.SIGN_MIAL , 
                                C.FILE_ID ,
                                C.FILE_NAME,
                                C.FILE_EXPANDED_NAME,
                                C.FILE_PATH,
                                C.FILE_SIZE,
                                C.FILE_DESC
                                ";
            int star_rownum = (page * pageSize) - pageSize + 1;
            int end_rownum = page * pageSize;
            string sql = "";

            sql = @" 
                        SELECT * FROM (
                        SELECT 
                                    {0} 
                                FROM   
                                IE_C_COMPANY_PROFILE A,
                                IE_R_APPLY_ORDER B ,
                                IE_C_FILE_PATH_DATA C ,
                                IE_R_SIGN_LINKED D
                                WHERE 
                                A.IS_DELETE='N' 
                                AND A.APPLY_NO =  B.APPLY_NO(+)
                                AND A.FILE_ID = C.FILE_ID(+)
                                AND B.SIGN_STATION_NO = D.ROW_ID(+)
                         ) WHERE 
                            (    :APPLY_NO  IS NULL  OR APPLY_NO LIKE   :APPLY_NO  )
                            AND ( :APPLY_EMP IS NULL  OR APPLY_EMP  LIKE   :APPLY_EMP )
                            AND ( :SIGN_EMP IS NULL  OR SIGN_EMP  LIKE  :SIGN_EMP )
                          ";


            string sql1 = string.Format(@"
                   SELECT 
                    LANGUAGE_ID, TEMPLATE_CONTENT, TEMPPLATE_HIGHLIGHT, COUNTRY, APPLY_TYPE, APPLY_NO, SORT, PUBLIC_STATUS, CREATED_APP_EMP, CREATED_APP_DATE, IS_DELETE, APPLY_EMP, APPLY_NAME, APPLY_TIME, APP_STATUS, SIGN_STATION, FLOW_ID, SIGN_EMP, SIGN_NAME, SIGN_MIAL , FILE_ID, FILE_NAME, FILE_EXPANDED_NAME, FILE_PATH, FILE_SIZE, FILE_DESC
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY  APPLY_NO ASC) AS ROW_NUMBER, K.*
                          FROM (
						      {0}
						  )  K ) E
                    WHERE E.ROW_NUMBER BETWEEN {1} AND {2} ORDER BY APPLY_TIME DESC
              ", string.Format(sql, columns), star_rownum, end_rownum);

            int totalCount = 0;
            var paramObj = new
            {
                APPLY_NO = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyNo) ? "" : applyNo?.Trim()?.ToUpper()),
                APPLY_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyEmp) ? "" : applyEmp?.Trim()?.ToUpper()),
                SIGN_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(signEmp) ? "" : signEmp?.Trim()?.ToUpper()),
            };

            var tempCount = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(string.Format("SELECT COUNT(1) AS CC FROM ({0}) ", string.Format(sql, columns))
                   , paramObj
                   );
            if (tempCount != null)
            {

                totalCount = (int)tempCount.CC;
            }

            var items = DBHelper.getRMSDBConnectObj().Query(
                 sql1
                , paramObj).ToList();

            var items1 = items.Select(r =>
            {
                int sortTemp;
                return new CompanyProfileApp
                {
                    LangId = r.LANGUAGE_ID,
                    templateContent = r.TEMPLATE_CONTENT,
                    templateHighLight = r.TEMPPLATE_HIGHLIGHT,
                    country = r.COUNTRY,
                    applyType = r.APPLY_TYPE,
                    applyNo = r.APPLY_NO,
                    sort = int.TryParse(r.SORT.ToString(), out sortTemp) ? sortTemp : (int?)null,
                    publicStatus = r.PUBLIC_STATUS,
                    createdAppEmp = r.CREATED_APP_EMP,
                    createdAppDate = r.CREATED_APP_DATE,
                    isDelete = r.IS_DELETE,
                    applyEmp = r.APPLY_EMP,
                    applyName = r.APPLY_NAME,
                    applyTime = r.APPLY_TIME,
                    applyStatus = r.APP_STATUS,
                    signStationName = r.SIGN_STATION,
                    FlowId = r.FLOW_ID,
                    signEmp = r.SIGN_EMP,
                    signName = r.SIGN_NAME,
                    signMail = r.SIGN_MIAL,
                    fileId = r.FILE_ID,
                    fileName = r.FILE_NAME,
                    fileExpandedName = r.FILE_EXPANDED_NAME,
                    filePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                    fileSize = r.FILE_SIZE,
                    fileDesc = r.FILE_DESC,

                };
            }).ToList();

            return new ElTableRes()
            {
                items = items1,
                totalCount = totalCount
            };

        }



        public ElTableRes GetCompanyProgressAppLs(string applyNo, string applyEmp, string signEmp, int page, int pageSize)
        {
            string columns = @"
                                A.LANGUAGE_ID,
                                A.IMPORTANT_DATA,
                                A.YEAR,
                                A.APPLY_TYPE,
                                A.APPLY_NO,
                                A.STATUS AS PUBLIC_STATUS,
                                A.CREATE_EMP AS CREATED_APP_EMP,
                                TO_CHAR( A.CREATE_TIME,'YYYY/MM/DD HH24:MI:SS' ) AS CREATED_APP_DATE, 
                                A.IS_DELETE, 
                                B.APPLY_EMP,
                                B.APPLY_NAME ,
                                TO_CHAR( B.APPLY_TIME , 'YYYY/MM/DD HH24:MI:SS') AS APPLY_TIME ,
                                B.STATUS AS APP_STATUS,
                                B.SIGN_STATION ,
                                B.SIGN_STATION_NO AS FLOW_ID,
                                B.SIGN_EMP ,
                                D.SIGN_NAME ,
                                D.SIGN_MIAL , 
                                C.FILE_ID ,
                                C.FILE_NAME,
                                C.FILE_EXPANDED_NAME,
                                C.FILE_PATH,
                                C.FILE_SIZE,
                                C.FILE_DESC
                                
                                ";
            int star_rownum = (page * pageSize) - pageSize + 1;
            int end_rownum = page * pageSize;
            string sql = "";

            sql = @" 
                        SELECT * FROM (
                        SELECT 
                                    {0} 
                                FROM   
                                IE_C_COMPANY_PROGRESS A,
                                IE_R_APPLY_ORDER B ,
                                IE_C_FILE_PATH_DATA C ,
                                IE_R_SIGN_LINKED  D
                                WHERE 
                                A.IS_DELETE='N' 
                                AND  A.APPLY_NO =  B.APPLY_NO(+)
                                AND A.FILE_ID = C.FILE_ID(+)
                                AND B.SIGN_STATION_NO = D.ROW_ID(+)
                         ) WHERE 
                            (    :APPLY_NO  IS NULL  OR APPLY_NO LIKE   :APPLY_NO  )
                            AND ( :APPLY_EMP IS NULL  OR APPLY_EMP  LIKE   :APPLY_EMP )
                            AND ( :SIGN_EMP IS NULL  OR SIGN_EMP  LIKE  :SIGN_EMP )
                          ";


            string sql1 = string.Format(@"
                   SELECT 
                    LANGUAGE_ID, IMPORTANT_DATA, YEAR, APPLY_TYPE, APPLY_NO, PUBLIC_STATUS, CREATED_APP_EMP, CREATED_APP_DATE, IS_DELETE, APPLY_EMP, APPLY_NAME, APPLY_TIME, APP_STATUS, SIGN_STATION, FLOW_ID, SIGN_EMP, SIGN_NAME ,SIGN_MIAL  , FILE_ID, FILE_NAME, FILE_EXPANDED_NAME, FILE_PATH, FILE_SIZE, FILE_DESC
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY  APPLY_NO ASC) AS ROW_NUMBER, K.*
                          FROM (
						      {0}
						  )  K ) E
                    WHERE E.ROW_NUMBER BETWEEN {1} AND {2} ORDER BY APPLY_TIME DESC
              ", string.Format(sql, columns), star_rownum, end_rownum);

            int totalCount = 0;
            var paramObj = new
            {
                APPLY_NO = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyNo) ? "" : applyNo?.Trim()?.ToUpper()),
                APPLY_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyEmp) ? "" : applyEmp?.Trim()?.ToUpper()),
                SIGN_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(signEmp) ? "" : signEmp?.Trim()?.ToUpper()),
            };

            var tempCount = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(string.Format("SELECT COUNT(1) AS CC FROM ({0}) ", string.Format(sql, columns))
                   , paramObj
                   );
            if (tempCount != null)
            {

                totalCount = (int)tempCount.CC;
            }

            var items = DBHelper.getRMSDBConnectObj().Query(
                 sql1
                , paramObj).ToList();

            var items1 = items.Select(r =>
            {

                return new CompanyProgressApp
                {
                    LangId = r.LANGUAGE_ID,
                    importantData = r.IMPORTANT_DATA,
                    year = r.YEAR,
                    applyType = r.APPLY_TYPE,
                    applyNo = r.APPLY_NO,
                    publicStatus = r.PUBLIC_STATUS,
                    createdAppEmp = r.CREATED_APP_EMP,
                    createdAppDate = r.CREATED_APP_DATE,
                    isDelete = r.IS_DELETE,
                    applyEmp = r.APPLY_EMP,
                    applyName = r.APPLY_NAME,
                    applyTime = r.APPLY_TIME,
                    applyStatus = r.APP_STATUS,
                    signStationName = r.SIGN_STATION,
                    FlowId = r.FLOW_ID,
                    signEmp = r.SIGN_EMP,
                    signName = r.SIGN_NAME,
                    signMail = r.SIGN_MIAL,
                    fileId = r.FILE_ID,
                    fileName = r.FILE_NAME,
                    fileExpandedName = r.FILE_EXPANDED_NAME,
                    filePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                    fileSize = r.FILE_SIZE,
                    fileDesc = r.FILE_DESC,

                };
            }).ToList();

            return new ElTableRes()
            {
                items = items1,
                totalCount = totalCount
            };

        }




        public ElTableRes GetOurBusinessAppLs(string applyNo, string applyEmp, string signEmp, int page, int pageSize)
        {
            string columns = @"
                                A.LANGUAGE_ID,
                                A.TITLE,
                                A.CONTENT,
                                A.SORT,
                                A.APPLY_TYPE,
                                A.APPLY_NO,
                                A.STATUS AS PUBLIC_STATUS,
                                A.CREATE_EMP AS CREATED_APP_EMP,
                                TO_CHAR( A.CREATE_TIME,'YYYY/MM/DD HH24:MI:SS' ) AS CREATED_APP_DATE, 
                                A.IS_DELETE, 
                                B.APPLY_EMP,
                                B.APPLY_NAME ,
                                TO_CHAR( B.APPLY_TIME , 'YYYY/MM/DD HH24:MI:SS') AS APPLY_TIME ,
                                B.STATUS AS APP_STATUS,
                                B.SIGN_STATION ,
                                B.SIGN_STATION_NO AS FLOW_ID,
                                B.SIGN_EMP ,
                                D.SIGN_NAME ,
                                D.SIGN_MIAL , 
                                C.FILE_ID ,
                                C.FILE_NAME,
                                C.FILE_EXPANDED_NAME,
                                C.FILE_PATH,
                                C.FILE_SIZE,
                                C.FILE_DESC
                                
                                ";
            int star_rownum = (page * pageSize) - pageSize + 1;
            int end_rownum = page * pageSize;
            string sql = "";

            sql = @" 
                        SELECT * FROM (
                        SELECT 
                                    {0} 
                                FROM   
                                IE_C_OUR_BUSINESS A,
                                IE_R_APPLY_ORDER B ,
                                IE_C_FILE_PATH_DATA C ,
                                IE_R_SIGN_LINKED  D
                                WHERE 
                                A.IS_DELETE='N' 
                                AND  A.APPLY_NO =  B.APPLY_NO(+)
                                AND A.FILE_ID = C.FILE_ID(+)
                                AND B.SIGN_STATION_NO = D.ROW_ID(+)
                         ) WHERE 
                            (    :APPLY_NO  IS NULL  OR APPLY_NO LIKE   :APPLY_NO  )
                            AND ( :APPLY_EMP IS NULL  OR APPLY_EMP  LIKE   :APPLY_EMP )
                            AND ( :SIGN_EMP IS NULL  OR SIGN_EMP  LIKE  :SIGN_EMP )
                          ";


            string sql1 = string.Format(@"
                   SELECT 
                 LANGUAGE_ID, TITLE, CONTENT, SORT, APPLY_TYPE, APPLY_NO, PUBLIC_STATUS, CREATED_APP_EMP, CREATED_APP_DATE, IS_DELETE, APPLY_EMP, APPLY_NAME, APPLY_TIME, APP_STATUS, SIGN_STATION, FLOW_ID, SIGN_EMP, SIGN_NAME, SIGN_MIAL, FILE_ID, FILE_NAME, FILE_EXPANDED_NAME, FILE_PATH, FILE_SIZE, FILE_DESC
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY  APPLY_NO ASC) AS ROW_NUMBER, K.*
                          FROM (
						      {0}
						  )  K ) E
                    WHERE E.ROW_NUMBER BETWEEN {1} AND {2} ORDER BY APPLY_TIME DESC
              ", string.Format(sql, columns), star_rownum, end_rownum);

            int totalCount = 0;
            var paramObj = new
            {
                APPLY_NO = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyNo) ? "" : applyNo?.Trim()?.ToUpper()),
                APPLY_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyEmp) ? "" : applyEmp?.Trim()?.ToUpper()),
                SIGN_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(signEmp) ? "" : signEmp?.Trim()?.ToUpper()),
            };

            var tempCount = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(string.Format("SELECT COUNT(1) AS CC FROM ({0}) ", string.Format(sql, columns))
                   , paramObj
                   );
            if (tempCount != null)
            {

                totalCount = (int)tempCount.CC;
            }

            var items = DBHelper.getRMSDBConnectObj().Query(
                 sql1
                , paramObj).ToList();

            var items1 = items.Select(r =>
            {
                int sortTemp;
                return new OurBusinessApp
                {
                    LangId = r.LANGUAGE_ID,
                    title = r.TITLE,
                    content = r.CONTENT,
                    sort = int.TryParse(r.SORT.ToString(), out sortTemp) ? sortTemp : (int?)null,
                    applyType = r.APPLY_TYPE,
                    applyNo = r.APPLY_NO,
                    publicStatus = r.PUBLIC_STATUS,
                    createdAppEmp = r.CREATED_APP_EMP,
                    createdAppDate = r.CREATED_APP_DATE,
                    isDelete = r.IS_DELETE,
                    applyEmp = r.APPLY_EMP,
                    applyName = r.APPLY_NAME,
                    applyTime = r.APPLY_TIME,
                    applyStatus = r.APP_STATUS,
                    signStationName = r.SIGN_STATION,
                    FlowId = r.FLOW_ID,
                    signEmp = r.SIGN_EMP,
                    signName = r.SIGN_NAME,
                    signMail = r.SIGN_MIAL,
                    fileId = r.FILE_ID,
                    fileName = r.FILE_NAME,
                    fileExpandedName = r.FILE_EXPANDED_NAME,
                    filePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                    fileSize = r.FILE_SIZE,
                    fileDesc = r.FILE_DESC,

                };
            }).ToList();

            return new ElTableRes()
            {
                items = items1,
                totalCount = totalCount
            };

        }


        public ElTableRes GetSkilledWorkerAppLs(string applyNo, string applyEmp, string signEmp, int page, int pageSize)
        {
            string columns = @"
                                A.LANGUAGE_ID,
                                A.SITE ,
                                A.CONTENT,
                                A.COUNTRY,
                                A.RECRUITMENT_NAME,
                                A.SORT,
                                A.APPLY_TYPE,
                                A.APPLY_NO,
                                A.STATUS AS PUBLIC_STATUS,
                                A.CREATE_EMP AS CREATED_APP_EMP,
                                TO_CHAR( A.CREATE_TIME,'YYYY/MM/DD HH24:MI:SS' ) AS CREATED_APP_DATE, 
                                A.IS_DELETE, 
                                B.APPLY_EMP,
                                B.APPLY_NAME ,
                                TO_CHAR( B.APPLY_TIME , 'YYYY/MM/DD HH24:MI:SS') AS APPLY_TIME ,
                                B.STATUS AS APP_STATUS,
                                B.SIGN_STATION ,
                                B.SIGN_STATION_NO AS FLOW_ID,
                                B.SIGN_EMP ,
                                D.SIGN_NAME ,
                                D.SIGN_MIAL , 
                                C.FILE_ID ,
                                C.FILE_NAME,
                                C.FILE_EXPANDED_NAME,
                                C.FILE_PATH,
                                C.FILE_SIZE,
                                C.FILE_DESC
                                
                                ";
            int star_rownum = (page * pageSize) - pageSize + 1;
            int end_rownum = page * pageSize;
            string sql = "";

            sql = @" 
                        SELECT * FROM (
                        SELECT 
                                    {0} 
                                FROM   
                                IE_C_WORKER_WEL_RECRUITMENT A,
                                IE_R_APPLY_ORDER B ,
                                IE_C_FILE_PATH_DATA C ,
                                IE_R_SIGN_LINKED  D
                                WHERE 
                                A.IS_DELETE='N' 
                                AND  A.RECRUITMENT_NAME='技术工'
                                AND
                                A.APPLY_NO =  B.APPLY_NO(+)
                                AND A.FILE_ID = C.FILE_ID(+)
                                AND B.SIGN_STATION_NO = D.ROW_ID(+)
                         ) WHERE 
                            (    :APPLY_NO  IS NULL  OR APPLY_NO LIKE   :APPLY_NO  )
                            AND ( :APPLY_EMP IS NULL  OR APPLY_EMP  LIKE   :APPLY_EMP )
                            AND ( :SIGN_EMP IS NULL  OR SIGN_EMP  LIKE  :SIGN_EMP )
                          ";


            string sql1 = string.Format(@"
                   SELECT 
              LANGUAGE_ID, SITE, CONTENT, COUNTRY, RECRUITMENT_NAME, SORT, APPLY_TYPE, APPLY_NO, PUBLIC_STATUS, CREATED_APP_EMP, CREATED_APP_DATE, IS_DELETE, APPLY_EMP, APPLY_NAME, APPLY_TIME, APP_STATUS, SIGN_STATION, FLOW_ID, SIGN_EMP, SIGN_NAME, SIGN_MIAL, FILE_ID, FILE_NAME, FILE_EXPANDED_NAME, FILE_PATH, FILE_SIZE, FILE_DESC
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY  APPLY_NO ASC) AS ROW_NUMBER, K.*
                          FROM (
						      {0}
						  )  K ) E
                    WHERE E.ROW_NUMBER BETWEEN {1} AND {2} ORDER BY APPLY_TIME DESC
              ", string.Format(sql, columns), star_rownum, end_rownum);

            int totalCount = 0;
            var paramObj = new
            {
                APPLY_NO = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyNo) ? "" : applyNo?.Trim()?.ToUpper()),
                APPLY_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyEmp) ? "" : applyEmp?.Trim()?.ToUpper()),
                SIGN_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(signEmp) ? "" : signEmp?.Trim()?.ToUpper()),
            };

            var tempCount = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(string.Format("SELECT COUNT(1) AS CC FROM ({0}) ", string.Format(sql, columns))
                   , paramObj
                   );
            if (tempCount != null)
            {

                totalCount = (int)tempCount.CC;
            }

            var items = DBHelper.getRMSDBConnectObj().Query(
                 sql1
                , paramObj).ToList();

            var items1 = items.Select(r =>
            {
                int sortTemp;
                return new SkilledWorkerApp
                {
                    LangId = r.LANGUAGE_ID,
                    site = r.SITE,
                    content = r.CONTENT,
                    country = r.COUNTRY,
                    recruimentName = r.RECRUITMENT_NAME,
                    sort = int.TryParse(r.SORT.ToString(), out sortTemp) ? sortTemp : (int?)null,
                    applyType = r.APPLY_TYPE,
                    applyNo = r.APPLY_NO,
                    publicStatus = r.PUBLIC_STATUS,
                    createdAppEmp = r.CREATED_APP_EMP,
                    createdAppDate = r.CREATED_APP_DATE,
                    isDelete = r.IS_DELETE,
                    applyEmp = r.APPLY_EMP,
                    applyName = r.APPLY_NAME,
                    applyTime = r.APPLY_TIME,
                    applyStatus = r.APP_STATUS,
                    signStationName = r.SIGN_STATION,
                    FlowId = r.FLOW_ID,
                    signEmp = r.SIGN_EMP,
                    signName = r.SIGN_NAME,
                    signMail = r.SIGN_MIAL,
                    fileId = r.FILE_ID,
                    fileName = r.FILE_NAME,
                    fileExpandedName = r.FILE_EXPANDED_NAME,
                    filePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                    fileSize = r.FILE_SIZE,
                    fileDesc = r.FILE_DESC,

                };
            }).ToList();

            return new ElTableRes()
            {
                items = items1,
                totalCount = totalCount
            };

        }


        public ElTableRes GetPublicWelfareRecruitmentAppLs(string applyNo, string applyEmp, string signEmp, int page, int pageSize)
        {
            string columns = @"
                                A.LANGUAGE_ID,
                                A.SITE ,
                                A.CONTENT,
                                A.RECRUITMENT_NAME,
                                A.SORT,
                                A.APPLY_TYPE,
                                A.APPLY_NO,
                                A.STATUS AS PUBLIC_STATUS,
                                A.CREATE_EMP AS CREATED_APP_EMP,
                                TO_CHAR( A.CREATE_TIME,'YYYY/MM/DD HH24:MI:SS' ) AS CREATED_APP_DATE, 
                                A.IS_DELETE, 
                                B.APPLY_EMP,
                                B.APPLY_NAME ,
                                TO_CHAR( B.APPLY_TIME , 'YYYY/MM/DD HH24:MI:SS') AS APPLY_TIME ,
                                B.STATUS AS APP_STATUS,
                                B.SIGN_STATION ,
                                B.SIGN_STATION_NO AS FLOW_ID,
                                B.SIGN_EMP ,
                                D.SIGN_NAME ,
                                D.SIGN_MIAL , 
                                C.FILE_ID ,
                                C.FILE_NAME,
                                C.FILE_EXPANDED_NAME,
                                C.FILE_PATH,
                                C.FILE_SIZE,
                                C.FILE_DESC
                                
                                ";
            int star_rownum = (page * pageSize) - pageSize + 1;
            int end_rownum = page * pageSize;
            string sql = "";

            sql = @" 
                        SELECT * FROM (
                        SELECT 
                                    {0} 
                                FROM   
                                IE_C_WORKER_WEL_RECRUITMENT A,
                                IE_R_APPLY_ORDER B ,
                                IE_C_FILE_PATH_DATA C ,
                                IE_R_SIGN_LINKED  D
                                WHERE 
                                  A.IS_DELETE='N' 
                                AND    A.RECRUITMENT_NAME='公益招聘'
                                AND
                                A.APPLY_NO =  B.APPLY_NO(+)
                                AND A.FILE_ID = C.FILE_ID(+)
                                AND B.SIGN_STATION_NO = D.ROW_ID(+)
                         ) WHERE 
                            (    :APPLY_NO  IS NULL  OR APPLY_NO LIKE   :APPLY_NO  )
                            AND ( :APPLY_EMP IS NULL  OR APPLY_EMP  LIKE   :APPLY_EMP )
                            AND ( :SIGN_EMP IS NULL  OR SIGN_EMP  LIKE  :SIGN_EMP )
                          ";


            string sql1 = string.Format(@"
                   SELECT 
              LANGUAGE_ID, SITE, CONTENT, RECRUITMENT_NAME, SORT, APPLY_TYPE, APPLY_NO, PUBLIC_STATUS, CREATED_APP_EMP, CREATED_APP_DATE, IS_DELETE, APPLY_EMP, APPLY_NAME, APPLY_TIME, APP_STATUS, SIGN_STATION, FLOW_ID, SIGN_EMP, SIGN_NAME, SIGN_MIAL, FILE_ID, FILE_NAME, FILE_EXPANDED_NAME, FILE_PATH, FILE_SIZE, FILE_DESC
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY  APPLY_NO ASC) AS ROW_NUMBER, K.*
                          FROM (
						      {0}
						  )  K ) E
                    WHERE E.ROW_NUMBER BETWEEN {1} AND {2} ORDER BY APPLY_TIME DESC
              ", string.Format(sql, columns), star_rownum, end_rownum);

            int totalCount = 0;
            var paramObj = new
            {
                APPLY_NO = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyNo) ? "" : applyNo?.Trim()?.ToUpper()),
                APPLY_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyEmp) ? "" : applyEmp?.Trim()?.ToUpper()),
                SIGN_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(signEmp) ? "" : signEmp?.Trim()?.ToUpper()),
            };

            var tempCount = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(string.Format("SELECT COUNT(1) AS CC FROM ({0}) ", string.Format(sql, columns))
                   , paramObj
                   );
            if (tempCount != null)
            {

                totalCount = (int)tempCount.CC;
            }

            var items = DBHelper.getRMSDBConnectObj().Query(
                 sql1
                , paramObj).ToList();

            var items1 = items.Select(r =>
            {
                int sortTemp;
                return new PublicWelfareRecruitmentApp
                {
                    LangId = r.LANGUAGE_ID,
                    site = r.SITE,
                    content = r.CONTENT,
                    recruimentName = r.RECRUITMENT_NAME,
                    sort = int.TryParse(r.SORT.ToString(), out sortTemp) ? sortTemp : (int?)null,
                    applyType = r.APPLY_TYPE,
                    applyNo = r.APPLY_NO,
                    publicStatus = r.PUBLIC_STATUS,
                    createdAppEmp = r.CREATED_APP_EMP,
                    createdAppDate = r.CREATED_APP_DATE,
                    isDelete = r.IS_DELETE,
                    applyEmp = r.APPLY_EMP,
                    applyName = r.APPLY_NAME,
                    applyTime = r.APPLY_TIME,
                    applyStatus = r.APP_STATUS,
                    signStationName = r.SIGN_STATION,
                    FlowId = r.FLOW_ID,
                    signEmp = r.SIGN_EMP,
                    signName = r.SIGN_NAME,
                    signMail = r.SIGN_MIAL,
                    fileId = r.FILE_ID,
                    fileName = r.FILE_NAME,
                    fileExpandedName = r.FILE_EXPANDED_NAME,
                    filePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                    fileSize = r.FILE_SIZE,
                    fileDesc = r.FILE_DESC,

                };
            }).ToList();

            return new ElTableRes()
            {
                items = items1,
                totalCount = totalCount
            };

        }




        public ElTableRes GetPublicWelfareAppLs(string applyNo, string applyEmp, string signEmp, int page, int pageSize)
        {
            string columns = @"
                                A.LANGUAGE_ID,
                                A.TITLE ,
                                A.CONTENT,
                                A.SORT,
                                A.APPLY_TYPE,
                                A.APPLY_NO,
                                A.STATUS AS PUBLIC_STATUS,
                                A.CREATE_EMP AS CREATED_APP_EMP,
                                TO_CHAR( A.CREATE_TIME,'YYYY/MM/DD HH24:MI:SS' ) AS CREATED_APP_DATE, 
                                A.IS_DELETE, 
                                B.APPLY_EMP,
                                B.APPLY_NAME ,
                                TO_CHAR( B.APPLY_TIME , 'YYYY/MM/DD HH24:MI:SS') AS APPLY_TIME ,
                                B.STATUS AS APP_STATUS,
                                B.SIGN_STATION ,
                                B.SIGN_STATION_NO AS FLOW_ID,
                                B.SIGN_EMP ,
                                D.SIGN_NAME ,
                                D.SIGN_MIAL , 
                                C.FILE_ID ,
                                C.FILE_NAME,
                                C.FILE_EXPANDED_NAME,
                                C.FILE_PATH,
                                C.FILE_SIZE,
                                C.FILE_DESC
                                
                                ";
            int star_rownum = (page * pageSize) - pageSize + 1;
            int end_rownum = page * pageSize;
            string sql = "";

            sql = @" 
                        SELECT * FROM (
                        SELECT 
                                    {0} 
                                FROM   
                                IE_C_WELFARE_PUBLIC A,
                                IE_R_APPLY_ORDER B ,
                                IE_C_FILE_PATH_DATA C ,
                                IE_R_SIGN_LINKED  D
                                WHERE 
                                A.IS_DELETE='N' 
                                 AND   A.APPLY_NO =  B.APPLY_NO(+)
                                AND A.FILE_ID = C.FILE_ID(+)
                                AND B.SIGN_STATION_NO = D.ROW_ID(+)
                         ) WHERE 
                            (    :APPLY_NO  IS NULL  OR APPLY_NO LIKE   :APPLY_NO  )
                            AND ( :APPLY_EMP IS NULL  OR APPLY_EMP  LIKE   :APPLY_EMP )
                            AND ( :SIGN_EMP IS NULL  OR SIGN_EMP  LIKE  :SIGN_EMP )
                          ";


            string sql1 = string.Format(@"
                   SELECT 
                 LANGUAGE_ID, TITLE, CONTENT, SORT, APPLY_TYPE, APPLY_NO, PUBLIC_STATUS, CREATED_APP_EMP, CREATED_APP_DATE, IS_DELETE, APPLY_EMP, APPLY_NAME, APPLY_TIME, APP_STATUS, SIGN_STATION, FLOW_ID, SIGN_EMP, SIGN_NAME, SIGN_MIAL, FILE_ID, FILE_NAME, FILE_EXPANDED_NAME, FILE_PATH, FILE_SIZE, FILE_DESC
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY  APPLY_NO ASC) AS ROW_NUMBER, K.*
                          FROM (
						      {0}
						  )  K ) E
                    WHERE E.ROW_NUMBER BETWEEN {1} AND {2} ORDER BY APPLY_TIME DESC
              ", string.Format(sql, columns), star_rownum, end_rownum);

            int totalCount = 0;
            var paramObj = new
            {
                APPLY_NO = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyNo) ? "" : applyNo?.Trim()?.ToUpper()),
                APPLY_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(applyEmp) ? "" : applyEmp?.Trim()?.ToUpper()),
                SIGN_EMP = string.Format("%{0}%", string.IsNullOrWhiteSpace(signEmp) ? "" : signEmp?.Trim()?.ToUpper()),
            };

            var tempCount = DBHelper.getRMSDBConnectObj().QueryFirstOrDefault(string.Format("SELECT COUNT(1) AS CC FROM ({0}) ", string.Format(sql, columns))
                   , paramObj
                   );
            if (tempCount != null)
            {

                totalCount = (int)tempCount.CC;
            }

            var items = DBHelper.getRMSDBConnectObj().Query(
                 sql1
                , paramObj).ToList();

            var items1 = items.Select(r =>
            {
                int sortTemp;
                return new PublicWelfareApp
                {
                    LangId = r.LANGUAGE_ID,
                    title = r.TITLE,
                    content = r.CONTENT,
                    sort = int.TryParse(r.SORT.ToString(), out sortTemp) ? sortTemp : (int?)null,
                    applyType = r.APPLY_TYPE,
                    applyNo = r.APPLY_NO,
                    publicStatus = r.PUBLIC_STATUS,
                    createdAppEmp = r.CREATED_APP_EMP,
                    createdAppDate = r.CREATED_APP_DATE,
                    isDelete = r.IS_DELETE,
                    applyEmp = r.APPLY_EMP,
                    applyName = r.APPLY_NAME,
                    applyTime = r.APPLY_TIME,
                    applyStatus = r.APP_STATUS,
                    signStationName = r.SIGN_STATION,
                    FlowId = r.FLOW_ID,
                    signEmp = r.SIGN_EMP,
                    signName = r.SIGN_NAME,
                    signMail = r.SIGN_MIAL,
                    fileId = r.FILE_ID,
                    fileName = r.FILE_NAME,
                    fileExpandedName = r.FILE_EXPANDED_NAME,
                    filePath = Constant.INTERNAL_LINK + r.FILE_PATH,
                    fileSize = r.FILE_SIZE,
                    fileDesc = r.FILE_DESC,

                };
            }).ToList();

            return new ElTableRes()
            {
                items = items1,
                totalCount = totalCount
            };

        }


        public List<ContactWindow> GetContactWindow(string LanguageId)
        {
            var dataLs = DBHelper.getRMSDBConnectObj()
             .Query(@"
                    select 
                      ROW_ID , OWNER_NAME, CONTACT, EMAIL, LANGUAGE_ID, SORT, IS_DELETE, CREATE_EMP,
                     TO_CHAR( CREATE_TIME,'YYYY/MM/DD HH24:MI:SS') AS  CREATE_TIME,
                      UPDATE_EMP,
                     TO_CHAR( UPDATE_TIME,'YYYY/MM/DD HH24:MI:SS')  AS   UPDATE_TIME
                        FROM    IE_C_CONTACT_US WHERE LANGUAGE_ID = :LANGUAGE_ID
                        AND IS_DELETE='N'
                    ", new { LANGUAGE_ID = LanguageId?.Trim() }).ToList();
            var contactLs = dataLs.Select(r =>
            {
                int tempSort;
                return new ContactWindow
                {
                    contactName = r.OWNER_NAME,
                    contactPhone = r.CONTACT,
                    contactMail = r.EMAIL,
                    sort = int.TryParse(r.SORT, out tempSort) ? int.Parse(r.SORT) : 0,
                    rowId = r.ROW_ID,
                    languageId = r.LANGUAGE_ID,
                    isDelete = r.IS_DELETE,
                    createdEmp = r.CREATE_EMP,
                    createdDate = r.CREATE_TIME,
                    updatedEmp = r.UPDATE_EMP,
                    updatedDate = r.UPDATE_TIME

                };
            }).ToList();
            contactLs = contactLs.OrderBy(r => r.sort).ToList();
            return contactLs;
        }


        public bool AddFile(string FileName, string FileEx, string FileSize, string FileDes, string CreateEmp, ref string FileID, ref string err)
        {
            err = "";
            using (var cnn = DBHelper.getRMSDBConnectObj())
            {
                try
                {
                    cnn.Open();
                    FileID = cnn.QueryFirstOrDefault("SELECT GET_ROW_ID AS ROW_ID FROM dual").ROW_ID;
                    int rs = cnn.Execute(
                          @"
                Insert into IE_C_FILE_PATH_DATA (ROW_ID,FILE_GROUP_ID,FILE_ID,FILE_NAME,FILE_VERSION,FILE_EXPANDED_NAME,FILE_PATH,FILE_SIZE,FILE_DESC,SORT,STATUS,IS_DELETE,DATA1,DATA2,CREATE_EMP,CREATE_TIME,UPDATE_EMP,UPDATE_TIME)
                values (GET_ROW_ID(),'',:FILE_ID,:FILE_NAME,null,:FILE_EXPANDED_NAME,:FILE_PATH,:FILE_SIZE,:FILE_DESC,null,'online','N',null,null,:CREATE_EMP,SYSDATE,null,null)
                ", new
                          {
                              FILE_ID = FileID,
                              FILE_NAME = FileName,
                              FILE_EXPANDED_NAME = FileEx,
                              FILE_PATH = "/Files/" + FileID + FileEx,
                              FILE_SIZE = FileSize,
                              FILE_DESC = FileDes,
                              CREATE_EMP = CreateEmp
                          }
                        );
                    return rs > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    return false;
                }


            }


        }

        public object GetFileInfor(string FileID, string Call)
        {
            if (Call.ToUpper() == CallType.DMZ.ToString())
            {
                var dat = DBHelper.getRMSDBConnectObj()
                      .QueryFirstOrDefault(@"
                        SELECT FILE_ID,FILE_NAME,FILE_DESC, FILE_PATH , FILE_SIZE FROM IE_C_FILE_PATH_DATA
                        WHERE FILE_ID=:FILE_ID
                        ", new { FILE_ID = FileID });
                if (dat != null)
                {
                    return new
                    {
                        fileId = dat.FILE_ID,
                        fileName = dat.FILE_NAME,
                        fileDesc = dat.FILE_DESC,
                        filePath = Constant.DMZ_LINK + dat.FILE_PATH,
                        fileSize = dat.FILE_SIZE,
                    };
                }



            }
            else if (Call.ToUpper() == CallType.INTERNAL.ToString())
            {
                var dat = DBHelper.getRMSDBConnectObj()
                   .QueryFirstOrDefault(@"
                        SELECT FILE_ID,FILE_NAME,FILE_DESC, FILE_PATH ,FILE_SIZE FROM IE_C_FILE_PATH_DATA
                        WHERE FILE_ID=:FILE_ID
                        ", new { FILE_ID = FileID });
                if (dat != null)
                {
                    return new
                    {
                        fileId = dat.FILE_ID,
                        fileName = dat.FILE_NAME,
                        fileDesc = dat.FILE_DESC,
                        filePath = Constant.INTERNAL_LINK + dat.FILE_PATH,
                        fileSize = dat.FILE_SIZE,
                    };
                }
            }
            return null;
        }



    }
}