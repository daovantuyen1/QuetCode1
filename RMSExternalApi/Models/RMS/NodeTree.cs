using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{
    public class NodeTree
    {
        /// <summary>
        /// 菜单节点的映射类
        /// </summary>
        public string ROW_ID { get; set; }
        public string FATHER_NODE { get; set; }
        public string NODE_VALUE { get; set; }
        public string NODE_DESC { get; set; }
        public string NODE_NAME { get; set; }
        public string SORT { get; set; }
        public string CREATE_EMP { get; set; }
        public DateTime CREATE_TIME { get; set; }
        public string EDIT_EMP { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public string OWNER_EMP { get; set; }
        public List<NodeTree> CHILDERN_NODE { get; set; }
    }

}