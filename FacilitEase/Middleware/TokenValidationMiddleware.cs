using FacilitEase.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var usernameClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "preferred_username");

            if (usernameClaim != null)
            {
                var username = usernameClaim.Value;
            }

            Console.WriteLine($"Issuer from Token: {jsonToken.Issuer}");

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

    private async Task<IEnumerable<SecurityKey>> GetSigningKeys()
    {
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            $"{_authority}/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever());

        var openIdConfig = await configurationManager.GetConfigurationAsync();
        return openIdConfig.SigningKeys;
    }
}

public static class TokenValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenValidationMiddleware(this IApplicationBuilder builder, string authority, string audience)
    {
        return builder.UseMiddleware<TokenValidationMiddleware>(authority, audience);
    }
}
