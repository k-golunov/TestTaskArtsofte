using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.api;

[Area("api")]
public class AccountApiController : ControllerBase
{
    private readonly IAccountManager _manager;

    public AccountApiController(IAccountManager manager)
    {
        _manager = manager;
    }
    
    [HttpPost]
    [Route("api/account/[action]")]
    public IActionResult Register(RegisterRequestModel model)
    {
        if (model.Password != model.PasswordConfirm)
            return BadRequest();
        _manager.Register(model);
        return Ok();
    }
}