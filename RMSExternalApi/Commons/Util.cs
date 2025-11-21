using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace RMSExternalApi.Commons
{
    public static class Util
    {


        public static string DecodeBase64(string base64EncodedData)
        {
            // Step 1: Convert Base64 string to a byte array
            byte[] base64DecodedBytes = Convert.FromBase64String(base64EncodedData);

            // Step 2: Use UTF8 encoding to convert the byte array back to a string
            // UTF8 is the standard for web/data transfer and supports all Unicode characters.
            string plainText = Encoding.UTF8.GetString(base64DecodedBytes);

            return plainText;
        }

        public static string GetClientIPv4Address(HttpRequestMessage request)
        {
            // Cần System.Web.HttpContext
            if (HttpContext.Current != null)
            {
                // Lấy IP từ HttpContext của ASP.NET
                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                // Loại bỏ IPv6 nếu có (ví dụ: ::1 cho localhost)
                if (ipAddress.Equals("::1") || ipAddress.Equals("127.0.0.1"))
                {
                    return "127.0.0.1";
                }
                return ipAddress;
            }
            // Trường hợp khác (ví dụ: Self-hosting)
            return "0.0.0.0";
        }


        public static void ByPassHttps()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public static string GetIp()
        {
            try
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return ip;
            }
            catch
            {
               
            }
            return "";
           
        }

        public static  bool CheckDateStrIsValid(this string DateStr)
        {
            try
            {
                DateTime.ParseExact(DateStr, "yyyy/MM/dd", new CultureInfo("en-US"));
                return true;
            }
            catch 
            {

                return false;
            }
           
        }

        public static string GetRandomString()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string strRandom = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
            return DateTime.Now.ToString("yyyyMMddHHmmssff") + strRandom;
        }

        public static string GetRandomStringOTP()
        {
            Random random = new Random();
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            string strRandom = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            return  strRandom;
        }


        public static bool CheckDateTimeStrIsValid(this string DateTimeStr)
        {
            try
            {
                DateTime.ParseExact(DateTimeStr, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"));
                return true;
            }
            catch
            {

                return false;
            }
        }

        public static string AppendNewLineHtml(this string input)
        {
            return "<span> "+ input + " </span> <br/>" ;

        }

        public static bool CheckDsValid(this DataSet ds)
        {
            if (ds == null) return false;
            if (ds.Tables == null) return false;
            if (ds.Tables[0].Rows == null) return false;
            if (ds.Tables[0].Rows.Count <= 0) return false;
            return true;
        }
        /// <summary>
        /// Check DataTable is Valid?
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool CheckDtValid(this DataTable dt)
        {
            if (dt == null) return false;
            if (dt.Rows == null) return false;
            if (dt.Rows.Count <= 0) return false;
            return true;
        }



        /// <summary>
        /// Kiểm tra xem một chuỗi có phải là địa chỉ email hợp lệ hay không.
        /// </summary>
        /// <param name="emailAddress">Chuỗi địa chỉ email cần kiểm tra.</param>
        /// <returns>True nếu chuỗi là địa chỉ email hợp lệ, ngược lại là False.</returns>
        public static bool IsValidEmail(string emailAddress)
        {
            // Kiểm tra xem chuỗi có rỗng hoặc null không
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return false;
            }

            try
            {
                // Sử dụng lớp MailAddress để phân tích và xác thực địa chỉ email
                // Nếu địa chỉ không hợp lệ, nó sẽ ném ra một FormatException
                MailAddress m = new MailAddress(emailAddress);

                // Nếu không có lỗi, tức là email có cú pháp hợp lệ
                return true;
            }
            catch (FormatException)
            {
                // Bắt ngoại lệ nếu chuỗi không phải là định dạng email hợp lệ
                return false;
            }
            catch (Exception ex)
            {
                // Bắt các ngoại lệ khác có thể xảy ra (ví dụ: ArgumentNullException nếu emailAddress là null,
                // nhưng chúng ta đã kiểm tra nó ở trên)
                // Trong thực tế, bạn có thể muốn log ex.Message ở đây.
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Convert  total Days to Year interger
        /// </summary>
        /// <param name="totalDays"></param>
        /// <returns></returns>
        public static int ConvertDaysToApproximateYears(int totalDays)
        {
            // 1. Define the average number of days in a year (365.25 accounts for leap years).
            const double DaysPerYear = 365.25;

            // 2. Divide the total days by the average days per year to get the approximate years (as a double).
            double approximateYears = (double)totalDays / DaysPerYear;

            // 3. Convert the double result to an integer. We use (int) to truncate (discard the fraction), 
            //    which means the result is the number of WHOLE years.
            return (int)approximateYears;

            // ALTERNATIVE: Use Math.Round for rounding to the nearest whole year:
            // return Convert.ToInt32(Math.Round(approximateYears));
        }



    }
}