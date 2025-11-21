using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace RMSExternalApi.Commons
{

    /// <summary>
    /// Class nay dung de ghi de ten file luu vao thu muc tam IIS ( TEMP_FOLDER_FOR_FILE_IIS ) , ten file se co tien to la IP v4
    /// </summary>
    public class IpPrefixedStreamProvider : MultipartFormDataStreamProvider
    {
        private readonly string _clientIpPrefix;

        // Constructor giữ nguyên
        public IpPrefixedStreamProvider(string rootPath, string clientIp)
            : base(rootPath)
        {
            // Chuẩn hóa IP để sử dụng làm tiền tố tệp
            _clientIpPrefix = clientIp.Replace('.', '_');
        }

        // ✨ KHẮC PHỤC LỖI: Chữ ký phương thức đúng cho .NET Framework ✨
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            // 1. Lấy tên tệp gốc từ Client (Cẩn thận loại bỏ dấu nháy kép)
            // headers.ContentDisposition.FileName là tên tệp gốc (bao gồm cả phần mở rộng)
            string clientFileName = headers.ContentDisposition.FileName.Trim('"');

            // 2. Lấy phần mở rộng
            string extension = Path.GetExtension(clientFileName);

            // 3. Tạo tên tệp ngẫu nhiên (đảm bảo tính duy nhất)
            string randomFileName = Guid.NewGuid().ToString();

            // 4. Trả về tên tệp tạm thời mong muốn: IP_NgẫuNhiên.PhầnMởRộng
            // LƯU Ý: Không cần đường dẫn ở đây, chỉ cần tên tệp (Provider sẽ tự nối RootPath)
            return $"{_clientIpPrefix}_{randomFileName}{extension}";
        }
    }
}