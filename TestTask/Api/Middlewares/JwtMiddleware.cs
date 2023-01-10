using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Logic.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    
    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }
    
    /// <summary>
    /// Check token in headers
    /// </summary>
    /// <param name="context">manager for user</param>
    /// <param name="accountManager">jwt access token</param>
    public async Task Invoke(HttpContext context, IAccountManager accountManager)
    {
        var token1 = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");
        var token = token1?.Last();

        if (token != null)
            AttachUserToContext(context, accountManager, token);

        await _next(context);
    }
    
    /// <summary>
    /// Check token validation
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="userManager">manager for user</param>
    /// <param name="token">jwt access token</param>
    public void AttachUserToContext(HttpContext context, IAccountManager accountManager, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

            context.Items["User"] = accountManager.GetById(userId);
        }
        catch
        {
            // todo: need to add logger
        }
    }
}