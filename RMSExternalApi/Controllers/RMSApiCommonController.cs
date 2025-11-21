using RMSExternalApi.Businesses;
using RMSExternalApi.Commons;
using RMSExternalApi.Models;
using RMSExternalApi.Models.RMS;
using RMSExternalApi.Models.RMS.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RMSExternalApi.Controllers
{

   
    public class RMSApiController : RMSAPIBaseController
    {



        /// <summary>
        /// Api docnum :1 , Get list image of slide show on DMZ public web,
        /// Example : LanguageId = VN , KeyPageHeader = 社招海报 , Call= DMZ
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="KeyPageHeader">文件组ID:/位置:公司简介.首页海报/社招海报/校招海报/技术工海报/公益招聘海报/联系我们海报</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetHeaderPoster(string LanguageId, string KeyPageHeader, string Call)
        {


            try
            {
                if (string.IsNullOrWhiteSpace(LanguageId)
                    || string.IsNullOrWhiteSpace(KeyPageHeader)
                    || string.IsNullOrWhiteSpace(Call))

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message =  LangHelper.Instance.Get( "Request invalid")
                    });

                }


                var datLs = RMSBaseBusiness.Instance.GetHeaderPoster(LanguageId, KeyPageHeader, Call);

                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = datLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-001] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-001] "  // Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }

        }


        /// <summary>
        /// Api docnum: 3,  Get company Development RoadMap,
        /// Example: LanguageId =VN ,Call = DMZ
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetCompanyDevRoadMap(string LanguageId, string Call)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LanguageId)
                    || string.IsNullOrWhiteSpace(Call)
                  )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetCompanyDevRoadMap(LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-002] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-002]"   // Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }

        /// <summary>
        /// Api docnum: 2, Get Company Introduction,
        /// Example : LanguageId = VN , Call = DMZ
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetCompanyIntroduction(string LanguageId, string Call)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LanguageId)
                    || string.IsNullOrWhiteSpace(Call)
                  )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetCompanyIntroduction(LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-003] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-003]"  // Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        /// Api docnum :4 ,Get about us 1 ,
        /// Example:  LanguageId= VN , Call= DMZ
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetAboutUs1(string LanguageId, string Call)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LanguageId)
                    || string.IsNullOrWhiteSpace(Call)
                  )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetAboutUs1(LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-004] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-004]"  //Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }



        /// <summary>
        /// Api docnum :5 ,Get about us 2,
        /// Example: LanguageId = VN, Call =DMZ
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetAboutUs2(string LanguageId, string Call)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LanguageId)
                    || string.IsNullOrWhiteSpace(Call)
                  )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetAboutUs2(LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-005] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-005]"  // Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }

        /// <summary>
        ///  Api docnum :6, Get contact information ,
        /// Example :LanguageId= VN
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetContactInfor(string LanguageId)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(LanguageId)
                  )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message  = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetContactInfor(LanguageId);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-006] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-006]" //  "System error"

                });
            }
        }

        /// <summary>
        /// Api docnum: 7, GetCountry Ls Of Worker Recruitment ,
        /// Example : LanguageId = VN , Call = DMZ
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetCountryLsOfWorkerRecruitment(string LanguageId, string Call)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetCountryLsOfWorkerRecruitment(LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-007] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-007]" //Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }

        /// <summary>
        /// Api docnum: 8,  Get Content Factory Of Worker Recruitment,
        /// Example : LanguageId = VN , Call= DMZ , Country = Việt Nam
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <param name="Country">Việt Nam</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetContentFactoryOfWorkerRecruitment(string LanguageId, string Call, string Country)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(Call)
                  || string.IsNullOrWhiteSpace(Country)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetContentFactoryOfWorkerRecruitment(LanguageId, Call, Country);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-008] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-008]"// Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }

        }


        /// <summary>
        /// Api docnum: 9,  Get Content Slider2 Welfare Recruitment,
        /// Example : LanguageId=  VN, Call = DMZ
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetContentSlider2WelfareRecruitment(string LanguageId, string Call)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetContentSlider2WelfareRecruitment(LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-009] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-009]"  //Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }

        }


        /// <summary>
        ///  Api docnum: 10, GetContent 1 Welfare Recruitment,
        /// Example : LanguageId = VN, Call = DMZ
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetContent1WelfareRecruitment(string LanguageId, string Call)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetContent1WelfareRecruitment(LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-010] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-010]" // Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        /// Api docnum: 11, Get Country Ls Of Seek Recruitment ,
        /// Example: RecruitmentName= 社会招募 , LanguageId = VN , Call = DMZ
        /// </summary>
        /// <param name="RecruitmentName">社会招募/校园招聘</param>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetCountryLsOfSeekRecruitment(string RecruitmentName, string LanguageId, string Call)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(RecruitmentName)
                  || string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetCountryLsOfSeekRecruitment(RecruitmentName, LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-011] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-011]"  //Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        /// Api docnum: 12, Get Job Catergory Ls Of Seek Recruitment,
        /// Example : RecruitmentName= 社会招募 , LanguageId = VN , CountryId = 2024051611195900451189 , Call = DMZ
        /// </summary>
        /// <param name="RecruitmentName">社会招募/校园招聘</param>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="CountryId">Example:2024051611195900451189</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetJobCatergoryLsOfSeekRecruitment(string RecruitmentName, string LanguageId, string CountryId, string Call)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(RecruitmentName)
                  || string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(CountryId)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetJobCatergoryLsOfSeekRecruitment(RecruitmentName, LanguageId, CountryId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-012] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-012]" // Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        ///  Api docnum: 13, Get Experience Ls Of Seek Recruitment ,
        /// Example : RecruitmentName = 社会招募 , LanguageId = VN , CountryId = 2024051611195900451189  ,Call= DMZ
        /// </summary>
        /// <param name="RecruitmentName">社会招募/校园招聘</param>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="CountryId">Example:2024051611195900451189</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetExperienceLsOfSeekRecruitment(string RecruitmentName, string LanguageId, string CountryId, string Call)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(RecruitmentName)
                  || string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(CountryId)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetExperienceLsOfSeekRecruitment(RecruitmentName, LanguageId, CountryId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-013] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-013]" //Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        /// Api docnum: 14, Get Education Ls Of Seek Recruitment ,
        /// Example : RecruitmentName = 社会招募 ,LanguageId =  VN , CountryId = 2024051611195900451189 , Call = DMZ
        /// </summary>
        /// <param name="RecruitmentName">社会招募/校园招聘</param>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="CountryId">Example:2024051611195900451189</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetEducationLsOfSeekRecruitment(string RecruitmentName, string LanguageId, string CountryId, string Call)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(RecruitmentName)
                  || string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(CountryId)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetEducationLsOfSeekRecruitment(RecruitmentName, LanguageId, CountryId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-014] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-014]"// Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        /// Api docnum: 15, Get Hot Job Ls,
        /// Example : RecruitmentName = 社会招募 , LanguageId =  VN , Call = DMZ
        /// </summary>
        /// <param name="RecruitmentName">社会招募/校园招聘</param>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetHotJobLs(string RecruitmentName, string LanguageId, string Call)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(RecruitmentName)
                  || string.IsNullOrWhiteSpace(LanguageId)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetHotJobLs(RecruitmentName, LanguageId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-015] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-015]" // Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        /// Api docnum: 16,  Get Detail Job,
        /// Example : JobID = 2024051810341600451273 , Call =DMZ 
        /// </summary>
        /// <param name="JobID">Ex: 2024051810341600451273</param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <param name="LanguageId">CN/EN/VN</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetDetailJob(string JobID, string Call, string LanguageId)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(JobID)
                  || string.IsNullOrWhiteSpace(Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                var dataLs = RMSBaseBusiness.Instance.GetDetailJob(JobID, Call, LanguageId);

                if (dataLs != null)
                {

                    Logging.Instance.SaveLogAccessJobDetailFromDMZ(JobID);
                }

                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-016] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-016]"   // Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }

        /// <summary>
        ///  Api docnum: 17, Get Job Ls Of Seek Recruitment >>
        /// Params: RecruitmentName:社会招募/校园招聘 , 
        /// LanguageId: VN/EN/CN,
        /// Call: DMZ/INTERNAL,
        /// page:Order of page to display: example page = 1 , page= 2 ,
        /// pageSize:Total qty of rows will show on a page : example :  pageSize=5 ,pageSize=10 ,
        /// CountryID: example:2024051611195900451189 ,
        ///Exampple:
        /// {  RecruitmentName: '社会招募'  , LanguageId :'VN' , Call:'DMZ', page:1, pageSize:10, CountryID:'2024051611195900451189',Position :'',JobCategory :'', Experience :'', Education :''   }
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetJobLsOfSeekRecruitment(JobOfSeekRecruitmentReq request)
        {
            try
            {

                if (
                     (request == null)
                   || string.IsNullOrWhiteSpace(request.RecruitmentName)
                   || string.IsNullOrWhiteSpace(request.LanguageId)
                   || string.IsNullOrWhiteSpace(request.Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                if (request.page <= 0 || request.pageSize <= 0)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "page or pageSize must be > 0"
                    });
                }

                Logging.Instance.SaveLogAccessFromDMZ();
                var dataLs = RMSBaseBusiness.Instance.GetJobLsOfSeekRecruitment(request);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-017] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-017] "// request.Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        ///  Api docnum: 28, Get Job Ls Of Seek Recruitment newest ( top 5) 
        ///  Params: RecruitmentName:社会招募/校园招聘 ,
        ///  LanguageId: VN/EN/CN,
        ///  Call: DMZ/INTERNAL,
        ///  ,Count: Quantity jobs need to show: ex: Count=5,Count=10
        ///  Example: {  RecruitmentName:'社会招募',LanguageId:'VN', Call:'DMZ', Count:5 }
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetJobLsOfSeekRecruitmentNewest(JobOfSeekRecruitmentNewestReq request)
        {
            try
            {

                if (
                     (request == null)
                   || string.IsNullOrWhiteSpace(request.RecruitmentName)
                   || string.IsNullOrWhiteSpace(request.LanguageId)
                   || string.IsNullOrWhiteSpace(request.Call)
                )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                if (request.Count <= 0 || request.Count > 30)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "Param Count must be between 1 and 30"
                    });
                }

                var dataLs = RMSBaseBusiness.Instance.GetJobLsOfSeekRecruitmentNewest(request);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-018] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-018]"   // request.Call.ToUpper() == CallType.DMZ.ToString() ? "System error" : ex.Message

                });
            }
        }


        /// <summary>
        ///Api docnum: 18, Get Company Poster Application List 
        /// Params: applyNo :Application docNo  . example: 20240516140825004511 ,
        /// applyEmp:EmpNo of user created application .example :V1030398 ,
        /// signEmp:next Signer empNo . example :V1021214 ,
        /// page: Order of page to display: example page = 1 , page= 2  ,
        /// pageSize:Total qty of rows will show on a page : example :  pageSize=5 ,pageSize=10
        /// Example : {  applyNo:'20240516140825004511', applyEmp:'',signEmp:'',page:1, pageSize:10}
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetCompanyPosterAppLs(CompanyPosterAppReq request)
        {
            try
            {

                if (
                    (request == null)
               )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message =  LangHelper.Instance.Get("Request invalid")
                    });

                }

                if (request.page <= 0 || request.pageSize <= 0)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "page or pageSize must be > 0"
                    });
                }

                var dataLs = RMSBaseBusiness.Instance.GetCompanyPosterAppLs(request.applyNo, request.applyEmp, request.signEmp, request.page, request.pageSize);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-019] " + ex.Message + ex.StackTrace);


                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-019]"//  ex.Message

                });
            }
        }

        /// <summary>
        /// Api docnum: 19, Get Company Profile Application List
        /// Params: applyNo :Application docNo  . example: 202405170915400 ,
        /// applyEmp:EmpNo of user created application .example :V1030398 ,
        /// signEmp:next Signer empNo . example :V1021214 ,
        /// page: Order of page to display: example page = 1 , page= 2  ,
        /// pageSize:Total qty of rows will show on a page : example :  pageSize=5 ,pageSize=10
        /// Example : {  applyNo:'202405170915400', applyEmp:'',signEmp:'',page:1, pageSize:10}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetCompanyProfileAppLs(CompanyProfileAppReq request)
        {
            try
            {

                if (
                    (request == null)
               )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                if (request.page <= 0 || request.pageSize <= 0)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "page or pageSize must be > 0"
                    });
                }

                var dataLs = RMSBaseBusiness.Instance.GetCompanyProfileAppLs(request.applyNo, request.applyEmp, request.signEmp, request.page, request.pageSize);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-020] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-020]"// ex.Message

                });
            }
        }




        /// <summary>
        /// Api docnum: 20, Get Company Progress Application List
        /// Params: applyNo :Application docNo  . example: 202405161630210045122 ,
        /// applyEmp:EmpNo of user created application .example :V1030398 ,
        /// signEmp:next Signer empNo . example :V1021214 ,
        /// page: Order of page to display: example page = 1 , page= 2  ,
        /// pageSize:Total qty of rows will show on a page : example :  pageSize=5 ,pageSize=10
        /// Example : {  applyNo:'202405161630210045122', applyEmp:'',signEmp:'',page:1, pageSize:10}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetCompanyProgressAppLs(CompanyProfileAppReq request)
        {
            try
            {

                if (
                    (request == null)
               )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                if (request.page <= 0 || request.pageSize <= 0)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "page or pageSize must be > 0"
                    });
                }

                var dataLs = RMSBaseBusiness.Instance.GetCompanyProgressAppLs(request.applyNo, request.applyEmp, request.signEmp, request.page, request.pageSize);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-021] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-021]" // ex.Message

                });
            }
        }



        /// <summary>
        /// Api docnum: 21, Get Our Business Application List
        /// Params: applyNo :Application docNo  . example: 202405171029310045 ,
        /// applyEmp:EmpNo of user created application .example :V1030398 ,
        /// signEmp:next Signer empNo . example :V1021214 ,
        /// page: Order of page to display: example page = 1 , page= 2  ,
        /// pageSize:Total qty of rows will show on a page : example :  pageSize=5 ,pageSize=10
        /// Example : {  applyNo:'202405171029310045', applyEmp:'',signEmp:'',page:1, pageSize:10}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetOurBusinessAppLs(OurBusinessAppReq request)
        {
            try
            {

                if (
                    (request == null)
               )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                if (request.page <= 0 || request.pageSize <= 0)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "page or pageSize must be > 0"
                    });
                }

                var dataLs = RMSBaseBusiness.Instance.GetOurBusinessAppLs(request.applyNo, request.applyEmp, request.signEmp, request.page, request.pageSize);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-022] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-022]" // ex.Message

                });
            }
        }

        /// <summary>
        /// Api docnum: 22, Get Skilled Worker Application Lisst
        /// Params: applyNo :Application docNo  . example: 2024051714491600251 ,
        /// applyEmp:EmpNo of user created application .example :V1030398 ,
        /// signEmp:next Signer empNo . example :V1021214 ,
        /// page: Order of page to display: example page = 1 , page= 2  ,
        /// pageSize:Total qty of rows will show on a page : example :  pageSize=5 ,pageSize=10
        /// Example : {  applyNo:'2024051714491600251', applyEmp:'',signEmp:'',page:1, pageSize:10}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetSkilledWorkerAppLs(SkilledWorkerAppReq request)
        {
            try
            {

                if (
                    (request == null)
               )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Request invalid")
                    });

                }

                if (request.page <= 0 || request.pageSize <= 0)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "page or pageSize must be > 0"
                    });
                }

                var dataLs = RMSBaseBusiness.Instance.GetSkilledWorkerAppLs(request.applyNo, request.applyEmp, request.signEmp, request.page, request.pageSize);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-023] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-023]" //ex.Message

                });
            }
        }



        /// <summary>
        /// Api docnum: 23, Get Public Welfare Recruitment App Ls
        /// Params: applyNo :Application docNo  . example: 2024051808000451263 ,
        /// applyEmp:EmpNo of user created application .example :V1030398 ,
        /// signEmp:next Signer empNo . example :V1021214 ,
        /// page: Order of page to display: example page = 1 , page= 2  ,
        /// pageSize:Total qty of rows will show on a page : example :  pageSize=5 ,pageSize=10
        /// Example : {  applyNo:'2024051808000451263', applyEmp:'',signEmp:'',page:1, pageSize:10}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetPublicWelfareRecruitmentAppLs(SkilledWorkerAppReq request)
        {
            try
            {

                if (
                    (request == null)
               )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    });

                }

                if (request.page <= 0 || request.pageSize <= 0)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "page or pageSize must be > 0"
                    });
                }

                var dataLs = RMSBaseBusiness.Instance.GetPublicWelfareRecruitmentAppLs(request.applyNo, request.applyEmp, request.signEmp, request.page, request.pageSize);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-024] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-024]" //ex.Message

                });
            }
        }




        /// <summary>
        /// Api docnum: 24, Get Public Welfare Application list
        /// Params: applyNo :Application docNo  . example: 20240518091651268 ,
        /// applyEmp:EmpNo of user created application .example :V1030398 ,
        /// signEmp:next Signer empNo . example :V1021214 ,
        /// page: Order of page to display: example page = 1 , page= 2  ,
        /// pageSize:Total qty of rows will show on a page : example :  pageSize=5 ,pageSize=10
        /// Example : {  applyNo:'20240518091651268', applyEmp:'',signEmp:'',page:1, pageSize:10}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetPublicWelfareAppLs(PublicWelfareAppReq request)
        {
            try
            {

                if (
                    (request == null)
               )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    });

                }

                if (request.page <= 0 || request.pageSize <= 0)
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "page or pageSize must be > 0"
                    });
                }

                var dataLs = RMSBaseBusiness.Instance.GetPublicWelfareAppLs(request.applyNo, request.applyEmp, request.signEmp, request.page, request.pageSize);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-025] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-025]"//  ex.Message

                });
            }
        }



        /// <summary>
        /// Api docnum: 25, Get Contact Window list
        /// </summary>
        /// <param name="LanguageId">VN/EN/CN</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetContactWindow(string LanguageId)
        {
            try
            {

                if (
                    (string.IsNullOrWhiteSpace(LanguageId))
               )

                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    });

                }



                var dataLs = RMSBaseBusiness.Instance.GetContactWindow(LanguageId);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dataLs
                });

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-026] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-026]" // ex.Message

                });
            }
        }


        /// <summary> 
        /// Api docnum: 26, Upload file 
        /// Example Request Body :    1. FormData: your data file need upload
        ///                      :    2. Query param :createEmp : Employee no upload file
        /// </summary>
        /// <param name="createEmp">Employee no upload file</param>
        /// <returns></returns>
        [NonAction]
        public IHttpActionResult UploadFile(string createEmp)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(createEmp))
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("No create emp in request")
                    });
                }
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                {


                    var curFile = httpRequest.Files[0];
                    if (curFile.ContentLength <= 0)
                    {
                        return Ok(new CusResponse
                        {
                            status = StatusType.error.ToString(),
                            message = "File no content"
                        });
                    }
                    if (curFile.ContentLength > 335544320) // 32 mb
                    {
                        return Ok(new CusResponse
                        {
                            status = StatusType.error.ToString(),
                            message = "Max upload file size allowed is 30 mb"
                        });
                    }

                    string extension = curFile.FileName.Substring(curFile.FileName.LastIndexOf(".")).ToLower();
                    string fileName = curFile.FileName;
                    string fileSize = (Math.Round(curFile.ContentLength / (1024.0 * 1024.0), 2)).ToString() + " MB";
                    string fileDes = fileName;
                    string FileId = "";
                    string err = "";
                    var rs = RMSBaseBusiness.Instance.AddFile(fileName, extension, fileSize, fileDes, createEmp, ref FileId, ref err);
                    if (!string.IsNullOrWhiteSpace(err))
                    {
                        return Ok(new CusResponse
                        {
                            status = StatusType.error.ToString(),
                            message = err,
                        });
                    }
                    if (rs)
                    {
                        string localfilePath = Constant.FILE_FOLDER + FileId + extension;
                        curFile.SaveAs(localfilePath);
                        return Ok(new CusResponse
                        {
                            status = StatusType.success.ToString(),
                            message = StatusType.success.ToString(),
                            data = new { FileId }
                        });
                    }
                    else
                        return Ok(new CusResponse
                        {
                            status = StatusType.error.ToString(),
                            message = "Upload Fail"
                        });

                }
                else
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = "No file in request"
                    });
                }
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-027] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + " [ER-027]"// ex.Message
                });
            }

        }

        /// <summary>
        /// Api docnum: 27, Get File information, Example : FileId =  2024061910171600451585 , Call =DMZ
        /// </summary>
        /// <param name="FileId">FileId, example :FileId =  2024061910171600451585 </param>
        /// <param name="Call">DMZ/INTERNAL</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetFileInfor(string FileId, string Call)
        {


            try
            {
                if (string.IsNullOrWhiteSpace(FileId)
            || string.IsNullOrWhiteSpace(Call))
                {
                    return Ok(new CusResponse
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    });
                }

                var data = RMSBaseBusiness.Instance.GetFileInfor(FileId, Call);
                return Ok(new CusResponse
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = data
                });
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-028] " + ex.Message + ex.StackTrace);

                return Ok(new CusResponse
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-028]"   // ex.Message
                });

            }


        }




 
    }




}
