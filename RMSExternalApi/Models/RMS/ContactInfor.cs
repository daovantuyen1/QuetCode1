using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class ContactInfor
    {
        public string contactName { set; get; }
        public string contactPhone { set; get; }
        public string contactMail { set; get; }
        public int sort { set; get; }
    }

    public class ContactWindow : ContactInfor
    {
        public string rowId { set; get; }
        public string languageId { set; get; }
        public string isDelete { set; get; }
        public string createdEmp { set; get; }
        public string createdDate { set; get; }
        public string updatedEmp { set; get; }
        public string updatedDate { set; get; }

    }
}