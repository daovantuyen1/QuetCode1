using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Commons.DB
{
    public class DBHelper
    {


        public static OracleConnection getRMSDBConnectObj()
        {
            string sqlConn = Constant.IS_REAL == "Y" ? Constant.ORACLE_RMS_DB_REAL : Constant.ORACLE_RMS_DB_TEST;

            //if (RMSConnection == null)
            //{
            //    RMSConnection = new OracleConnection(sqlConn);
            //    RMSConnection.Open();
            //}
            //if (RMSConnection.State != System.Data.ConnectionState.Open)
            //    RMSConnection.Open();
            //return RMSConnection;

            var RMSConnection = new OracleConnection(sqlConn);
            return RMSConnection;

        }

        public static string getRMSDBConnectStr()
        {
            return Constant.IS_REAL == "Y" ? Constant.ORACLE_RMS_DB_REAL : Constant.ORACLE_RMS_DB_TEST;
        }

        public static OracleConnection RMSConnection = null;
        private static object lockObj1 = new object();
        
        public static OracleConnection InstanceRMSDB
        {
            get
            {
                string sqlConn = Constant.IS_REAL == "Y" ? Constant.ORACLE_RMS_DB_REAL : Constant.ORACLE_RMS_DB_TEST;

                lock (lockObj1)
                {
                    if (RMSConnection == null)
                    {
                        RMSConnection = new OracleConnection(sqlConn);
                        RMSConnection.Open();
                    }
                    else
                    {
                        if (RMSConnection.State != System.Data.ConnectionState.Open)
                            RMSConnection.Open();
                    }
                }

                return RMSConnection;
            }
        }


    

    }
}