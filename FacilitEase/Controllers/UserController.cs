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

        [HttpPost]
        public IActionResult Login([FromBody] AzureReturn azureReturn)
        {
            try
            {
                var accessToken = azureReturn.AccessToken;
                var token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
                string username = "";

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                var usernameClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "preferred_username");

                if (usernameClaim != null)
                {
                    username = usernameClaim.Value.ToString();
                }

                Console.WriteLine($"Issuer from Token: {jsonToken.Issuer}");

                var parameters = new TokenValidationParameters
                {
                    ValidIssuer = _authority,
                    ValidAudience = _audience,
                    IssuerSigningKeys = GetSigningKeys().Result, // Use .Result for simplicity
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromMinutes(5) // Adjust as needed
                };

                ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out _);
                HttpContext.User = principal;

                var appToken = _loginService.CheckUser(username);
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