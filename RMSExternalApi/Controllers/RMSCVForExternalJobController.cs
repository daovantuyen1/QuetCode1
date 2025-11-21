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
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RMSExternalApi.Controllers
{
    public class RMSCVForExternalJobController : RMSAPIBaseController
    {

        #region  Manage CV for External job

        /// <summary>
        /// Get CV job for External job List of current account
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public CusResponse1<List<CVExternalJobBaseModel>> GetCVExternalJobLsOfJobMail()
        {
            try
            {


                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<List<CVExternalJobBaseModel>>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account"),

                    };

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();

                var dat = RMSCVExternalJobBusiness.Instance.GetCVExternalJobLsOfJobMail(curAcc?.F_MAIL);
                return new CusResponse1<List<CVExternalJobBaseModel>>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dat
                };
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510241713] " + ex.Message + ex.StackTrace);
                return new CusResponse1<List<CVExternalJobBaseModel>>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510241713]"   // ex.Message
                };
            }


        }

        /// <summary>
        /// Get Cv job information for External job
        /// </summary>
        /// <param name="tpID">Doc no</param>
        /// <returns></returns>

        [HttpGet]
        public CusResponse1<CVForExternalJob> GetCVForExternalJob(string tpID)
        {
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<CVForExternalJob>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };

                if (string.IsNullOrWhiteSpace(tpID))
                    return new CusResponse1<CVForExternalJob>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    };
                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                var tpInfor = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPool(tpID);
                if (tpInfor == null)
                    return new CusResponse1<CVForExternalJob>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("No data")
                    };

                if (tpInfor.JOB_MAIL?.ToLower() != curAcc?.F_MAIL?.ToLower())
                    return new CusResponse1<CVForExternalJob>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("This CV job not belongs you")
                    };

                var dat = RMSCVExternalJobBusiness.Instance.GetCVForExternalJob(tpID);

                return new CusResponse1<CVForExternalJob>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = dat
                };

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510241439] " + ex.Message + ex.StackTrace);
                return new CusResponse1<CVForExternalJob>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510241439]"   // ex.Message
                };
            }


        }


        /// <summary>
        /// Delete CV infor for External job
        /// </summary>
        /// <param name="tpID">Doc no</param>
        /// <returns></returns>

        [HttpGet]
        public CusResponse1<object> DeleteCVForExternalJob(string tpID)
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
                var docInfor = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPool(tpID);
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


                if (docInfor.EDITSTATUS == "-1" && docInfor.ISDELETE == "0")
                {  //trang thai luu nhap-> cho phep xoa
                    if (RMSCVExternalJobBusiness.Instance.DeleteCVForExternalJob(tpID))
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
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510221655] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510221655]"   // ex.Message
                };

            }

        }


        /// <summary>
        /// Modify CV for external job 
        /// </summary>
        /// <param name="cVForExternalJob"></param>
        /// <returns></returns>

        [HttpPost]
        public CusResponse1<object> ModifyCVForExternalJob(CVForExternalJob cVForExternalJob)
        {
            try
            {

                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };

                #region Check field valid
                if (cVForExternalJob == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Request invalid")
                    };

                if (string.IsNullOrWhiteSpace(cVForExternalJob.mail))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("mail is empty")
                    };
                if (Util.IsValidEmail(cVForExternalJob.mail) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("mail is invalid")
                    };

                if (string.IsNullOrWhiteSpace(cVForExternalJob.mobile))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("mobile is empty")
                    };

                if (string.IsNullOrWhiteSpace(cVForExternalJob.name))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("name is empty")
                    };
                if (string.IsNullOrWhiteSpace(cVForExternalJob.gender))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("gender is empty")
                    };
                if (new[] { "M", "F" }.Contains(cVForExternalJob.gender) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("gender is invalid,format:M/F")
                    };
                if (string.IsNullOrWhiteSpace(cVForExternalJob.birthday))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("birthday is empty")
                    };


                DateTime dateTime1;
                var rsDate = DateTime.TryParseExact(cVForExternalJob.birthday?.Replace("-", "/"), "yyyy/MM/dd", new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateTime1);
                if (rsDate == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("birthday invalid, format:yyyy/mm/dd")
                    };

                if (string.IsNullOrWhiteSpace(cVForExternalJob.citizenId))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("citizenId is empty")
                    };

                if (string.IsNullOrWhiteSpace(cVForExternalJob.married))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("married is empty")
                    };

                if (new[] { "Y", "N" }.Contains(cVForExternalJob.married) == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("married is invalid,format:Y/N")
                    };


                if (string.IsNullOrWhiteSpace(cVForExternalJob.address))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("address is empty")
                    };

                if (string.IsNullOrWhiteSpace(cVForExternalJob.monthlySalaryWish))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("monthlySalaryWish is empty")
                    };


                if (string.IsNullOrWhiteSpace(cVForExternalJob.positionWish))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("positionWish is empty")
                    };

                if (cVForExternalJob.educations?.Count <= 0)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("educations is empty")
                    };

                var eduLevelLs = RMSConfigDataBusiness.Instance.GetEducationLevelLs()?.data;


                foreach (var edu in cVForExternalJob.educations)
                {
                    if (string.IsNullOrWhiteSpace(edu.startTime)
                            || string.IsNullOrWhiteSpace(edu.endTime)
                            || string.IsNullOrWhiteSpace(edu.school)
                            || string.IsNullOrWhiteSpace(edu.eduQualify)
                        )
                    {
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("educations is invalid")
                        };
                    }

                    if (eduLevelLs.Select(r => r.value).Contains(edu.eduQualify) == false)
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("Education qualification is invalid")
                        };

                    DateTime startDate;
                    var rsStartDate = DateTime.TryParseExact(edu.startTime, "yyyy-MM", new CultureInfo("en-US"), DateTimeStyles.None, out startDate);
                    if (rsStartDate == false)
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("eduction start time is invalid")
                        };

                    DateTime endDate;
                    var rsEndDate = DateTime.TryParseExact(edu.endTime, "yyyy-MM", new CultureInfo("en-US"), DateTimeStyles.None, out endDate);
                    if (rsEndDate == false)
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("eduction end time is invalid")
                        };


                }

                if (cVForExternalJob.jobExperiences?.Count <= 0)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("jobExperiences is empty")
                    };


                if (cVForExternalJob.jobExperiences?.Count > 0)
                {
                    foreach (var job in cVForExternalJob.jobExperiences)
                    {
                        if (string.IsNullOrWhiteSpace(job.startTime)
                           || string.IsNullOrWhiteSpace(job.endTime)
                            || string.IsNullOrWhiteSpace(job.company)
                            || string.IsNullOrWhiteSpace(job.position)
                            || string.IsNullOrWhiteSpace(job.description)
                            )
                        {
                            return new CusResponse1<object>
                            {
                                status = StatusType.error.ToString(),
                                message = "jobExperiences is invalid"
                            };
                        }

                        DateTime startDate;
                        var rsStartDate = DateTime.TryParseExact(job.startTime, "yyyy-MM", new CultureInfo("en-US"), DateTimeStyles.None, out startDate);
                        if (rsStartDate == false)
                            return new CusResponse1<object>
                            {
                                status = StatusType.error.ToString(),
                                message = LangHelper.Instance.Get("job Experiences start time is invalid")
                            };



                        DateTime endDate;
                        var rsEndDate = DateTime.TryParseExact(job.endTime, "yyyy-MM", new CultureInfo("en-US"), DateTimeStyles.None, out endDate);
                        if (rsEndDate == false)
                            return new CusResponse1<object>
                            {
                                status = StatusType.error.ToString(),
                                message = LangHelper.Instance.Get("job Experiences end time is invalid")
                            };





                    }

                }

                if (cVForExternalJob.projectExperiences?.Count > 0)
                {
                    foreach (var pro in cVForExternalJob.projectExperiences)
                    {
                        if (string.IsNullOrWhiteSpace(pro.startTime)
                           || string.IsNullOrWhiteSpace(pro.endTime)
                            || string.IsNullOrWhiteSpace(pro.name)
                            || string.IsNullOrWhiteSpace(pro.description)
                            )
                        {
                            return new CusResponse1<object>
                            {
                                status = StatusType.error.ToString(),
                                message = "projectExperiences is invalid"
                            };
                        }


                        DateTime startDate;
                        var rsStartDate = DateTime.TryParseExact(pro.startTime, "yyyy-MM", new CultureInfo("en-US"), DateTimeStyles.None, out startDate);
                        if (rsStartDate == false)
                            return new CusResponse1<object>
                            {
                                status = StatusType.error.ToString(),
                                message = LangHelper.Instance.Get("project Experiences start time is invalid")
                            };



                        DateTime endDate;
                        var rsEndDate = DateTime.TryParseExact(pro.endTime, "yyyy-MM", new CultureInfo("en-US"), DateTimeStyles.None, out endDate);
                        if (rsEndDate == false)
                            return new CusResponse1<object>
                            {
                                status = StatusType.error.ToString(),
                                message = LangHelper.Instance.Get("project Experiences end time is invalid")
                            };




                    }

                }

                if (string.IsNullOrWhiteSpace(cVForExternalJob.jobID))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("jobID is empty")
                    };

                var jobObj = RMSBaseBusiness.Instance.GetDetailJob(cVForExternalJob.jobID, "DMZ", "VN");
                if (jobObj == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("jobID not exist")
                    };

                if (string.IsNullOrWhiteSpace(cVForExternalJob.jobName))
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("jobName is empty")
                    };

                //if (!string.IsNullOrWhiteSpace(cVForExternalJob.CVDetail))
                //{
                //    // diem cho check CVDetail la valid base 64 of image

                //}


                #endregion Check field valid

                var curAcc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                if (string.IsNullOrWhiteSpace(cVForExternalJob.TPID))
                { // add new 
                    cVForExternalJob.jobMail = curAcc?.F_MAIL?.Trim()?.ToLower();
                    // kiem tra jobId nay da dk user nay dang ky truoc do chua?
                    var oldCVJob = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPoolByJobIDJobMail(cVForExternalJob.jobID, cVForExternalJob.jobMail);
                    if (oldCVJob != null)
                        return new CusResponse1<object>
                        {
                            status = StatusType.error.ToString(),
                            message = LangHelper.Instance.Get("You has registed this job before, please check job, if job still not in approve process, you can delete it and resubmit this job")
                        };
                    string TPID = "";
                    if (RMSCVExternalJobBusiness.Instance.AddCVForExternalJob(cVForExternalJob, ref TPID))
                        return new CusResponse1<object>
                        {
                            status = StatusType.success.ToString(),
                            message = StatusType.success.ToString(),
                            data = TPID
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
                    var oldCV = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPool(cVForExternalJob.TPID);
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

                    if (oldCV.EDITSTATUS == "-1" && oldCV.ISDELETE == "0")
                    {   // CV job nay dang o trang thai nhap=> cho phep update
                        cVForExternalJob.jobMail = curAcc.F_MAIL?.Trim()?.ToLower();

                        if (RMSCVExternalJobBusiness.Instance.UpdateCVForExternalJob(cVForExternalJob))
                            return new CusResponse1<object>
                            {
                                status = StatusType.success.ToString(),
                                message = StatusType.success.ToString(),
                                data = cVForExternalJob.TPID,
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
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510221656] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510221656]"   // ex.Message
                };

            }


        }





        /// <summary>
        ///(Use Postman for debug)  Upload file CV pdf for External job , format payload: var form= new FormData();  form.append("TPID","Your doc No") ;form.append("file",file[0])
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<CusResponse1<object>> UploadFileCVForExternalJob1()
        {

            string fullPath = "";
            try
            {
                if (CheckHaveUpdatedEmailForAccount() == false)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Please update email for your account")

                    };

                var rsBlock = await HandleBlockUploadMutilFilesForPdf();
                if (rsBlock.status == StatusType.error.ToString())
                    return rsBlock;

                string randomFileName = "EJ_" + RMSBaseBusiness.Instance.GetNewRowID() + ".pdf";  // EJ_ : External job : key khoa de biet file nay thuoc external job
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
                var cvJobInfor = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPool(TPID);
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


                if (cvJobInfor.EDITSTATUS == "-1" && cvJobInfor.ISDELETE == "0")
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


                if (!string.IsNullOrWhiteSpace(cvJobInfor?.CV_TEMP_FILE))
                {

                    // thiet lap de tranh loi bao mat : Path Traversal
                    cvJobInfor.CV_TEMP_FILE = Regex.Replace(cvJobInfor.CV_TEMP_FILE.Replace(".pdf", ""), "[^a-zA-Z0-9-_]", "")+ ".pdf";
                    cvJobInfor.CV_TEMP_FILE = Path.Combine("", cvJobInfor.CV_TEMP_FILE);
                    //

                    string oldCVFilePath = Constant.TEMP_CV_FOLDER + @"\" + cvJobInfor.CV_TEMP_FILE;

                    if (System.IO.File.Exists(oldCVFilePath))
                        System.IO.File.Delete(oldCVFilePath);


                    // xoa temp cv file trong db
                    RMSCVExternalJobBusiness.Instance.UpdateCVTempFileNameForExternalJob(TPID, "", curAcc?.F_MAIL);


                }

                if (RMSCVExternalJobBusiness.Instance.UpdateCVTempFileNameForExternalJob(TPID, randomFileName, curAcc.F_MAIL) == false)
                {

                    totalResponse = new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("Upload file fail")
                    };
                    goto checkErrorPoint;

                }

                var fileInforMovedServerFile = RMSCVExternalJobBusiness.Instance.GetFileAttachInforOfExternalJobMovedToServerFile(TPID);
                if (fileInforMovedServerFile != null)
                {
                    //  goi api xoa file tren server file
                    string GroupID = fileInforMovedServerFile.FILEID;
                    var lsFileSvFile = RMSFileBusiness.Instance.GetFileInfo(GroupID);
                    if (lsFileSvFile?.Count > 0)
                    {
                        string fileId = lsFileSvFile.FirstOrDefault().fileId;
                        if (RMSFileBusiness.Instance.DeleteFileByFileID(fileId))
                        {
                            RMSCVExternalJobBusiness.Instance.DeleteFileAttachInforOfExternalJobMovedToServerFile(TPID, GroupID);
                        }
                    }

                }

                return new CusResponse1<object>
                {
                    status = StatusType.success.ToString(),
                    message = StatusType.success.ToString(),
                    data = TPID,
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
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510221657] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510221657]"   // ex.Message
                };

            }


        }




        /// <summary>
        /// Upload file CV pdf for External job , format payload: var form= new FormData();  form.append("TPID","Your doc No") ;form.append("file",file[0])
        /// </summary>
        /// <returns></returns>

      //   [HttpPost]
      //  [NonAction]
        //public CusResponse1<object> UploadFileCVForExternalJob()
        //{

        //    try
        //    {
        //        //var rsBlock=  HandleBlockUploadMutilFiles();
        //        //if (rsBlock.status == StatusType.error.ToString()) 
        //        //return rsBlock;

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
        //        var cvJobInfor = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPool(TPID);
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


        //        if (cvJobInfor.EDITSTATUS == "-1" && cvJobInfor.ISDELETE == "0")
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

        //        string randomFileName = "EJ_" + RMSBaseBusiness.Instance.GetNewRowID() + ".pdf";  // EJ_ : External job : key khoa de biet file nay thuoc external job
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
        //            RMSCVExternalJobBusiness.Instance.UpdateCVTempFileNameForExternalJob(TPID, "", curAcc?.F_MAIL);

        //        }

        //        if (RMSCVExternalJobBusiness.Instance.UpdateCVTempFileNameForExternalJob(TPID, randomFileName, curAcc.F_MAIL) == false)
        //        {
        //            if (System.IO.File.Exists(fullPath))
        //                System.IO.File.Delete(fullPath);
        //            return new CusResponse1<object>
        //            {
        //                status = StatusType.error.ToString(),
        //                message = LangHelper.Instance.Get("Upload file fail")
        //            };

        //        }

        //        var fileInforMovedServerFile = RMSCVExternalJobBusiness.Instance.GetFileAttachInforOfExternalJobMovedToServerFile(TPID);
        //        if (fileInforMovedServerFile != null)
        //        {
        //            //  goi api xoa file tren server file
        //            string GroupID = fileInforMovedServerFile.FILEID;
        //            var lsFileSvFile = RMSFileBusiness.Instance.GetFileInfo(GroupID);
        //            if (lsFileSvFile?.Count > 0)
        //            {
        //                string fileId = lsFileSvFile.FirstOrDefault().fileId;
        //                if (RMSFileBusiness.Instance.DeleteFileByFileID(fileId))
        //                {
        //                    RMSCVExternalJobBusiness.Instance.DeleteFileAttachInforOfExternalJobMovedToServerFile(TPID, GroupID);
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

        //        LoggingLocal.SaveLog(LogType.Error, "[ER-202510221657] " + ex.Message + ex.StackTrace);
        //        return new CusResponse1<object>
        //        {
        //            status = StatusType.error.ToString(),
        //            message = StatusType.error.ToString() + "[ER-202510221657]"   // ex.Message
        //        };

        //    }


        //}



        /// <summary>
        /// Delete file Cv for External Job
        /// </summary>
        /// <param name="TPID">Doc no</param>
        /// <returns></returns>

        [HttpGet]
        public CusResponse1<object> DeleteFileCVForExternalJob(string TPID)
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
                var cvInfor = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPool(TPID);
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

                if (cvInfor.EDITSTATUS == "-1" && cvInfor.ISDELETE == "0")
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
                    cvInfor.CV_TEMP_FILE  = Regex.Replace(cvInfor.CV_TEMP_FILE.Replace(".pdf", ""), "[^a-zA-Z0-9-_]", "") + ".pdf";
                    cvInfor.CV_TEMP_FILE = Path.Combine("", cvInfor.CV_TEMP_FILE);
                    //

                    string oldCVFilePath = Constant.TEMP_CV_FOLDER + @"\" + cvInfor.CV_TEMP_FILE;
                    if (System.IO.File.Exists(oldCVFilePath))
                        System.IO.File.Delete(oldCVFilePath);

                    // xoa temp cv file trong db
                    RMSCVExternalJobBusiness.Instance.UpdateCVTempFileNameForExternalJob(TPID, "", curAcc?.F_MAIL);

                }

                var fileInforMovedServerFile = RMSCVExternalJobBusiness.Instance.GetFileAttachInforOfExternalJobMovedToServerFile(TPID);
                if (fileInforMovedServerFile != null)
                {
                    //  goi api xoa file tren server file
                    string GroupID = fileInforMovedServerFile.FILEID;
                    var lsFileSvFile = RMSFileBusiness.Instance.GetFileInfo(GroupID);
                    if (lsFileSvFile?.Count > 0)
                    {
                        string fileId = lsFileSvFile.FirstOrDefault().fileId;
                        if (RMSFileBusiness.Instance.DeleteFileByFileID(fileId))
                        {
                            RMSCVExternalJobBusiness.Instance.DeleteFileAttachInforOfExternalJobMovedToServerFile(TPID, GroupID);
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
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510240812] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202510240812]"   // ex.Message
                };
            }
        }


        /// <summary>
        /// Submit External job of TPID (move status from Draft to Submit)
        /// </summary>
        /// <param name="TPID"></param>
        /// <returns></returns>
        [HttpGet]
        public CusResponse1<object> SubmitExternalJob(string TPID)
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
                var tbDat = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPool(TPID);
                if (tbDat == null)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("No data")
                    };

                if (tbDat.JOB_MAIL != curAcc?.F_MAIL)
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("This CV job not belongs you")
                    };

                if (tbDat.EDITSTATUS == "-1" && tbDat.ISDELETE == "0")
                {
                    // dang o trang thai nhap-> cho phep submit
                    var rs = RMSCVExternalJobBusiness.Instance.UpdateStatusOfCVExternalJobToSubmit(TPID, curAcc?.F_MAIL);

                    return new CusResponse1<object>
                    {
                        status = rs ? StatusType.success.ToString() : StatusType.error.ToString(),
                        message = rs ? LangHelper.Instance.Get("Submit success") : LangHelper.Instance.Get("Submit fail")
                    };
                }
                else
                {
                    return new CusResponse1<object>
                    {
                        status = StatusType.error.ToString(),
                        message = LangHelper.Instance.Get("This CV job in signing process or closed")
                    };
                }

            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202511011511] " + ex.Message + ex.StackTrace);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = StatusType.error.ToString() + "[ER-202511011511]"   // ex.Message
                };

            }

        }



        #endregion Manage CV for External job

    }
}
