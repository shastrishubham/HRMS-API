using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Configuration;
using System.Text;
using System.Web.Http;

[assembly: OwinStartup(typeof(HRMS_API.Startup))]
namespace HRMS_API
{
    public class Startup
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            ConfigureJwtAuth(app); // Enable JWT validation
            //WebApiConfig.Register(config); // Route config
            //app.UseWebApi(config);
        }

        private void ConfigureJwtAuth(IAppBuilder app)
        {
            var secret = ConfigurationManager.AppSettings["JwtSecret"];
            var key = Encoding.ASCII.GetBytes(secret);

            app.UseJwtBearerAuthentication(new Microsoft.Owin.Security.Jwt.JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, // no delay for expiry
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }
            });
        }
    }
}