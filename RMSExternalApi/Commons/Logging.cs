using RMSExternalApi.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using RMSExternalApi.Businesses;
using RMSExternalApi.Commons.DB;

namespace RMSExternalApi.Commons
{

    public class Logging
    {

        #region SingelTon
        private static object lockObj = new object();
        private Logging() { }
        private static Logging _instance;
        public static Logging Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Logging();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

      

        public void SaveLogAccessFromDMZ()
        {
            string rquestId = Util.GetIp();
            if (!string.IsNullOrWhiteSpace(rquestId) && rquestId != "::1")
            {
                try
                {

                    DBHelper.getRMSDBConnectObj().Execute(@"
                                            BEGIN 
                                            INSERT INTO IE_DMZ_LOG_ACCESS (F_IP,F_CREATE_TIME,F_UPDATE_TIME,F_DAY,F_COUNT,F_MESSAGE)
                                            SELECT :F_IP ,SYSDATE,'',TO_CHAR(SYSDATE,'YYYY/MM/DD'),0,'Access web'
                                            FROM DUAL
                                            WHERE NOT EXISTS
                                            (
                                            SELECT 1 FROM IE_DMZ_LOG_ACCESS WHERE  F_IP = :F_IP AND F_DAY= TO_CHAR(SYSDATE,'YYYY/MM/DD') AND  F_JOB_ID IS NULL
                                            )
                                            ;

                                            UPDATE IE_DMZ_LOG_ACCESS SET F_COUNT = NVL(F_COUNT,1) + 1 , F_UPDATE_TIME =SYSDATE
                                            WHERE  F_IP = :F_IP AND F_DAY= TO_CHAR(SYSDATE,'YYYY/MM/DD')  AND  F_JOB_ID IS NULL
                                             ;
                                            END; 
                                            ",
                        new
                        {
                            F_IP = rquestId

                        });

                }
                catch (Exception ex)
                {

                }


            }

        }


        public void SaveLogAccessJobDetailFromDMZ(string jobId)
        {
            string rquestId = Util.GetIp();
            jobId = jobId?.Trim();
            if (!string.IsNullOrWhiteSpace(rquestId) && rquestId != "::1" && !string.IsNullOrWhiteSpace(jobId))
            {
                try
                {

                    DBHelper.getRMSDBConnectObj().Execute(@"
                                            BEGIN 
                                            INSERT INTO IE_DMZ_LOG_ACCESS (F_IP,F_CREATE_TIME,F_UPDATE_TIME,F_DAY,F_COUNT,F_JOB_ID,F_MESSAGE)
                                            SELECT :F_IP ,SYSDATE,'',TO_CHAR(SYSDATE,'YYYY/MM/DD'),0, :F_JOB_ID ,'Access job'
                                            FROM DUAL
                                            WHERE NOT EXISTS
                                            (
                                            SELECT 1 FROM IE_DMZ_LOG_ACCESS WHERE  F_IP = :F_IP AND F_DAY= TO_CHAR(SYSDATE,'YYYY/MM/DD') AND  F_JOB_ID =  :F_JOB_ID 
                                            )
                                            ;

                                            UPDATE IE_DMZ_LOG_ACCESS SET F_COUNT = NVL(F_COUNT,1) + 1 , F_UPDATE_TIME =SYSDATE
                                            WHERE  F_IP = :F_IP AND F_DAY= TO_CHAR(SYSDATE,'YYYY/MM/DD')  AND  F_JOB_ID =  :F_JOB_ID  
                                             ;
                                            END; 
                                            ",
                        new
                        {
                            F_IP = rquestId,
                            F_JOB_ID=jobId

                        });

                }
                catch (Exception ex)
                {

                }


            }

        }
    }

}