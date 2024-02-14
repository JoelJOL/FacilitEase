using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _authority;
    private readonly string _audience;

    public TokenValidationMiddleware(RequestDelegate next, string authority, string audience)
    {
        _next = next;
        _authority = authority;
        _audience = audience;
    }
    /// <summary>
    /// Authenticate the http requests
    /// Azure id token validation
    /// </summary>
    /// <param name="context">The http request</param>
    /// <returns>The middleware forwards request to the routing</returns>
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401; // Unauthorized
            return;
        }

        try
        {
            //Decodin the azure token id
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            //Get the email id of the user from the id token
            var usernameClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "preferred_username");

            if (usernameClaim != null)
            {
                var username = usernameClaim.Value;
            }

            //Parameters to validate the id token
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = _authority,
                ValidAudience = _audience,
                IssuerSigningKeys = await GetSigningKeys(),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromMinutes(5) // Adjust as needed
            };

            ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out _);
            context.User = principal;

            await _next(context);
        }
        catch (SecurityTokenValidationException)
        {
            context.Response.StatusCode = 401; // Unauthorized
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500; // Internal Server Error
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
/// <summary>
/// Build the middleware during the initial execution of program.cs
/// </summary>
public static class TokenValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenValidationMiddleware(this IApplicationBuilder builder, string authority, string audience)
    {
        return builder.UseMiddleware<TokenValidationMiddleware>(authority, audience);
    }
}