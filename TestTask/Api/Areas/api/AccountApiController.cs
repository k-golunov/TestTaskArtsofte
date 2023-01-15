using Api.Attributes;
using Dal.Entities;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Api.Areas.api;

[Area("api")]
[Route("api/account/[action]")]
public class AccountApiController : ControllerBase
{
    private readonly IAccountManager _manager;

    public AccountApiController(IAccountManager manager)
    {
        _manager = manager;
    }
    
    /// <summary>
    /// Register user use RegisterRequestModel
    /// </summary>
    /// <param name="model">model with data for register (FIO, Phone, Email, Password)</param>
    /// <returns>AuthenticateResponse with Id, Email, Access token</returns>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        if (model.Password != model.PasswordConfirm)
            return BadRequest();
        var response = await _manager.Register(model);
        return Ok(response);
    }

    /// <summary>
    /// Login user use LoginRequestModel
    /// </summary>
    /// <param name="model">model for login user (Phone, Password)</param>
    /// <returns>AuthenticateResponse with Id, Email, Access token</returns>
    [HttpPost]
    public IActionResult Login(LoginRequestModel model)
    {
        var response = _manager.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(response);
    }

    /// <summary>
    /// Get info about user
    /// </summary>
    /// <returns>user`s info (FIO, Phone, Email, Last Login)</returns>
    [Authorize]
    [HttpGet]
    // [Route("get-my-info")]
    public IActionResult GetMyInfo()
    {
        var user = (User) HttpContext.Items["User"];
        var cabinet = new CabinetModel
        {
            FIO = user.FIO,
            Email = user.Email,
            LastLogin = user.LastLogin,
            Phone = user.Phone
        };
        
        return Ok(cabinet);
    }

    /// <summary>
    /// Not work(
    /// Kills an authorized session
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Request.Headers["Authorization"] = StringValues.Empty;
        return Ok();
    }
}