using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS.Requests
{
    public class LoginWithEmail
    {
        /// <summary>
        /// Email for login with email  (Required)
        /// </summary>
        public string email { set; get; }
    }



    public class LoginWithPhone
    {
        /// <summary>
        /// phone for login with phone  (Required)
        /// </summary>
        public string phone { set; get; }
    }


    public class ConfirmOTPForEmail {
        /// <summary>
        /// Email for login with email (Required)
        /// </summary>
        public string email { set; get; }

        /// <summary>
        /// OTP for login with email (Required)
        /// </summary>
        public string OTP { set; get; }



    }


    public class ConfirmOTPForPhone 
    {
        /// <summary>
        /// Phone for login with Phone (Required)
        /// </summary>
        public string phone { set; get; }

        /// <summary>
        /// OTP for login with Phone (Required)
        /// </summary>
        public string OTP { set; get; }



    }

}