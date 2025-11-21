using Newtonsoft.Json.Linq;
//using RestSharp;
using RMSExternalApi.Commons;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RMSExternalApi.Businesses
{
    public class RMSFileBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSFileBusiness() { }
        private static RMSFileBusiness _instance;
        public static RMSFileBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSFileBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        public static string GetRMSAPIURL()
        {
            if (ConfigurationManager.AppSettings["IS_REAL"].ToString() == "Y")
                return Constant.EXTERNAL_DOMAIN_REAL;
            else
                return Constant.EXTERNAL_DOMAIN_TEST;
        }


        /// <summary>
        /// Delete file on server file 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public bool DeleteFileByFileID(string fileId)
        {
          
            // Gộp Base URL và Endpoint lại, sau đó thêm Query String
            string baseUrl = $"{GetRMSAPIURL()}/forward/rmsfile/api/";
            string endpoint = "delFileByFileId";

            // Xây dựng chuỗi Query Parameter
            // Sử dụng System.Web.HttpUtility.UrlEncode (nếu có) hoặc cách thủ công nếu không.
            string emp = "system";
            string encodedFileId = Uri.EscapeDataString(fileId?.Trim() ?? string.Empty);

            string requestUri = $"{baseUrl}{endpoint}?emp={emp}&fileId={encodedFileId}";

            try
            {
                // 1. Khởi tạo HttpClient
                // HttpClient được thiết kế để tái sử dụng, nhưng ta vẫn có thể khởi tạo nó trong hàm.
                using (var client = new HttpClient())
                {
                    Util.ByPassHttps();
                    // 2. Thiết lập Timeout
                    client.Timeout = TimeSpan.FromMilliseconds(10000);
                   var taskWait = client.DeleteAsync(requestUri);
                    Task.WaitAll(taskWait);
                    // Chờ kết quả (biến Async thành Sync)
                    HttpResponseMessage response = taskWait.Result;

                    // 4. Xử lý Response
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc nội dung Response (cũng là Async, cần .Result)
                        string responseContent = response.Content.ReadAsStringAsync().Result;

                        var rsObj = JObject.Parse(responseContent);

                        // Kiểm tra mã lỗi JSON
                        if (rsObj["code"]?.ToString() == "0" && rsObj["msg"]?.ToString() == "成功删除了一个文件")
                        {
                            return true;
                        }
                    }
                    else
                    {
                        LoggingLocal.SaveLog(LogType.Error, $"HTTP Status: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingLocal.SaveLog(LogType.Error, ex.Message + ex.StackTrace);
            }

            return false;
        }


       
        /// <summary>
        /// Get infor of file on Server file
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<FileOnServerFile> GetFileInfo(string groupId)
        {
            // Tạo danh sách rỗng để trả về trong trường hợp thất bại
            var emptyList = new List<FileOnServerFile>();

            // 1. Xây dựng URL đầy đủ
            string baseUrl = $"{GetRMSAPIURL()}/forward/rmsfile/api/";
            string endpoint = "getFileInfo";

            // Đảm bảo groupId được mã hóa URL (URL Encoding)
            string encodedGroupId = Uri.EscapeDataString(groupId?.Trim() ?? string.Empty);

            // URL cuối cùng bao gồm endpoint và tham số truy vấn
            string requestUri = $"{baseUrl}{endpoint}?groupId={encodedGroupId}";

            try
            {
               
                using (var client = new HttpClient())
                {
                    Util.ByPassHttps();
                    // 3. Thiết lập Timeout
                    client.Timeout = TimeSpan.FromMilliseconds(10000);

                    // 4. Thực thi Request GET (Đồng bộ)
                    // Sử dụng .Result để chặn luồng xử lý và biến Async thành Sync
                    Task<HttpResponseMessage> task = client.GetAsync(requestUri);
                    HttpResponseMessage response = task.Result;

                    // 5. Xử lý Response
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc nội dung Response (Async, cần .Result)
                        string responseContent = response.Content.ReadAsStringAsync().Result;

                        // Bắt đầu phân tích JSON như code gốc của bạn
                        var rsObj = JObject.Parse(responseContent);

                        if (rsObj["code"]?.ToString() == "0")
                        {
                            // Kiểm tra và phân tích phần "data"
                            var dataToken = rsObj["data"];
                            if (dataToken.Type != JTokenType.Null)
                            {
                                var datObj = JObject.Parse(dataToken.ToString());
                                var fileListToken = datObj["fileList"];

                                if (fileListToken.Type == JTokenType.Array)
                                {
                                    var fileLsArr = (JArray)fileListToken;
                                    if (fileLsArr.Count > 0)
                                    {
                                        return fileLsArr.Select(r =>
                                        new FileOnServerFile
                                        {
                                            fileId = r["fileId"]?.ToString(),
                                            filePath = r["filePath"]?.ToString(),
                                            fileDesc = r["fileDesc"]?.ToString(),
                                            fileName = r["fileName"]?.ToString(),
                                        }).ToList();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Ghi log lỗi HTTP
                        LoggingLocal.SaveLog(LogType.Error, $"HTTP Status: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý và ghi log các lỗi khác (ví dụ: lỗi phân tích JSON)
                LoggingLocal.SaveLog(LogType.Error, ex.Message + ex.StackTrace);
            }

            return emptyList;
        }


        /// <summary>
        /// Update status sended file to server file success
        /// </summary>
        /// <param name="fileGroup"></param>
        /// <returns></returns>
        public bool UpdFileStatus(string fileGroup)
        {
            // 1. Xây dựng URL đầy đủ (Base URL + Endpoint + Query String)
            string baseUrl = $"{GetRMSAPIURL()}/forward/rmsfile/api/";
            string endpoint = "updFileStatus";

            // Đảm bảo tham số được mã hóa URL (URL Encoding)
            string encodedFileGroup = Uri.EscapeDataString(fileGroup?.Trim() ?? string.Empty);

            // URL cuối cùng
            string requestUri = $"{baseUrl}{endpoint}?fileGroup={encodedFileGroup}";

            try
            {
               

                using (var client = new HttpClient())
                {
                    // Tùy chọn: Xử lý SSL/HTTPS bỏ qua (giữ lại code gốc)
                    Util.ByPassHttps();
                    // 2. Thiết lập Timeout
                    client.Timeout = TimeSpan.FromMilliseconds(10000);

                    // Tạo HttpContent rỗng
                    var content = new StringContent(string.Empty);

                    // Chặn luồng xử lý và chờ kết quả
                    Task<HttpResponseMessage> task = client.PutAsync(requestUri, content);
                    HttpResponseMessage response = task.Result;

                    // 4. Xử lý Response
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc nội dung Response (Async, cần .Result)
                        string responseContent = response.Content.ReadAsStringAsync().Result;

                        var rsObj = JObject.Parse(responseContent);

                        // Kiểm tra mã lỗi JSON
                        if (rsObj["code"]?.ToString() == "0" && rsObj["msg"]?.ToString() == "更新状态成功")
                        {
                            return true;
                        }
                    }
                    else
                    {
                        // Ghi log lỗi HTTP Status Code
                        LoggingLocal.SaveLog(LogType.Error, $"HTTP Status: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý và ghi log các lỗi khác (ví dụ: lỗi phân tích JSON)
                LoggingLocal.SaveLog(LogType.Error, ex.Message + ex.StackTrace);
            }

            return false;
        }
    }

    #region Note

    //0.UP FILE(ko can dang nhap)

    //POST: http://10.220.7.63:811/forward/rmsfile/api/uploadFiles

    //param FormData: 
    // files :(binary)
    //emp:V1030398
    //fileDesc :"",
    // -> return Group ID :
    //1.XOA FILE(ko can dang nhap)
    //DELETE : http://10.220.7.63:811/forward/rmsfile/api/delFileByFileId?emp=V1030398&fileId=2025102408141503837377

    //fileId= // tu muc 3 , tra ve ket qua se chua FileID



    //2. CAP NHAT TRANG THAI DA UP FILE THANH CONG: (ko can dang nhap)
    //PUT: http://10.220.7.63:811/forward/rmsfile/api/updFileStatus?fileGroup=2025070109135003629515

    // param: fileGroup = GroupID
    //3. get thong tin file : (ko can dang nhap)

    //GET :http://10.220.7.63:811/forward/rmsfile/api/getFileInfo?groupId=2025070109135003629515
    // param: groupId = GroupID

    //4. cap nhat file id da up thanh cong sang server file vao  talent_pool hien tai:

    //POST http://10.220.7.63:811/forward/rms/api/Talentpool/Talent_File_Upload?TPID=YRC2025101100001&FileId=2025102408171803837384

    //FileId=: Group ID



    #endregion Note
}