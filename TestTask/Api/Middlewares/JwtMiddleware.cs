using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Logic.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private List<string?> _blackList;
    
    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
        _blackList = new List<string?>();
    }
    
    /// <summary>
    /// Check token in headers
    /// Or cookies
    /// </summary>
    /// <param name="context"></param>
    /// <param name="accountManager">manager for user</param>
    public async Task Invoke(HttpContext context, IAccountManager accountManager)
    {

        // if we use api we need check headers for authorize
        var token1 = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");
        var token = token1?.Last();
        
        // if we use view we need check cookies for authorize
        if (token == null)
            context.Request.Cookies.TryGetValue("access_token", out token);

        // if (context.Request.Cookies.ContainsKey("logout"))
        //     _blackList.Add(token);
        //
        // if (_blackList.Contains(token))
        // {
        //     context.Response.Cookies.Delete("access_token");
        //     context.Response.Cookies.Delete("logout");
        //     token = null;
        // }
        token = CheckLogout(context, token);

        if (token != null)
            AttachUserToContext(context, accountManager, token);
        await _next(context);
    }
    
    /// <summary>
    /// Check token validation
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="accountManager">manager for user</param>
    /// <param name="token">jwt access token</param>
    public void AttachUserToContext(HttpContext context, IAccountManager accountManager, string token)
    {
        try
        {
            var jwtToken = GetJwtSecurityToken(token);
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

            var user = accountManager.GetById(userId);
            // user.Password = "***";
            context.Items["User"] = user;
        }
        catch
        {
            // todo: need to add logger
        }
    }

    /// <summary>
    /// Add token in black list if user logout and disallow user use route where where authorization is required
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="token">access token</param>
    /// <returns>null or unchanged token</returns>
    private string? CheckLogout(HttpContext context, string? token)
    {
        if (context.Request.Cookies.ContainsKey("logout"))
            _blackList.Add(token);

        if (_blackList.Contains(token))
        {
            context.Response.Cookies.Delete("access_token");
            context.Response.Cookies.Delete("logout");
            DeleteInvalidToken(token);
            return null;
        }
        
        return token;
    }

    /// <summary>
    /// Delete invalid tokens in blacklist
    /// </summary>
    private void DeleteInvalidToken(string token)
    {
        var jwtToken = GetJwtSecurityToken(token);
        if (DateTime.Now > jwtToken.ValidTo)
            _blackList.Remove(token);
    }

    /// <summary>
    /// Get JwtSecurity Token for get info without token 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private JwtSecurityToken GetJwtSecurityToken(string token)
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
        return (JwtSecurityToken)validatedToken;
    }
}