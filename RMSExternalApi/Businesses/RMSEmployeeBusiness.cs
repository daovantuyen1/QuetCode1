using Dapper;
using RMSExternalApi.Commons.DB;
using RMSExternalApi.DTO.RMS;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Businesses
{
    public class RMSEmployeeBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSEmployeeBusiness() { }
        private static RMSEmployeeBusiness _instance;
        public static RMSEmployeeBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSEmployeeBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion




        /// <summary>
        /// Add new emp register to interview worker
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="jobMail"></param>
        /// <param name="jobPhone"></param>
        public bool AddEmployeeRegisterInterview(EmployeeRegister employee, string jobMail, string jobPhone)
        {

            //   chua co dlieu theo jobMail->insert
            //  da ton tai dlieu theo jobMail:
            //-> da ton tai Factory-> update
            //->chua ton tai Factory->insert


            int rs = DBHelper.getRMSDBConnectObj()
                .Execute(@"
                INSERT INTO IE_R_EMPLOYEE (JOB_MAIL, JOB_MOBILE, F_NAME, F_MOBILE, F_FACTORY, F_INTERVIEW_DATE, F_CREATE_DATE, F_UPDATE_DATE)
                VALUES (:JOB_MAIL, :JOB_MOBILE, :F_NAME, :F_MOBILE, :F_FACTORY, TO_DATE(:F_INTERVIEW_DATE,'YYYY/MM/DD') , SYSDATE, SYSDATE)
                ", new
                {
                    JOB_MAIL = jobMail?.Trim()?.ToLower() + "",
                    JOB_MOBILE = jobPhone?.Trim() + "",
                    F_NAME = employee.name?.Trim() + "",
                    F_MOBILE = employee.mobile?.Trim() + "",
                    F_FACTORY = employee.factory?.Trim() + "",
                    F_INTERVIEW_DATE = employee.interviewDate?.Replace("-", "/") + "",
                });
            return rs > 0 ? true : false;

        }

        /// <summary>
        /// update inform emp register to interview worker
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="jobMail"></param>
        public bool UpdateEmployeeRegisterInterview(EmployeeRegister employee, string jobMail)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(@"
                  UPDATE IE_R_EMPLOYEE
                  SET 
                       F_NAME = :F_NAME,
                       F_MOBILE = :F_MOBILE,
                       F_INTERVIEW_DATE = TO_DATE(:F_INTERVIEW_DATE,'YYYY/MM/DD') ,
                       F_UPDATE_DATE = SYSDATE
                  WHERE  LOWER( JOB_MAIL ) = :JOB_MAIL AND  F_FACTORY = :F_FACTORY
             ", new
            {
                F_NAME = employee.name?.Trim() + "",
                F_MOBILE = employee.mobile?.Trim() + "",
                F_INTERVIEW_DATE = employee.interviewDate?.Replace("-", "/") + "",
                JOB_MAIL = jobMail?.Trim()?.ToLower() + "",
                F_FACTORY = employee.factory?.Trim() + "",
            });
            return rs > 0 ? true : false;
        }

        public List<IE_R_EMPLOYEE> GetEmpRegisterInterviewLS(string jobMail)
        {
            return DBHelper.getRMSDBConnectObj().Query<IE_R_EMPLOYEE>(@"
                    SELECT 
                    JOB_MAIL, JOB_MOBILE, F_NAME, F_MOBILE, F_FACTORY,
                    TO_CHAR(F_INTERVIEW_DATE,'YYYY/MM/DD') AS  F_INTERVIEW_DATE, 
                    TO_CHAR(F_CREATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS  F_CREATE_DATE, 
                    TO_CHAR(F_UPDATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS F_UPDATE_DATE
                    FROM  IE_R_EMPLOYEE  WHERE LOWER(JOB_MAIL)  = :JOB_MAIL
                    ORDER BY F_UPDATE_DATE DESC
                    ", new { JOB_MAIL = jobMail?.Trim()?.ToLower() + "" }).ToList();

        }


        public List<EmployeeRegister> GetEmppRegisterInterviewLS(string jobMail)
        {
            var dat = GetEmpRegisterInterviewLS(jobMail);
            return dat?.Select(r => new EmployeeRegister
            {
                name = r.F_NAME,
                mobile = r.F_MOBILE,
                factory = r.F_FACTORY,
                interviewDate = r.F_INTERVIEW_DATE,
                createDate = r.F_CREATE_DATE,
                updateDate = r.F_UPDATE_DATE,
            }).ToList();
        }



        /// <summary>
        /// 递归查询子节点数据
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
        public List<NodeTree> getMenuNodeTree(List<NodeTree> Nodes)
        {
            List<NodeTree> list = new List<NodeTree>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                list.Add(Nodes[i]);
            }

            for (int i = 0; i < list.Count; i++)
            {
                //设置子节点数据
                string fatherNode = list[i].NODE_NAME;
                string data1 = list[i].ROW_ID;
                string sql = @"select ROW_ID,FATHER_NODE, NODE_NAME,NODE_VALUE,NODE_DESC,SORT,CREATE_EMP,CREATE_TIME,EDIT_EMP,EDIT_TIME from C_NODE_VALUE where FATHER_NODE=:fatherNode and IS_DELETE='0' AND DATA1=:data1  ORDER BY SORT ASC";
                List<NodeTree> childrenNode = DBHelper.getRMSDBConnectObj().Query<NodeTree>(sql, new { fatherNode = fatherNode + "", data1 = data1 + "" }).ToList();
                list[i].CHILDERN_NODE = getMenuNodeTree(childrenNode);
            }

            if (list.Count == 0)
            {
                return new List<NodeTree>();
            }
            return list;
        }



        /// <summary>
        /// 根據系統名稱和父節點名稱查詢子節點詳情
        /// </summary>
        /// <param name="fatherNode">系統名稱</param>
        /// <param name="sysName">父節點名稱</param>
        /// <returns></returns>
        public List<NodeTree> getNodeTreeBySysNameAndFatherNode(string fatherNode, string sysName)
        {
            string sql = $@"select ROW_ID,SYSTEM_NAME,NODE_NAME,FATHER_NODE,NODE_VALUE,SORT,NODE_DESC,CREATE_EMP,CREATE_TIME,EDIT_EMP,EDIT_TIME,DATA2 as OWNER_EMP FROM C_NODE_VALUE WHERE SYSTEM_NAME=:sysName and is_delete=0 AND FATHER_NODE=:fatherNode  ORDER BY SORT ASC,CREATE_TIME DESC";

            List<NodeTree> fatherNodes = DBHelper.getRMSDBConnectObj().Query<NodeTree>(sql, new { sysName = sysName  + "", fatherNode = fatherNode  + "" }).ToList();
            //设置子节点数据
            List<NodeTree> nodes = getMenuNodeTree(fatherNodes);
            return nodes;

        }


    }
}