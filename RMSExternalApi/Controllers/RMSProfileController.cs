using RMSExternalApi.Businesses;
using RMSExternalApi.Commons;
using RMSExternalApi.Models;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RMSExternalApi.Controllers
{
    public class RMSProfileController : RMSAPIBaseController
    {
        #region Manage CV template


        /// <summary>
        /// Get data of CV Template
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public CusResponse1<CVTemplate> GetProfile()
        {
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<CVTemplate>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var dat = RMSCVTemplateBusiness.Instance.GetCVTemplate(curAcc?.F_MAIL);
                return new CusResponse1<CVTemplate>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dat
                };

            }
            catch (Exception ex)
            {

                LoggingLocal.SaveLog(LogType.Error, "[ER-202510220935] " + ex.Message + ex.StackTrace);
                return new CusResponse1<CVTemplate>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510220935]"   // ex.Message
                };
            }

        }


        /// <summary>
        /// Modify CV template
        /// </summary>
        /// <param name="cVTemplate"></param>
        /// <returns></returns>

        [HttpPost]
        public CusResponse1<object> ModifyProfile(CVTemplate cVTemplate)
        {
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };


                #region Check fields valid 
                if (cVTemplate == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    };
                if (string.IsNullOrWhiteSpace(cVTemplate.mobile))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("mobile is empty")
                    };
                if (string.IsNullOrWhiteSpace(cVTemplate.name))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("name is empty")
                    };
                if (string.IsNullOrWhiteSpace(cVTemplate.gender))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("gender is empty")
                    };
                if (new[] { "M", "F" }.Contains(cVTemplate.gender) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("gender invalid,valid is: M/F")
                    };


                if (string.IsNullOrWhiteSpace(cVTemplate.birthday))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("birthday is empty")
                    };
                DateTime dateTime1;
                var rsDate = DateTime.TryParseExact(cVTemplate.birthday?.Replace("-", "/"), "yyyy/MM/dd", new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateTime1);
                if (rsDate == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("birthday invalid, format:yyyy/mm/dd")
                    };


                if (string.IsNullOrWhiteSpace(cVTemplate.citizenId))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("citizenId is empty")
                    };

                if (string.IsNullOrWhiteSpace(cVTemplate.married))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("married is empty")
                    };
                if (new[] { "Y", "N" }.Contains(cVTemplate.married) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("married invalid, format:Y/N")
                    };


                if (string.IsNullOrWhiteSpace(cVTemplate.address))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("address is empty")
                    };
                #endregion Check fields valid 

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                cVTemplate.mail = curAcc?.F_MAIL;

                var rs = RMSCVTemplateBusiness.Instance.SaveCVTemplate(cVTemplate);

                return new CusResponse1<object>
                {
                    status = rs ? StatusType.success.ToString() : StatusType.error.ToString(),
                    message = rs ? StatusType.success.ToString() : StatusType.error.ToString(),

                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510221030] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510221030]"   // ex.Message
                };
            }

        }


        /// <summary>
        ///(Use Postman for debug) Upload file attach to Cv template , format payload :var fileName= btoa("your file name"); /* fileName in base64 format */   var form=new FormData(); form.append("fileName",fileName)  ; form.append("file",file[0]);
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<CusResponse1<object>> UploadFileAttachToProfile()
        {
            string fullPath = "";
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Please update email for your account")

                    };

                var rsBlock = await HandleBlockUploadMutilFilesForPdf();
                if (rsBlock.status == StatusType.error.ToString())
                    return rsBlock;

                string randomFileName = "CV_" + RMSBaseBusiness.Instance.GetNewRowID() + ".pdf";  // CV_  
                fullPath = Constant.FOLDER_CV_TEMPLATE_TEMP + @"\" + randomFileName;
                string tempFilePathIIS = rsBlock.data.ToString();
                File.Move(tempFilePathIIS, fullPath);

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();


                var totalResponse = new CusResponse1<object>();

            checkErrorPoint:
                if (totalResponse.status == StatusType.error.ToString())
                {
                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                    return totalResponse;
                }

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Form == null)
                {
                    totalResponse = new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    };
                    goto checkErrorPoint;
                }

                string fileNameBase64 = httpRequest.Form["fileName"]?.ToString();

                if (string.IsNullOrWhiteSpace(fileNameBase64))
                {
                    totalResponse = new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("fileName is empty")
                    };
                    goto checkErrorPoint;
                }

                string fileName = Util.DecodeBase64(fileNameBase64);

                var curCVTemplate = RMSCVTemplateBusiness.Instance.GetCVTemplate(curAcc?.F_MAIL);
                if (curCVTemplate == null)
                {
                    totalResponse = new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please save your profile first and then you can upload attached file")
                    };
                    goto checkErrorPoint;
                }


                FileInfo fileInfor = new FileInfo(fullPath);

                if (fileInfor.Length > 5242880) // 5mb
                {
                    totalResponse = new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("File size allow <=5mb")
                    };
                    goto checkErrorPoint;

                }

                if (CheckFileUploadValidHelper.IsValidPdfFile(fullPath) == false)
                {
                    totalResponse = new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Content of pdf file is invalid")
                    };
                    goto checkErrorPoint;

                }

                if (!string.IsNullOrWhiteSpace(curCVTemplate?.fileID))
                {
                    // xoa file cu trong ca 2 folder chinh va temp
                    string oldFilePath = Constant.FOLDER_CV_TEMPLATE_TEMP + @"\" + curCVTemplate?.fileID;
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);

                    string oldFilePath1 = Constant.FOLDER_CV_TEMPLATE + @"\" + curCVTemplate?.fileID;
                    if (File.Exists(oldFilePath1))
                        File.Delete(oldFilePath1);


                }

                if (RMSCVTemplateBusiness.Instance.UpdateTempFileAttachInforToCVTemplate
                      (randomFileName, fileName, curAcc?.F_MAIL) == false)
                {
                    totalResponse = new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Upload file fail")
                    };
                    goto checkErrorPoint;
                }

                return new CusResponse1<object>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = randomFileName,
                };

            }
            catch (Exception ex)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(fullPath) && File.Exists(fullPath))
                        File.Delete(fullPath);
                }
                catch
                {


                }
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510291613] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510291613]"   // ex.Message
                };

            }


        }


        #endregion Manage CV template

    }
}
