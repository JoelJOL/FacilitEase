using DotNetEnv;
using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FacilitEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _authority = Env.GetString("Application_Authority");
        private readonly string _audience = Env.GetString("Application_Audience");
        private readonly ILoginService _loginService;
        private readonly IAzureRoleManagementService _roleManagementService;

        public UserController(ILoginService loginService, IAzureRoleManagementService roleManagementService)
        {
            _loginService = loginService;
            _roleManagementService = roleManagementService;
        }
        /// <summary>
        /// To validate the azure generated token and generate a JWT token of the applicaiton for authentication
        /// </summary>
        /// <param name="azureReturn">An ApiModel that has access token</param>
        /// <returns>Application generated JWT token</returns>
        [HttpPost]
        public IActionResult Login([FromBody] AzureReturn azureReturn)
        {
            try
            {
                var accessToken = azureReturn.AccessToken;

                //Get the id token from the header of the http request
                var token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
                string username = "";

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                //Decoding the id token
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                //Decoding the email id from the azure JWT token
                var usernameClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "preferred_username");

                if (usernameClaim != null)
                {
                    username = usernameClaim.Value.ToString();
                }

                //Setting the validate parameters to validate the id token from azure
                var parameters = new TokenValidationParameters
                {
                    ValidIssuer = _authority,
                    ValidAudience = _audience,
                    IssuerSigningKeys = GetSigningKeys().Result,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };

                //Validating the azure token. If not azure token, an exception will be thrown
                ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out _);
                HttpContext.User = principal;

                //To generate a JWT token if the user is present in the employee table
                var appToken = _loginService.CheckUser(username);

                //Get the roles from the active directory
                var roless = _roleManagementService.GetAppRoles(accessToken);

                // You can add your business logic here or return a specific ActionResult
                return Ok(appToken);
            }
            catch (SecurityTokenValidationException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return StatusCode(500); // Internal Server Error
            }
        }
        /// <summary>
        /// The contains the cryptographic keys used by the OpenID Connect provider to sign tokens. 
        /// These are essential for verifying the authenticity of tokens received by the application
        /// </summary>
        /// <returns>Keys used by OpenID Connect</returns>
        private async Task<IEnumerable<SecurityKey>> GetSigningKeys()
        {
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{_authority}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever());

            var openIdConfig = await configurationManager.GetConfigurationAsync();
            return openIdConfig.SigningKeys;
        }
    }
}