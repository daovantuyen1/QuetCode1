using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Commons
{
    public class Constant
    {
        public static readonly string SQL_SERVER_DB_MEDICINE = ConfigurationManager.AppSettings["SQL_SERVER_DB_MEDICINE"].ToString();
        public static readonly string KEY_ENCODE = "$quan9fu$KuDa3632689@deptra!vkl$%&";
         public static readonly string MAIL_FROM = ConfigurationManager.AppSettings["MAIL_FROM"].ToString();
        public static readonly string LINK_SIGN = ConfigurationManager.AppSettings["LINK_SIGN"].ToString();
        
        #region RMS ZONE
        public static readonly string ORACLE_RMS_DB_TEST = Aes128Encryption.Instance.Decrypt( ConfigurationManager.AppSettings["ORACLE_RMS_DB_TEST"].ToString());
        public static readonly string ORACLE_RMS_DB_REAL = Aes128Encryption.Instance.Decrypt(ConfigurationManager.AppSettings["ORACLE_RMS_DB_REAL"].ToString());
        public static readonly string DMZ_LINK = ConfigurationManager.AppSettings["DMZ_LINK"].ToString();
        public static readonly string INTERNAL_LINK = ConfigurationManager.AppSettings["INTERNAL_LINK"].ToString();
        public static readonly string FILE_FOLDER = ConfigurationManager.AppSettings["FILE_FOLDER"].ToString();
        public static readonly string IS_REAL = ConfigurationManager.AppSettings["IS_REAL"].ToString();
        public static readonly string SECRECT_KEY_JWT = ConfigurationManager.AppSettings["SECRECT_KEY_JWT"].ToString(); 
        /// <summary>
        /// Duong dan temp folder de luu file CV pdf cua cac job cua user
        /// </summary>
        public static readonly string TEMP_CV_FOLDER = ConfigurationManager.AppSettings["TEMP_CV_FOLDER"].ToString();
        public static readonly string EXTERNAL_DOMAIN_TEST = ConfigurationManager.AppSettings["EXTERNAL_DOMAIN_TEST"].ToString();
        public static readonly string EXTERNAL_DOMAIN_REAL = ConfigurationManager.AppSettings["EXTERNAL_DOMAIN_REAL"].ToString();
        public static readonly string IS_SEND_MAIL = ConfigurationManager.AppSettings["IS_SEND_MAIL"].ToString();
        public static readonly string API_SEND_MAIL = ConfigurationManager.AppSettings["API_SEND_MAIL"].ToString();
        public static readonly string KEY_MAIL_FOR_ROW_ID = "RqpCAVI1UhqsaqyhapGqkNVjFWAtUjAkMLGBZQsJDCRgWEFgAEhBIwLCCBHXH";

        /// <summary>
        /// Thư mục tạm thời lưu file upload lên IIS (ko dùng thư mục temp mặc định của IIS- ko control dk vấn đề virus file )
        /// </summary>
        public static readonly string TEMP_FOLDER_FOR_FILE_IIS = ConfigurationManager.AppSettings["TEMP_FOLDER_FOR_FILE_IIS"].ToString();

        /// <summary>
        /// Folder luu file dinh kem vao CV template
        /// </summary>
        public static readonly string FOLDER_CV_TEMPLATE = ConfigurationManager.AppSettings["FOLDER_CV_TEMPLATE"].ToString();

        /// <summary>
        /// file attach to cv template se dk luu tam thoi trong foler nay, sau do se quet virus file do, neu Ok se chuyen no sang folder FOLDER_CV_TEMPLATE
        /// </summary>
        public static readonly string FOLDER_CV_TEMPLATE_TEMP = ConfigurationManager.AppSettings["FOLDER_CV_TEMPLATE_TEMP"].ToString();

        /// <summary>
        /// URL cua attach file cho cv template (folder chinh)
        /// </summary>
        // public static readonly string URL_CV_ATTACH_FILE = ConfigurationManager.AppSettings["URL_CV_ATTACH_FILE"].ToString();

        /// <summary>
        /// URL cua attach file cho cv template (folder tam thoi)
        /// </summary>
        // public static readonly string URL_CV_ATTACH_FILE_TEMP = ConfigurationManager.AppSettings["URL_CV_ATTACH_FILE_TEMP"].ToString();



        public static readonly string SEND_MAIL_USER = ConfigurationManager.AppSettings["SEND_MAIL_USER"].ToString();

        public static readonly string SEND_MAIL_PASS = ConfigurationManager.AppSettings["SEND_MAIL_PASS"].ToString();


        public static readonly string KEY_AES128 = ConfigurationManager.AppSettings["KEY_AES128"].ToString();

        public static readonly string IV_AES128 = ConfigurationManager.AppSettings["IV_AES128"].ToString();


    }

    public enum CallType
    {
        DMZ = 0,
        INTERNAL = 1
    }
    #endregion   RMS ZONE
}