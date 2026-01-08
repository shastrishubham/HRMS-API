using Google.Apis.Auth;
using HRMS_API.Models;
using Microsoft.IdentityModel.Tokens;
using ServerModel.Data;
using ServerModel.Employee;
using ServerModel.Model.Employee;
using ServerModel.Model.Login;
using ServerModel.Repository;
using ServerModel.ServerModel.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {

        EmployeeRespository employeeRespository;

        public AuthController()
        {
            DbContext.ConnectionString = WebApiConfig.ConnectionString;
            employeeRespository = new EmployeeRespository();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/auth/google-login")]
        public async System.Threading.Tasks.Task<IHttpActionResult> GoogleLogin([FromBody] TokenRequest model)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(model.Token);

                // Optionally, verify `payload.Audience == your Google clientId`
                // You may want to check/insert user into DB here

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["JwtSecret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, payload.Subject),
                    new Claim(ClaimTypes.Email, payload.Email),
                    new Claim(ClaimTypes.Name, payload.Name)
                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var jwt = tokenHandler.WriteToken(securityToken);

                var result = new TokenRequest
                {
                    Token = jwt,
                    Email = payload.Email,
                    Name = payload.Name,
                    GoogleId = payload.Subject
                };

                // get the user details
                EmployeePersonalInformation empDetail =  EmployeePersonalInformationServer.GetEmployeeDetailByEmailAddress(payload.Email);
                if(empDetail != null)
                {
                    // check subscription
                    int trialEnAfterDays = Convert.ToInt32(ConfigurationManager.AppSettings["TrialEndAfterDay"]);
                    DateTime trialEndDate = DateTime.Today.AddDays(trialEnAfterDays);

                    if(empDetail.ActiveFrom < trialEndDate)
                    {
                        // subscription continue & create entry in loginout table
                        bool id = UpsertLogInLogOut(payload.Subject, Guid.Empty);
                        if (id)
                        {
                            result.UserId = empDetail.EMP_Info_Id.HasValue ? empDetail.EMP_Info_Id.Value : Guid.Empty;
                            result.CompId = empDetail.CompId.HasValue ? empDetail.CompId.Value : Guid.Empty;
                            return Ok(result);
                        }
                        else
                            return BadRequest("Failed to upsert LogIn/LogOut record.");
                    }
                    else
                    {
                        // subscription end
                        return BadRequest("Your subscription has ended. Please renew to continue using the service.");
                    }
                }
                else
                {
                    // check subscription
                    int trialEnAfterDays = Convert.ToInt32(ConfigurationManager.AppSettings["TrialEndAfterDay"]);
                    DateTime trialEndDate = DateTime.Today.AddDays(trialEnAfterDays);

                    // get first time login date
                    LoginSetupServer loginSetupServer = new LoginSetupServer();
                    DateTime? firstLoginDate = loginSetupServer.GetFirstLogInDateById(payload.Subject);
                    if(firstLoginDate != null)
                    {
                        if(firstLoginDate < trialEndDate)
                        {
                            bool id = UpsertLogInLogOut(payload.Subject, Guid.Empty);
                            if (id)
                                return Ok(result);
                            else
                                return BadRequest("Failed to upsert LogIn/LogOut record.");
                        }
                        else
                        {
                            // subscription end
                            return BadRequest("Your free trial has ended. Please upgrade to a paid plan to continue using the service.");
                        }
                    }
                    else
                    {
                        bool id = UpsertLogInLogOut(payload.Subject, Guid.Empty);
                        if (id)
                            return Ok(result);
                        else
                            return BadRequest("Failed to upsert LogIn/LogOut record.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid token: " + ex.Message);
            }
        }


        private bool UpsertLogInLogOut(string oAuthUserId, Guid userId)
        {
            LogInOut logInOut = new LogInOut
            {
                OAuthUserId = oAuthUserId,
                UserId = userId
            };

            int id = LoginSetupServer.UpsertLogInLogout(logInOut);
            if (id > 0)
                return true;
            else
                return false;       
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/auth/GetEmployeeDetailByEmailAddress")]
        public EmployeePersonalInformation GetEmployeeDetailByEmailAddress()
        {
            string email = "shastri.ajinkya84@gmail.com";
            EmployeePersonalInformation empDetail = EmployeePersonalInformationServer.GetEmployeeDetailByEmailAddress(email);
            return empDetail;
        }
    }
}
