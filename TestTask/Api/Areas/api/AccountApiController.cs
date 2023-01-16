using Api.Attributes;
using Dal.Entities;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Api.Areas.api;

[Area("api")]
[Route("api/account/[action]")]
[ApiController]
public class AccountApiController : ControllerBase
{
    private readonly IAccountManager _manager;
    private readonly ILogger<AccountApiController> _logger;

    public AccountApiController(IAccountManager manager, ILogger<AccountApiController> logger)
    {
        _manager = manager;
        _logger = logger;
        LogContext.PushProperty("Source", "AccountApiController");
    }
    
    /// <summary>
    /// Register user use RegisterRequestModel
    /// </summary>
    /// <param name="model">model with data for register (FIO, Phone, Email, Password)</param>
    /// <returns>AuthenticateResponse with Id, Email, Access token</returns>
    [HttpPost]
    public async Task<IActionResult> Register([FromBody]RegisterRequestModel model)
    {
        // if (model.Password != model.PasswordConfirm)
        // {
        //     return BadRequest();
        // }
        var user = _manager.GetByPhone(model.Phone);
        if (user != null)
        {
            _logger.LogInformation($"the user with phone {model.Phone} is already registered");
            return BadRequest(new ErrorResponseModel("400",
                $"the user with phone {model.Phone} is already registered"));
        }
        var response = await _manager.Register(model);
        return Ok(response);
    }

    /// <summary>
    /// Login user use LoginRequestModel
    /// </summary>
    /// <param name="model">model for login user (Phone, Password)</param>
    /// <returns>AuthenticateResponse with Id, Email, Access token</returns>
    [HttpPost]
    public IActionResult Login([FromBody]LoginRequestModel model)
    {
        var response = _manager.Authenticate(model);

        if (response == null)
        {
            _logger.LogInformation($"Phone {model.Phone} or password is incorrect");
            return BadRequest(new ErrorResponseModel("400", $"Phone {model.Phone} or password is incorrect"));
        }
            

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
        var id = user.Id; 
        var cabinet = _manager.GetInfo(id);
        
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
        var user = (User) HttpContext.Items["User"];
        _logger.LogInformation($"User with phone {user.Phone} logout");
        HttpContext.Request.Headers["Authorization"] = StringValues.Empty;
        return Ok();
    }
}