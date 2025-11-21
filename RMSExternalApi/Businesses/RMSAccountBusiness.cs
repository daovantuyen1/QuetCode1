using Dapper;
using Oracle.ManagedDataAccess.Client;
using RMSExternalApi.Commons;
using RMSExternalApi.Commons.DB;
using RMSExternalApi.Controllers;
using RMSExternalApi.DTO.RMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Businesses
{
    public class RMSAccountBusiness
    {
        #region SingelTon
        private static object lockObj = new object();
        private RMSAccountBusiness() { }
        private static RMSAccountBusiness _instance;
        public static RMSAccountBusiness Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RMSAccountBusiness();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion






        /// <summary>
        /// Save temp OTP when login  
        /// </summary>
        /// <param name="key"></param>
        /// <param name="OTP"></param>

        /// <returns></returns>
        public bool SaveOTP(string key, string OTP)
        {
            var IP = Util.GetIp();
            DBHelper.getRMSDBConnectObj().Execute(" DELETE FROM IE_R_LOGIN_OTP WHERE  F_IP = :F_IP "
                , new { F_IP = IP.Trim() });
            int rs = DBHelper.getRMSDBConnectObj().Execute("INSERT INTO IE_R_LOGIN_OTP(F_KEY,F_OTP,F_SYSDATE,F_IP) VALUES (:F_KEY,:F_OTP,SYSDATE,:F_IP)",
                   new { F_KEY = key?.Trim()?.ToLower()+"", F_OTP = OTP?.Trim()?.ToLower() + "", F_IP = IP.Trim() + "" });
            return rs > 0 ? true : false;

        }


        public IE_R_LOGIN_OTP GetOTPInfor(string key, string OTP)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<IE_R_LOGIN_OTP>(" SELECT F_KEY, F_OTP , TO_CHAR(F_SYSDATE,'YYYY/MM/DD HH24:MI:SS') AS F_SYSDATE , F_IP  FROM IE_R_LOGIN_OTP WHERE LOWER(F_KEY) = :F_KEY AND LOWER(F_OTP) = :F_OTP AND F_IP = :F_IP "
              , new { F_KEY = key?.Trim()?.ToLower() + "", F_OTP = OTP?.Trim()?.ToLower() + "", F_IP = Util.GetIp()?.Trim() + "" });

        }

        public IE_R_LOGIN_OTP GetOTPInforByKey(string key)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<IE_R_LOGIN_OTP>(" SELECT F_KEY, F_OTP , TO_CHAR(F_SYSDATE,'YYYY/MM/DD HH24:MI:SS') AS F_SYSDATE , F_IP  FROM IE_R_LOGIN_OTP WHERE LOWER(F_KEY) = :F_KEY AND F_IP = :F_IP "
              , new { F_KEY = key?.Trim()?.ToLower() + "", F_IP = Util.GetIp()?.Trim() + "" });

        }


        public IE_C_ACCOUNT GetAccountInforByMail(string mail)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<IE_C_ACCOUNT>(
                 @"
            select F_MAIL,F_NAME,F_MOBILE, TO_CHAR(F_CREATE_DATE ,'YYYY/MM/DD HH24:MI:SS') AS F_CREATE_DATE , TO_CHAR(F_UPDATE_DATE ,'YYYY/MM/DD HH24:MI:SS') AS  F_UPDATE_DATE  from IE_C_ACCOUNT 
            WHERE LOWER(F_MAIL) = :F_MAIL
            ", new { F_MAIL = mail?.Trim()?.ToLower() + "" });
        }

        public IE_C_ACCOUNT GetAccountInforByPhone(string phone)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<IE_C_ACCOUNT>(
                 @"
            select F_MAIL,F_NAME,F_MOBILE, TO_CHAR(F_CREATE_DATE ,'YYYY/MM/DD HH24:MI:SS') AS F_CREATE_DATE , TO_CHAR(F_UPDATE_DATE ,'YYYY/MM/DD HH24:MI:SS') AS  F_UPDATE_DATE  from IE_C_ACCOUNT 
            WHERE  F_MOBILE  = :F_MOBILE
            ", new { F_MOBILE = phone?.Trim() + "" });
        }



        public bool AddAccount(string mail, string name, string mobile)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(
             @" INSERT INTO IE_C_ACCOUNT (F_MAIL,F_NAME,F_MOBILE,F_CREATE_DATE ,F_UPDATE_DATE ) VALUES(:F_MAIL,:F_NAME,:F_MOBILE, SYSDATE ,SYSDATE )
                ", new { F_MAIL = mail?.Trim()?.ToLower() + "", F_NAME = name?.Trim() + "", F_MOBILE = mobile?.Trim()?.ToLower() + "" });
            return rs > 0 ? true : false;
        }


        public bool UpdateAccount(string mail, string name, string mobile)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(
             "UPDATE IE_C_ACCOUNT SET F_NAME = :F_NAME, F_MOBILE= :F_MOBILE , F_UPDATE_DATE = SYSDATE   WHERE LOWER(F_MAIL) = :F_MAIL"
             , new { F_NAME = name?.Trim() + "", F_MOBILE = mobile?.Trim()?.ToLower() + "", F_MAIL = mail?.Trim()?.ToLower() + "" });
            return rs > 0 ? true : false;
        }


        public bool UpdateNameForAccount(string name, string mail, string mobile)
        {
            if (!string.IsNullOrWhiteSpace(mail))
            {
                int rs = DBHelper.getRMSDBConnectObj().Execute(
                          "UPDATE IE_C_ACCOUNT SET F_NAME = :F_NAME , F_UPDATE_DATE = SYSDATE   WHERE LOWER(F_MAIL) = :F_MAIL"
                          , new { F_NAME = name?.Trim() + "", F_MAIL = mail?.Trim()?.ToLower() + "" });
                return rs > 0 ? true : false;
            }
            else if (!string.IsNullOrWhiteSpace(mobile))
            {
                int rs = DBHelper.getRMSDBConnectObj().Execute(
                        "UPDATE IE_C_ACCOUNT SET F_NAME = :F_NAME  , F_UPDATE_DATE = SYSDATE   WHERE F_MOBILE = :F_MOBILE"
                        , new { F_NAME = name?.Trim(), F_MOBILE = mobile?.Trim()?.ToLower() });
                return rs > 0 ? true : false;
            }

            return false;
        }


        public bool UpdateAccountWithKeyIsMobile(string mail, string mobile)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(
             "UPDATE IE_C_ACCOUNT SET F_MAIL = :F_MAIL , F_UPDATE_DATE = SYSDATE   WHERE F_MOBILE  = :F_MOBILE"
             , new { F_MOBILE = mobile?.Trim() + "", F_MAIL = mail?.Trim()?.ToLower() + "" });
            return rs > 0 ? true : false;
        }


        public bool UpdateAccountWithKeyIsEmail(string mail, string mobile)
        {
            int rs = DBHelper.getRMSDBConnectObj().Execute(
             "UPDATE IE_C_ACCOUNT SET F_MOBILE = :F_MOBILE , F_UPDATE_DATE = SYSDATE   WHERE  LOWER(F_MAIL)  = :F_MAIL"
             , new { F_MOBILE = mobile?.Trim() + "", F_MAIL = mail?.Trim()?.ToLower() + "" });
            return rs > 0 ? true : false;
        }



        public IE_C_ACCOUNT GetAccountInforFromCurJWT()
        {
            var BearerToken = HttpContext.Current.Request.Headers["Authorization"].ToString();
            BearerToken = BearerToken.Replace("Bearer", "").Trim();
            var tokenJWT = AuthenToken.DecodeToken(BearerToken);
            string mail = Aes128Encryption.Instance.Decrypt(tokenJWT.Payload.ToList()[2].Value.ToString()); //     Encode.Decrypt(tokenJWT.Payload.ToList()[2].Value.ToString(), Constant.KEY_ENCODE);
            string phone = Aes128Encryption.Instance.Decrypt(tokenJWT.Payload.ToList()[3].Value.ToString());  //  Encode.Decrypt(tokenJWT.Payload.ToList()[3].Value.ToString(), Constant.KEY_ENCODE);

            if (!string.IsNullOrWhiteSpace(mail) && mail != "bb")
                return GetAccountInforByMail(mail);
            else if (!string.IsNullOrWhiteSpace(phone) && phone != "cc")
                return GetAccountInforByPhone(phone);
            return null;
        }


        /// <summary>
        /// Save jwt token to db to use for check login/logout
        /// </summary>
        /// <param name="token"></param>
        /// <param name="acc"></param>
        public void AddRLoginSession(string token, string acc)
        {
            DBHelper.getRMSDBConnectObj().Execute
                (@"
                  DELETE FROM IE_R_LOGIN_SESSION WHERE  F_IP = :F_IP AND F_ACCOUNT = :F_ACCOUNT
                ", new
                {
                    F_IP = Util.GetIp(),
                    F_ACCOUNT = acc?.Trim()?.ToLower(),
                });

            DBHelper.getRMSDBConnectObj().Execute(
                @"
                    INSERT INTO IE_R_LOGIN_SESSION (F_ACCOUNT,F_IP,F_TOKEN,F_CREATE_DATE)
                    VALUES  (:F_ACCOUNT,:F_IP,:F_TOKEN,SYSDATE)
                    ", new
                {
                    F_ACCOUNT = acc?.Trim()?.ToLower(),
                    F_IP = Util.GetIp(),
                    F_TOKEN = token?.Trim(),
                });

        }



        public IE_R_LOGIN_SESSION GetTokenRLoginSession(string token)
        {
            return DBHelper.getRMSDBConnectObj().QueryFirstOrDefault<IE_R_LOGIN_SESSION>(@"
                SELECT 
                F_ACCOUNT ,F_IP ,F_TOKEN ,F_CREATE_DATE
                FROM IE_R_LOGIN_SESSION
                WHERE F_TOKEN = :F_TOKEN AND F_IP = :F_IP
            ", new { F_TOKEN = token + "", F_IP = Util.GetIp() + "" });
        }

        public void DeleteTokenRLoginSession(string token)
        {
            DBHelper.getRMSDBConnectObj().Execute(@"
                DELETE FROM IE_R_LOGIN_SESSION WHERE F_TOKEN = :F_TOKEN
                ",new { F_TOKEN=token?.Trim() + "" } );
        }
    }
}