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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RMSExternalApi.Controllers
{
    public class RMSCVForSchoolJobController : RMSAPIBaseController
    {

        /// <summary>
        /// Get CV job for School job List of current account
        /// </summary>
        /// <returns></returns>
     
            
        [HttpGet]
        public CusResponse1<List<CVSchoolJobBaseModel>> GetCVSchoolJobLsOfJobMail()
        {
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<List<CVSchoolJobBaseModel>>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };
                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var dat = RMSCVSchoolJobBusiness.Instance.GetCVSchoolJobLsOfJobMail(curAcc?.F_MAIL);
                return new CusResponse1<List<CVSchoolJobBaseModel>>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dat
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510251715] " + ex.Message + ex.StackTrace);
                return new CusResponse1<List<CVSchoolJobBaseModel>>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510251715]"   // ex.Message
                };
            }


        }



        /// <summary>
        /// Get Cv job information for School job
        /// </summary>
        /// <param name="tpID">Doc No</param>
        /// <returns></returns>
      
        [HttpGet]
        public CusResponse1<CVForSchoolJob> GetCVForSchoolJob(string tpID)
        {
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<CVForSchoolJob>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };

                if (string.IsNullOrWhiteSpace(tpID))
                    return new CusResponse1<CVForSchoolJob>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    };
                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var tpInfor = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPool(tpID);
                if (tpInfor == null)
                    return new CusResponse1<CVForSchoolJob>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("No data")
                    };

                if (tpInfor.JOB_MAIL?.ToLower() != curAcc?.F_MAIL?.ToLower())
                    return new CusResponse1<CVForSchoolJob>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("This CV job not belongs you")
                    };

                var dat = RMSCVSchoolJobBusiness.Instance.GetCVForSchoolJob(tpID);

                return new CusResponse1<CVForSchoolJob>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dat
                };

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510251718] " + ex.Message + ex.StackTrace);
                return new CusResponse1<CVForSchoolJob>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510251718]"   // ex.Message
                };
            }


        }



        /// <summary>
        /// Delete CV infor for School job
        /// </summary>
        /// <param name="tpID">Doc no of job applied</param>
        /// <returns></returns>
      
        [HttpGet]
        public CusResponse1<object> DeleteCVForSchoolJob(string tpID)
        {
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var docInfor = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPool(tpID);
                if (docInfor == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("No information")
                    };
                if (docInfor.JOB_MAIL?.ToLower() != curAcc.F_MAIL?.ToLower())
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("This CV job not belongs you")
                    };


                if (docInfor.TP_STATUS == "待安排面试" && docInfor.IS_DELETE == "0")
                {  //trang thai luu nhap-> cho phep xoa
                    if (RMSCVSchoolJobBusiness.Instance.DeleteCVForSchoolJob(tpID))
                        return new CusResponse1<object>
                        {
                            status = StatusType.success.ToString(),
                            message = StatusType.success.ToString(),
                        };
                    else
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("Delete fail")
                        };
                }
                else
                { // don da gui di-> ko cho phep xoa
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("This CV job in signing process or closed")
                    };

                }
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510251723] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510251723]"   // ex.Message
                };

            }

        }



        /// <summary>
        /// Modify CV for School job 
        /// </summary>
        /// <param name="cVForSchool"></param>
        /// <returns></returns>
       
        [HttpPost]
        public CusResponse1<object> ModifyCVForSchoolJob(CVForSchoolJob cVForSchool)
        {
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get( "Please update email for your account")

                    };

                #region Check field valid
                if (cVForSchool == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    };

                if (string.IsNullOrWhiteSpace(cVForSchool.name))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("name is empty")
                    };

                if (string.IsNullOrWhiteSpace(cVForSchool.gender))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("gender is empty")
                    };
                if (new[] { "M", "F" }.Contains(cVForSchool.gender) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("gender is invalid,format:M/F")
                    };


                if (string.IsNullOrWhiteSpace(cVForSchool.ethnic))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("ethnic is empty")
                    };


                if (string.IsNullOrWhiteSpace(cVForSchool.mobile))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("mobile is empty")
                    };


                if (string.IsNullOrWhiteSpace(cVForSchool.school))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("school is empty")
                    };



                if (string.IsNullOrWhiteSpace(cVForSchool.qualification))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("qualification is empty")
                    };



                if (string.IsNullOrWhiteSpace(cVForSchool.citizenId))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("citizenId is empty")
                    };


                if (string.IsNullOrWhiteSpace(cVForSchool.birthday))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("birthday is empty")
                    };

                DateTime dateTime1;
                var rsDate = DateTime.TryParseExact(cVForSchool.birthday?.Replace("-", "/"), "yyyy/MM/dd", new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateTime1);
                if (rsDate == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("birthday invalid, format:yyyy/mm/dd")
                    };




                if (string.IsNullOrWhiteSpace(cVForSchool.major))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("major is empty")
                    };


                if (string.IsNullOrWhiteSpace(cVForSchool.hometown))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("hometown is empty")
                    };

                if (string.IsNullOrWhiteSpace(cVForSchool.email))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("email is empty")
                    };



                if (string.IsNullOrWhiteSpace(cVForSchool.positionWish))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("positionWish is empty")
                    };

                if (!string.IsNullOrWhiteSpace(cVForSchool.foreignLanguage))
                {
                    if (cVForSchool.foreignLanguageLevel < 0)
                    {
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("foreignLanguageLevel must be from 0 to 100 scores")
                        };
                    }
                }

                if (string.IsNullOrWhiteSpace(cVForSchool.jobID))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("jobID is empty")
                    };

                var jobObj= RMSBaseBusiness.Instance.GetDetailJob(cVForSchool.jobID, "DMZ", "VN");
                 if(jobObj==null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("jobID not exist")
                    };

                if (string.IsNullOrWhiteSpace(cVForSchool.jobName))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("jobName is empty")
                    };


                #endregion Check field valid


                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                if (string.IsNullOrWhiteSpace(cVForSchool.TPID))
                { // add new 
                    cVForSchool.jobMail = curAcc?.F_MAIL?.Trim()?.ToLower();
                    // kiem tra jobId nay da dk user nay dang ky truoc do chua?
                    var oldCVJob = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPoolByJobIDJobMail(cVForSchool.jobID, cVForSchool.jobMail);
                    if (oldCVJob != null)
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("You has registed this job before, please check job, if job still not in approve process, you can delete it and resubmit this job")
                        };
                    string TPID = "";
                    if (RMSCVSchoolJobBusiness.Instance.AddCVForSchoolJob(cVForSchool,ref TPID))
                        return new CusResponse1<object>
                        {
                            status = StatusType.success.ToString(),
                            message = StatusType.success.ToString(),
                            data= TPID
                        };
                    else
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("Register fail")
                        };


                }
                else  // update
                {
                    // kiem tra TPID co hop le ko? hop le : TPID co ton tai, TPID thuoc ve cur user, TPID dang ko trong qua trinh ky duyet
                    var oldCV = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPool(cVForSchool.TPID);
                    if (oldCV == null)
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("Your CV job not exist")
                        };
                    if (oldCV.JOB_MAIL?.ToLower() != curAcc.F_MAIL?.ToLower())
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("This CV job not belongs you")
                        };


                    if (oldCV.TP_STATUS == "待安排面试" && oldCV.IS_DELETE == "0")
                    {   // CV job nay dang o trang thai nhap=> cho phep update
                        cVForSchool.jobMail = curAcc.F_MAIL?.Trim()?.ToLower();

                        if (RMSCVSchoolJobBusiness.Instance.UpdateCVForSchoolJob(cVForSchool))
                            return new CusResponse1<object>
                            {
                                status = StatusType.success.ToString(),
                                message = StatusType.success.ToString(),
                                data= cVForSchool.TPID ,
                            };
                        else
                            return new CusResponse1<object>
                            {
                                status = StatusType.error.ToString(),
                                message = LangHelper.Instance.Get("Update CV job fail")
                            };


                    }
                    else
                    {    // CV dang trong quatrinh ky duyet or da close=> ko cho phep sua

                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("This CV job in signing process or closed")
                        };
                    }


                }

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510270814] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510270814]"   // ex.Message
                };

            }


        }





        /// <summary>
        ///(Use Postman for debug) Upload file CV pdf for School job , format payload: var form= new FormData();  form.append("TPID","Your doc No") ;form.append("file",file[0])
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<CusResponse1<object>> UploadFileCVForSchoolJob1()
        {
            string fullPath = "";
            try
            {
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

                    string randomFileName = "SJ_" + RMSBaseBusiness.Instance.GetNewRowID() + ".pdf";  // SJ_ : School job : key khoa de biet file nay thuoc school job
                    fullPath = Constant.TEMP_CV_FOLDER + @"\" + randomFileName;
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

                    string TPID = httpRequest.Form["TPID"]?.ToString();
                    //Kiem tra TPID co hop le ko cho upload file ko  ?
                    var cvJobInfor = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPool(TPID);
                    if (cvJobInfor == null)
                    {
                        totalResponse = new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("Your CV job not exist")
                        };
                        goto checkErrorPoint;
                    }
                    if (cvJobInfor?.JOB_MAIL?.ToLower() != curAcc.F_MAIL?.ToLower())
                    {
                        totalResponse = new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("This CV job not belongs you")
                        };
                        goto checkErrorPoint;
                    }


                    //TP_STATUS=  待安排面试
                    if (cvJobInfor.TP_STATUS == "待安排面试" && cvJobInfor.IS_DELETE == "0")
                    {
                        // CV job nay dang o trang thai nhap=> cho phep update
                    }
                    else    // CV dang trong quatrinh ky duyet or da close=> ko cho phep sua
                    {
                        totalResponse = new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("This CV job in signing process or closed")
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

                    if (!string.IsNullOrWhiteSpace(cvJobInfor.CV_TEMP_FILE))
                    {

                        // thiet lap de tranh loi bao mat : Path Traversal
                        cvJobInfor.CV_TEMP_FILE = Regex.Replace(cvJobInfor.CV_TEMP_FILE.Replace(".pdf", ""), "[^a-zA-Z0-9-_]", "") + ".pdf";
                        cvJobInfor.CV_TEMP_FILE = Path.Combine("", cvJobInfor.CV_TEMP_FILE);
                        //

                        string oldCVFilePath = Constant.TEMP_CV_FOLDER + @"\" + cvJobInfor.CV_TEMP_FILE;
                        if (System.IO.File.Exists(oldCVFilePath))
                            System.IO.File.Delete(oldCVFilePath);
                       
                        // xoa temp cv file trong db
                        RMSCVSchoolJobBusiness.Instance.UpdateCVTempFileNameForSchoolJob(TPID, "", curAcc?.F_MAIL);

                    }

                    if (RMSCVSchoolJobBusiness.Instance.UpdateCVTempFileNameForSchoolJob(TPID, randomFileName, curAcc.F_MAIL) == false)
                    {

                        totalResponse = new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("Upload file fail")
                        };
                        goto checkErrorPoint;

                    }

                    var fileInforMovedServerFile = RMSCVSchoolJobBusiness.Instance.GetFileAttachInforOfSchoolJobMovedToServerFile(TPID);
                    if (fileInforMovedServerFile != null)
                    {
                        //  goi api xoa file tren server file
                        string GroupID = fileInforMovedServerFile.FILE_ID;
                        var lsFileSvFile = RMSFileBusiness.Instance.GetFileInfo(GroupID);
                        if (lsFileSvFile?.Count > 0)
                        {
                            string fileId = lsFileSvFile.FirstOrDefault().fileId;
                            if (RMSFileBusiness.Instance.DeleteFileByFileID(fileId))
                            {
                                RMSCVSchoolJobBusiness.Instance.DeleteFileAttachInforOfSchoolJobMovedToServerFile(TPID);
                            }
                        }

                    }

                    return new CusResponse1<object>
                    {
                        status = StatusType.success.ToString(),
                        message = StatusType.success.ToString(),
                        data=TPID ,
                    };

                }
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
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510270827] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510270827]"   // ex.Message
                };

            }


        }


        /// <summary>
        /// Upload file CV pdf for School job , format payload: var form= new FormData();  form.append("TPID","Your doc No") ;form.append("file",file[0])
        /// </summary>
        /// <returns></returns>
      
        // [HttpPost]
        [NonAction]
        //public CusResponse1<object> UploadFileCVForSchoolJob()
        //{

        //    try
        //    {
        //        if (CheckHaveUpdatedEmailForAccount() == false)
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("Please update email for your account")

        //            };

        //        var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();

        //        var httpRequest = HttpContext.Current.Request;
        //        if (httpRequest.Form == null)
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("Request invalid")
        //            };

        //        string TPID = httpRequest.Form["TPID"]?.ToString();
        //        //Kiem tra TPID co hop le ko cho upload file ko  ?
        //        var cvJobInfor = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPool(TPID);
        //        if (cvJobInfor == null)
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("Your CV job not exist")
        //            };
        //        if (cvJobInfor?.JOB_MAIL?.ToLower() != curAcc.F_MAIL?.ToLower())
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("This CV job not belongs you")
        //            };


        //        //TP_STATUS=  待安排面试
        //        if (cvJobInfor.TP_STATUS == "待安排面试" && cvJobInfor.IS_DELETE == "0")
        //        {
        //            // CV job nay dang o trang thai nhap=> cho phep update
        //        }
        //        else    // CV dang trong quatrinh ky duyet or da close=> ko cho phep sua
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("This CV job in signing process or closed")
        //            };


        //        if (httpRequest?.Files?.Count <= 0)
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("There is not file attached")
        //            };

        //        var curFile = httpRequest.Files[0];
        //        if (curFile.ContentLength > 5242880) // 5mb
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("File size allow <=5mb")
        //            };

        //        if (curFile.FileName?.ToLower().EndsWith(".pdf") == false)
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("Only accept pdf file")
        //            };

        //        string randomFileName = "SJ_" + RMSBaseBusiness.Instance.GetNewRowID() + ".pdf";  // SJ_ : School job : key khoa de biet file nay thuoc school job
        //        string fullPath = Constant.TEMP_CV_FOLDER + @"\" + randomFileName;
        //        curFile.SaveAs(fullPath);
        //        if (CheckFileUploadValidHelper.IsValidPdfFile(fullPath) == false)
        //        {
        //            if (System.IO.File.Exists(fullPath))
        //                System.IO.File.Delete(fullPath);

        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("Content of pdf file is invalid")
        //            };

        //        }


        //        if (!string.IsNullOrWhiteSpace(cvJobInfor.CV_TEMP_FILE))
        //        {
        //            string oldCVFilePath = Constant.TEMP_CV_FOLDER + @"\" + cvJobInfor.CV_TEMP_FILE;
        //            if (System.IO.File.Exists(oldCVFilePath))
        //                System.IO.File.Delete(oldCVFilePath);

        //            // xoa temp cv file trong db
        //            RMSCVSchoolJobBusiness.Instance.UpdateCVTempFileNameForSchoolJob(TPID, "", curAcc?.F_MAIL);

        //        }

        //        if (RMSCVSchoolJobBusiness.Instance.UpdateCVTempFileNameForSchoolJob(TPID, randomFileName, curAcc.F_MAIL) == false)
        //        {
        //            if (System.IO.File.Exists(fullPath))
        //                System.IO.File.Delete(fullPath);
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("Upload file fail")
        //            };

        //        }

        //        var fileInforMovedServerFile = RMSCVSchoolJobBusiness.Instance.GetFileAttachInforOfSchoolJobMovedToServerFile(TPID);
        //        if (fileInforMovedServerFile != null)
        //        {
        //            //  goi api xoa file tren server file
        //            string GroupID = fileInforMovedServerFile.FILE_ID;
        //            var lsFileSvFile = RMSFileBusiness.Instance.GetFileInfo(GroupID);
        //            if (lsFileSvFile?.Count > 0)
        //            {
        //                string fileId = lsFileSvFile.FirstOrDefault().fileId;
        //                if (RMSFileBusiness.Instance.DeleteFileByFileID(fileId))
        //                {
        //                    RMSCVSchoolJobBusiness.Instance.DeleteFileAttachInforOfSchoolJobMovedToServerFile(TPID);
        //                }
        //            }

        //        }

        //        return new CusResponse1<object>
        //        {
        //            status = StatusType.success.ToString(),
        //            message = StatusType.success.ToString(),
        //        };

        //    }
        //    catch (Exception ex)
        //    {

        //        LoggingLocal.SaveLog(LogType.Error, "[ER-202510270827] " + ex.Message + ex.StackTrace);
        //        return new CusResponse1<object>
        //        {
        //            status = StatusType.error.ToString(),
        //            message = StatusType.error.ToString() + "[ER-202510270827]"   // ex.Message
        //        };

        //    }


        //}



        /// <summary>
        /// Delete file Cv for School Job
        /// </summary>
        /// <param name="TPID">Doc no</param>
        /// <returns></returns>
     
        [HttpGet]
        public CusResponse1<object> DeleteFileCVForSchoolJob(string TPID)
        {
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };


                if (string.IsNullOrWhiteSpace(TPID))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    };
                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var cvInfor = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPool(TPID);
                if (cvInfor == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Your CV job not exist")
                    };

                if (cvInfor?.JOB_MAIL?.ToLower() != curAcc.F_MAIL?.ToLower())
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("This CV job not belongs you")
                    };

                //TP_STATUS=  待安排面试
                if (cvInfor.TP_STATUS == "待安排面试" && cvInfor.IS_DELETE == "0")
                {
                    // CV job nay dang o trang thai nhap=> cho phep update
                }
                else    // CV dang trong quatrinh ky duyet or da close=> ko cho phep sua
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("This CV job in signing process or closed")
                    };


                if (!string.IsNullOrWhiteSpace(cvInfor.CV_TEMP_FILE))
                {
                    // thiet lap de tranh loi bao mat : Path Traversal
                    cvInfor.CV_TEMP_FILE = Regex.Replace(cvInfor.CV_TEMP_FILE.Replace(".pdf", ""), "[^a-zA-Z0-9-_]", "") + ".pdf";
                    cvInfor.CV_TEMP_FILE = Path.Combine("", cvInfor.CV_TEMP_FILE);
                    //


                    string oldCVFilePath = Constant.TEMP_CV_FOLDER + @"\" + cvInfor.CV_TEMP_FILE;
                    if (System.IO.File.Exists(oldCVFilePath))
                        System.IO.File.Delete(oldCVFilePath);

                    // xoa temp cv file trong db
                    RMSCVSchoolJobBusiness.Instance.UpdateCVTempFileNameForSchoolJob(TPID, "", curAcc?.F_MAIL);


                }

                var fileInforMovedServerFile = RMSCVSchoolJobBusiness.Instance.GetFileAttachInforOfSchoolJobMovedToServerFile(TPID);
                if (fileInforMovedServerFile != null)
                {
                    //  goi api xoa file tren server file
                    string GroupID = fileInforMovedServerFile.FILE_ID;
                    var lsFileSvFile = RMSFileBusiness.Instance.GetFileInfo(GroupID);
                    if (lsFileSvFile?.Count > 0)
                    {
                        string fileId = lsFileSvFile.FirstOrDefault().fileId;
                        if (RMSFileBusiness.Instance.DeleteFileByFileID(fileId))
                        {
                            RMSCVSchoolJobBusiness.Instance.DeleteFileAttachInforOfSchoolJobMovedToServerFile(TPID);
                        }
                    }
                }

                return new CusResponse1<object>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510270824] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510270824]"   // ex.Message
                };
            }
        }



    }
}
