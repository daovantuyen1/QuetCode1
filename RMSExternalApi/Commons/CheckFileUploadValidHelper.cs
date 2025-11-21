using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Commons
{
    public class CheckFileUploadValidHelper
    {
        public static bool IsValidPdfFile(string filePath)
        {
            try
            {
                // Check MIME type
                string mimeType = MimeMapping.GetMimeMapping(filePath);
                if (mimeType != "application/pdf")
                    return false;

                // Check file extension
                if (new FileInfo(filePath).Extension?.ToLower() != ".pdf")
                    return false;

                // Check PDF magic number
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (fileStream.CanSeek)
                        fileStream.Position = 0;

                    byte[] buffer = new byte[5];
                    int bytesRead = fileStream.Read(buffer, 0, 5);
                    fileStream.Position = 0;

                    if (bytesRead < 5)
                        return false;

                    // Check for %PDF- header
                    return buffer[0] == 0x25 && // %
                           buffer[1] == 0x50 && // P
                           buffer[2] == 0x44 && // D
                           buffer[3] == 0x46 && // F
                           buffer[4] == 0x2D;   // -
                }
            }
            catch
            {
                return false;
            }
        }

    }
}