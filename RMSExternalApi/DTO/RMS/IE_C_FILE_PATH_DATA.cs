using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.DTO.RMS
{
    public class IE_C_FILE_PATH_DATA
    {
      public string   ROW_ID { set; get; }                   
      public string    FILE_GROUP_ID { set; get; }
        public string   FILE_ID { set; get; }
        public string   FILE_NAME { set; get; }
        public string   FILE_VERSION { set; get; }
        public string   FILE_EXPANDED_NAME { set; get; }
        public string   FILE_PATH { set; get; }
        public string   FILE_SIZE { set; get; }
        public string   FILE_DESC { set; get; }
        public string   SORT { set; get; }
        public string   STATUS { set; get; }
        public string   IS_DELETE { set; get; }
        public string   DATA1 { set; get; }
        public string   DATA2 { set; get; }
        public string   CREATE_EMP { set; get; }
        public string   CREATE_TIME { set; get; }
        public string   UPDATE_EMP { set; get; }
        public string   UPDATE_TIME { set; get; }


    }
}