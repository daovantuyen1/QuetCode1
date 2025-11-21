using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class EmployeeRegister
    {
        /// <summary>
        /// your name
        /// </summary>
        public string name { set; get; }
        /// <summary>
        ///  your phone number
        /// </summary>
        public string mobile { set; get; }
        /// <summary>
        /// Factory , that you want to register to do work
        /// </summary>
        public string factory { set; get; }
        /// <summary>
        /// Interview date: yyyy/mm/dd
        /// </summary>
        public string interviewDate { set; get; }
        public string createDate { set; get; }
        public string updateDate { set; get; }
    }


    public class ConfigData
    {
        public string value { set; get; }
        public string textVn { set; get; }
        public string textCn { set; get; }
        public string textEn { set; get; }
    }
  

}