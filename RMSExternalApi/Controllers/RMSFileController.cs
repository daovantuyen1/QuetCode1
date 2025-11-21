using RMSExternalApi.Businesses;
using RMSExternalApi.Commons;
using RMSExternalApi.DTO.RMS;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
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
    public class RMSFileController : RMSAPIBaseController
    {



        /// <summary>
        /// Only allow https/http : block other : file://, fpt:// vv
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private  bool IsValidScheme(string url)
        {
            Uri uri = null;
            if (Uri.TryCreate(url, UriKind.Absolute, out  uri))
            {
                // Chỉ cho phép HTTP và HTTPS
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            }
            return false;
        }

        /// <summary>
        /// Get file from an url and return that file to client 
        /// </summary>
        /// <returns></returns>
        private async Task<HttpResponseMessage> DownloadFileFromUrl(string fileUrl1, string fileName1)
        {
            // 1. Xác định URL và Tên File
            // *Lưu ý: Bạn nên truyền URL hoặc tên file mong muốn qua tham số (ví dụ: string fileUrl)*
            string fileUrl = fileUrl1; // "https://rms-vn-cns.myfiinet.com/forward/rmsfile/uploadFile/2025-10-25/20251025080317896.pdf";
            string fileName = Path.GetFileName(fileUrl) ?? fileName1;// "20251025080317896.pdf"; // Lấy tên file từ URL


            try
            {

                if (IsValidScheme(fileUrl) == false)
                {
                    var result1 = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"File Url invalid protocol")
                    };
                    return result1;
                }

                if(fileUrl.StartsWith(RMSFileBusiness.GetRMSAPIURL())==false)
                {
                    var result1 = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"File Url not in allowed list")
                    };
                    return result1;
                }


                 if (fileUrl?.ToLower().StartsWith("file://")==true
                    || fileUrl?.ToLower().StartsWith("dict://") == true
                    || fileUrl?.ToLower().StartsWith("ftp://") == true
                      || fileUrl?.ToLower().StartsWith("gopher://") == true
                    )
                {
                    var result1 = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"File Url not in allowed list")
                    };
                    return result1;

                }
                


                // 2. Tải File từ URL bằng HttpClient
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(20);
                    Util.ByPassHttps();
                    // Tải nội dung file dưới dạng byte array
                    // Dùng await để thực hiện tác vụ bất đồng bộ
                    byte[] bytes = await httpClient.GetByteArrayAsync(fileUrl);

                    // 3. Tạo Response
                    string mimeType = MimeMapping.GetMimeMapping(fileName); // Vẫn giữ cách lấy MIME type

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(bytes)
                    };

                    // 4. Thiết lập Header để trình duyệt download file
                    result.Content.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = fileName
                        };
                    result.Content.Headers.ContentType =
                        new MediaTypeHeaderValue(mimeType);

                    return result;
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Xử lý lỗi HTTP (ví dụ: 404 Not Found, Timeout)
                var result1 = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Error retrieving file from URL: {httpEx.Message}")
                };
                return result1;
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
                return result1;
            }
        }


        /// <summary>
        /// Get file from a local folder and return that file to client
        /// </summary>
        /// <returns></returns>
        private async Task<HttpResponseMessage> DownloadFileFromLocalFolder(string fullPath1, string fileName1)
        {

            try
            {
                await Task.Delay(1);
                string fileName = fileName1; // "EJ_2025102810150700469508.pdf";
                string fullPath = fullPath1; // @"C:\Temp\RMSExternalSystem\HRRecruitPortalAPIForDMZNow\RMSExternalApi\TempCVFolder\EJ_2025102810150700469508.pdf"; 

                fullPath = Path.Combine("", fullPath);
                string mimeType = MimeMapping.GetMimeMapping(fileName);


                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(bytes)
                };
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileName
                    };
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue(mimeType   /*  "application/vnd.ms-excel"*/);
                return result;
            }
            catch (Exception ex)
            {
                var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message.Contains("Could not find") ? "File not found" : ex.Message)
                };
                return result1;
            }

        }


        /// <summary>
        /// Download file (No use swagger UI for test, plz use Postman/axios..)
        /// </summary>
        /// <param name="fileID">id of file</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> DownloadFile(string fileID)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(fileID))
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("fileID is empty")
                    };


                var acc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();

                if (CheckHaveUpdatedEmailForAccount() == false)
                {
                    var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Please update email for your account")
                    };
                    return result1;
                }



                if (new[] { "EJ_", "SJ_", "CV_" }.Contains(fileID.Substring(0, 3)))
                {
                    string fullPath = "";

                    if (fileID.StartsWith("EJ_"))  // attached file cua External job duoc luu trong thu muc local
                    {
                        var tbInfor = RMSCVExternalJobBusiness.Instance.GetTbTpTalentPoolByCvTempFileId(fileID);
                        if (tbInfor == null)
                        {
                            var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent("No data job")
                            };
                            return result1;
                        }

                        if (acc.F_MAIL != tbInfor?.JOB_MAIL)
                        {
                            var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent("You dont have permission")
                            };
                            return result1;
                        }

                        // thiet lap de tranh loi bao mat : Path Traversal
                        string fileID1 = fileID;
                        fileID1  = Regex.Replace(fileID1.Replace(".pdf", ""), "[^a-zA-Z0-9-_]", "") + ".pdf";
                        fileID1 = Path.Combine("", fileID1);
                        //
                        fullPath = Constant.TEMP_CV_FOLDER + @"\" + fileID1;
                    }
                    if (fileID.StartsWith("SJ_"))   // attached file cua School job duoc luu trong thu muc local
                    {
                        var tbInfor = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPoolByCVTemFile(fileID);
                        if (tbInfor == null)
                        {
                            var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent("No data job")
                            };
                            return result1;
                        }

                        if (acc.F_MAIL != tbInfor?.JOB_MAIL)
                        {
                            var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent("You dont have permission")
                            };
                            return result1;
                        }

                        // thiet lap de tranh loi bao mat : Path Traversal
                        string fileID1 = fileID;
                        fileID1 = Regex.Replace(fileID1.Replace(".pdf", ""), "[^a-zA-Z0-9-_]", "") + ".pdf";
                        fileID1 = Path.Combine("", fileID1);
                        //

                        fullPath = Constant.TEMP_CV_FOLDER + @"\" + fileID1;
                    }
                    if (fileID.StartsWith("CV_"))   // attached file cua CV template duoc luu trong thu muc local
                    {
                        var cvTemplateDat = RMSCVTemplateBusiness.Instance.GetCVTemplateInforByFileID(fileID);


                        if (cvTemplateDat?.F_MAIL != acc.F_MAIL)
                        {
                            var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent("You dont have permission")
                            };
                            return result1;
                        }


                        // thiet lap de tranh loi bao mat : Path Traversal
                        string fileID1 = fileID;
                        fileID1 = Regex.Replace(fileID1.Replace(".pdf", ""), "[^a-zA-Z0-9-_]", "") + ".pdf";
                        fileID1 = Path.Combine("", fileID1);
                        //

                        fullPath = cvTemplateDat?.F_IS_SCAN_FILE == "Y" ? Constant.FOLDER_CV_TEMPLATE + @"\" + fileID1 :
                       Constant.FOLDER_CV_TEMPLATE_TEMP + @"\" + fileID1;
                    }

                    return await DownloadFileFromLocalFolder(fullPath, fileID);

                }
                else  // file da dk day sang server file
                {
                    var fileExternalJobInfor = RMSCVExternalJobBusiness.Instance.GetFileAttachInforOfExternalJobMovedToServerFileByFileID(fileID);
                    var fileSchoolJobInfor = RMSCVSchoolJobBusiness.Instance.GetTbCrTpTalentPoolByFileID(fileID);
                    var fileInforOnServerFileLs = new List<FileOnServerFile>();
                    string fileID1 = "";
                    if (fileExternalJobInfor != null)
                    {
                        fileID1 = fileExternalJobInfor?.FILEID;
                        var talent=   RMSCVExternalJobBusiness.Instance.GetTbTpTalentPool(fileExternalJobInfor.TPID);
                        if(talent==null)
                        {
                            var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent("No data job")
                            };
                            return result1;
                        }
                        if(talent.JOB_MAIL != acc?.F_MAIL)
                        {
                            var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent("You dont have permission")
                            };
                            return result1;
                        }

                    }
                     
                    if (fileSchoolJobInfor != null)
                    {
                        fileID1 = fileSchoolJobInfor.FILE_ID;

                        if (fileSchoolJobInfor.JOB_MAIL != acc?.F_MAIL)
                        {
                            var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent("You dont have permission")
                            };
                            return result1;
                        }

                    }

                    if(string.IsNullOrWhiteSpace(fileID1))
                    {
                        var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("No data")
                        };
                        return result1;
                    }
                       
                    fileInforOnServerFileLs = RMSFileBusiness.Instance.GetFileInfo(fileID1);

                    if (fileInforOnServerFileLs?.Count > 0)
                    {
                        var fileSv = fileInforOnServerFileLs.First();
                        return await DownloadFileFromUrl(fileSv.filePath, Util.GetRandomString() + ".pdf");

                    }

                }


            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, "[ER-202510301021] " + ex.Message + ex.StackTrace);

                var result1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("[ER-202510301021] " + ex.Message)
                };
                return result1;

            }
            return null;

        }

    }
}
