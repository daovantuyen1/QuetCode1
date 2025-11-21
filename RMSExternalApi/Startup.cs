using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Web.Services.Description;
using RMSExternalApi.Commons;

[assembly: OwinStartup(typeof(RMSExternalApi.Startup))]

namespace RMSExternalApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseJwtBearerAuthentication(
                   new JwtBearerAuthenticationOptions
                   {
                       AuthenticationMode = AuthenticationMode.Active,
                       TokenValidationParameters = new TokenValidationParameters()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = "*", //your domain need use this api //some string, normally web url,  
                           ValidAudience = "*", //your domain need use this api 
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constant.SECRECT_KEY_JWT))
                       }
                   });
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
