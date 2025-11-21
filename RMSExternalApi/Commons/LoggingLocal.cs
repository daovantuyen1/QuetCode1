using RMSExternalApi.Businesses;
using RMSExternalApi.Commons.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
namespace RMSExternalApi.Commons
{

    public enum LogType
    {
        Info,
        Debug,
        Fatal,
        Error,
        Warn
    }
    public class LoggingLocal
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void SaveLog(LogType logType, string mess, params object[] param)
        {
            try
            {

                string acc = "";
                string IP = Util.GetIp();
                try
                {
                    var accObj = RMSAccountBusiness.Instance.GetAccountInforFromCurJWT();
                    acc = !string.IsNullOrWhiteSpace(accObj?.F_MAIL) ? accObj?.F_MAIL : accObj?.F_MOBILE;
                }
                catch
                {
                }

                mess = $"[IP:{IP},acc:{acc}]" + mess;
                mess = param?.Count() > 0 ? string.Format(mess, param) : mess;

                SaveLogToDb(mess, acc, IP, logType.ToString());

               
                // 2. Save log to local file
                switch (logType)
                {
                    case LogType.Info: _log.Info(mess);  break;
                    case LogType.Debug: _log.Debug(mess); break;
                    case LogType.Fatal: _log.Fatal(mess); break;
                    case LogType.Error: _log.Error(mess); break;
                    case LogType.Warn: _log.Warn(mess); break;
                    default: break;
                }

            }
            catch
            {

            }
        }

        public static void SaveLogToDb(string mess,string acc,string IP,string LogType)
        {
            try
            {
                // xoa tat ca cac log cu cua 1 IP , chi giu lai 21 log gan day nhat
                DBHelper.getRMSDBConnectObj().Execute(
                @"
                DELETE FROM IE_LOG WHERE ROWID IN (
                -- GIU LAI 21 LOG CUOI CUNG CUA 1 IP , VA TRA VE CAC LOG CU TRC DO 
                SELECT  F_ROWID FROM (
                WITH TOTAL AS (  SELECT   E.* 
                              FROM (SELECT ROW_NUMBER() OVER (ORDER BY  F_CREATE_DATE ASC ) AS ROW_NUMBER, K.* 
                            FROM (SELECT ROWID AS F_ROWID, GG.* FROM IE_LOG GG WHERE F_IP = :F_IP )  K ) E
                               )
                , ROW_TB AS 
                (
                SELECT MAX(ROW_NUMBER) AS MAX_ROW_NUMBER , MAX(ROW_NUMBER) -20   AS MIN_ROW_NUMBER
                FROM TOTAL
                ) 
                SELECT A.F_ROWID 
                FROM 
                TOTAL A , ROW_TB   B WHERE 
                A.ROW_NUMBER  NOT  BETWEEN B.MIN_ROW_NUMBER AND B.MAX_ROW_NUMBER
                )
                )
                ", new { F_IP = IP } );

                DBHelper.getRMSDBConnectObj().Execute(@"
                INSERT INTO IE_LOG (F_ACCOUNT,F_IP,F_MESS,F_CREATE_DATE)
                VALUES  (:F_ACCOUNT,:F_IP,:F_MESS,SYSDATE)
                ",new {
                    F_ACCOUNT= acc,
                    F_IP=IP,
                    F_MESS=$"[{LogType}]:" + mess,
                } );
                DBHelper.getRMSDBConnectObj().Execute(" DELETE FROM IE_LOG WHERE F_CREATE_DATE <=SYSDATE-180 "); //xoa log qua 180 day
            }
            catch 
            {

            }
        }

    }
}