using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        if (model.Password != model.PasswordConfirm)
            return BadRequest();
        var response = await _manager.Register(model);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Login(LoginRequestModel model)
    {
        var response = _manager.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(response);
    }
    
}