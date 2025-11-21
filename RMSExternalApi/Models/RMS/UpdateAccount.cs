using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS.Requests
{
    public class UpdateAccount
    {
        /// <summary>
        /// (Required)
        /// </summary>
        public string name { set; get; }
      
    }


    public class UpdateEmailForAccount {

        /// <summary>
        /// (Required)
        /// </summary>
        public string mail { set; get; }
       
        /// <summary>
        /// (Required)
        /// </summary>

        public string OTP { set; get; }



    }

    public class UpdatePhoneForAccount {

        /// <summary>
        /// (Required)
        /// </summary>
        public string phone { set; get; }

        /// <summary>
        /// (Required)
        /// </summary>

        public string OTP { set; get; }
    }
}