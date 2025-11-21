using RMSExternalApi.Businesses;
using RMSExternalApi.Commons;
using RMSExternalApi.Models;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RMSExternalApi.Controllers
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [CustomTokenValid]
    public class RMSAPIBaseController : ApiController
    {


        /// <summary>
        ///  Xu ly chi cho phep upload 1 file trong moi request , va chu dong luu file tam
        ///  thoi vao thu muc rieng ( ko dung thu muc temp cua IIS) de quan ly va control van de viruss
        /// </summary>
        /// <returns></returns>
        protected async Task<CusResponse1<object>> HandleBlockUploadMutilFilesForPdf()
        {




            // 1. Kiểm tra loại nội dung: Phải là multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
            {
                // Trả về lỗi 415 Unsupported Media Type
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = HttpStatusCode.UnsupportedMediaType.ToString()
                };

            }

            // Đường dẫn tuyệt đối để lưu tệp (ví dụ: ~/App_Data/Uploads)
            string root = Constant.TEMP_FOLDER_FOR_FILE_IIS;  ///   HttpContext.Current.Server.MapPath("~/TempFolderForFileIIS");
            // Đảm bảo thư mục tồn tại
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            #region Delete old file of current IP
            string prefixOldFile = Util.GetClientIPv4Address(Request)?.Replace(".", "_");

            var fileLSStr = System.IO.Directory.GetFiles(root);
            if (fileLSStr?.Length > 0)
            {
                var oldFileLs = fileLSStr.Where(r => r.Contains(prefixOldFile)).ToList();
                oldFileLs?.ForEach(r =>
                {
                    try
                    {

                        if (File.Exists(r))
                            File.Delete(r);
                    }
                    catch { }

                });
            }
            #endregion Delete old file of current IP

            // Sử dụng MultipartFormDataStreamProvider để tự động lưu các phần tệp
            //var provider = new MultipartFormDataStreamProvider(root);

            var provider = new IpPrefixedStreamProvider(root, Util.GetClientIPv4Address(Request)?.Replace(".", "_"));

            // 2. Đọc nội dung yêu cầu và TỰ ĐỘNG lưu các tệp vào thư mục 'root'
            // function nay bat buoc phai su dung vs cach   await ... , ko su dung .Wait()
            await Request.Content.ReadAsMultipartAsync(provider);


            // 3. ✨ KIỂM TRA SỐ LƯỢNG TỆP ✨
            if (provider.FileData.Count != 1)
            {
                // Nếu có 0 tệp hoặc > 1 tệp, ta cần dọn dẹp và từ chối yêu cầu.

                // Xóa tất cả tệp đã được lưu tạm thời
                foreach (var file in provider.FileData)
                {
                    File.Delete(file.LocalFileName);
                }

                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = LangHelper.Instance.Get( "allow upload only one file")
                };

            }

            // 4.Xử lý tệp duy nhất(Nếu kiểm tra thành công)
            var fileData = provider.FileData.First();

            // Lấy đường dẫn tệp đã được lưu tạm thời trên Server (Tên file ngẫu nhiên)
            string localFilePath = fileData.LocalFileName;

            string clientFileName = fileData.Headers.ContentDisposition.FileName.Replace("\"", "");

            string fileExtension = Path.GetExtension(clientFileName);

            if (fileExtension?.ToLower().EndsWith("pdf") == false)
            {
                File.Delete(localFilePath);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = LangHelper.Instance.Get("Only allow pdf file"),
                };
            }

            FileInfo fileInfor = new FileInfo(localFilePath);
            if (fileInfor.Length > 5242880)
            {
                File.Delete(localFilePath);
                return new CusResponse1<object>
                {
                    status = StatusType.error.ToString(),
                    message = LangHelper.Instance.Get("File size allow <=5mb")
                };
            }



            // string newFileName = Guid.NewGuid().ToString() + fileExtension;
            //  string destinationPath = Path.Combine(root, newFileName);

            // File.Move(localFilePath, destinationPath);

            return new CusResponse1<object>
            {
                status = StatusType.success.ToString(),
                message = StatusType.success.ToString(),
                data = localFilePath
            };

        }

        protected bool CheckHaveUpdatedEmailForAccount()
        {
            var acc = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
            return string.IsNullOrWhiteSpace(acc?.F_MAIL) ? false : true;
        }
    }



}
