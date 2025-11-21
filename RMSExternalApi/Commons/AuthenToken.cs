using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RMSExternalApi.Commons;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Controllers;
using RMSExternalApi.Businesses;

namespace RMSExternalApi.Controllers
{
    public class AuthenToken
    {

        public static string GetToken(string userName,string mail,string phone)
        {
            try
            {

                string key = Constant.SECRECT_KEY_JWT; //Secret key which will be used later during validation    
                var issuer = "*";  // your domain need use this api //normally this will be your site URL    

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                //Create a List of Claims, Keep claims name short    
                var permClaims = new List<Claim>();
                permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                permClaims.Add(new Claim("aa", Aes128Encryption.Instance.Encrypt(!string.IsNullOrWhiteSpace(userName) ? userName : "aa")   /* Encode.Encrypt(!string.IsNullOrWhiteSpace(userName)? userName:"aa", Constant.KEY_ENCODE) */ ));   //userName
                permClaims.Add(new Claim("bb", Aes128Encryption.Instance.Encrypt(!string.IsNullOrWhiteSpace(mail) ? mail : "bb")   /*   Encode.Encrypt(!string.IsNullOrWhiteSpace(mail)? mail:"bb", Constant.KEY_ENCODE) */ ));   //mail
                permClaims.Add(new Claim("cc", Aes128Encryption.Instance.Encrypt(!string.IsNullOrWhiteSpace(phone) ? phone : "cc")  /* Encode.Encrypt(!string.IsNullOrWhiteSpace(phone)? phone:"cc", Constant.KEY_ENCODE) */ ));   //phone
                permClaims.Add(new Claim("dd", Aes128Encryption.Instance.Encrypt(Util.GetIp())  /*  Encode.Encrypt(Util.GetIp(), Constant.KEY_ENCODE) */)); // IP


                //Create Security Token object by giving required parameters    
                var token = new JwtSecurityToken(issuer, //Issure    
                                issuer,  //Audience    
                                permClaims,
                                expires: DateTime.Now.AddMinutes(100),
                                signingCredentials: credentials);
                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt_token; 
            }
            catch
            {
                return null;
            }

        }

        public static JwtSecurityToken DecodeToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                return jwtSecurityToken;
            }
            catch
            {
                return null;

            }

        }

        public static bool CheckTokenValid(string token)
        {
            try
            {
              
                var tokenDB =  RMSAccountBusiness.Instance.GetTokenRLoginSession(token);
                // neu token ko co trong db-> ko hop le
                if (tokenDB == null) return false;

                var tokenJWT = DecodeToken(token);
                if (tokenJWT == null) return false;

                // string UserId = Encode.Decrypt(tokenJWT.Payload.ToList()[1].Value.ToString(), Constant.KEY_ENCODE);
                string IP = Aes128Encryption.Instance.Decrypt(tokenJWT.Payload.ToList()[4].Value.ToString());//  Encode.Decrypt(tokenJWT.Payload.ToList()[4].Value.ToString(), Constant.KEY_ENCODE);
                if (IP == Util.GetIp())
                {
                    return true;
                }
            }
            catch
            {


            }
            return false;
        }
    }

    /// <summary>
    /// Check Token Send from request is vaild.
    /// </summary>
    public class CustomTokenValidAttribute : System.Web.Http.AuthorizeAttribute
    {

        protected bool OnAuthorization(HttpContextBase httpContext)
        {
            var BearerToken = HttpContext.Current.Request.Headers["Authorization"]?.ToString();
            BearerToken = BearerToken.Replace("Bearer", "").Trim();
            if (AuthenToken.CheckTokenValid(BearerToken) == false)
                return false;
            return true;

        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
          
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("authorization has been denied for this request")
            };


        }
    }

}