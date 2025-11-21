using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class MailDataView
    {
        /// <summary>
        /// Key khóa mail để xóa mail cũ: giống mail gửi OTP của BHXH
        /// </summary>
        public string KeyRowIDMail { set; get; }

        public string SystemName { set; get; }
        public string PassWord { set; get; }
        public string UserName { set; get; }

        private string _MAIL_FROM { set; get; }

        public string MAIL_FROM
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _MAIL_FROM = "";
                else _MAIL_FROM = value;
            }
            get
            {
                return _MAIL_FROM;
            }

        }
        private string _MAIL_TO { set; get; }
        public string MAIL_TO
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _MAIL_TO = "";
                else _MAIL_TO = value;
            }
            get
            {
                return _MAIL_TO;
            }

        }

        private string _MAIL_CC { set; get; }
        public string MAIL_CC
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _MAIL_CC = "";
                else _MAIL_CC = value;
            }
            get
            {
                return _MAIL_CC;
            }

        }


        private string _MAIL_BCC { set; get; }

        public string MAIL_BCC
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _MAIL_BCC = "";
                else _MAIL_BCC = value;
            }
            get
            {
                return _MAIL_BCC;
            }

        }

        private string _IMPORTANT_LEVEL;
        public string IMPORTANT_LEVEL
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _IMPORTANT_LEVEL = "";
                else _IMPORTANT_LEVEL = value;
            }
            get
            {
                return _IMPORTANT_LEVEL;
            }

        }


        private string _IS_HTML { set; get; }

        public string IS_HTML
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _IS_HTML = "";
                else _IS_HTML = value;
            }
            get
            {
                return _IS_HTML;
            }

        }

        private string _MAIL_SUBJECT { set; get; }
        public string MAIL_SUBJECT
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _MAIL_SUBJECT = "";
                else _MAIL_SUBJECT = value;
            }
            get
            {
                return _MAIL_SUBJECT;
            }

        }

        private string _MAIL_BODY { set; get; }

        public string MAIL_BODY
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _MAIL_BODY = "";
                else _MAIL_BODY = value;
            }
            get
            {
                return _MAIL_BODY;
            }

        }

        public string APPOINT_TIME { set; get; }

    }

}