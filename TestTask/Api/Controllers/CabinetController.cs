using Api.Attributes;
using Dal.Entities;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("")]
public class CabinetController : Controller
{
    private readonly IAccountManager _manager;
    private readonly ILogger<CabinetController> _logger;
    public CabinetController(IAccountManager manager, ILogger<CabinetController> logger)
    {
        _manager = manager;
        _logger = logger;
    }
    /// <summary>
    /// View cabinet page
    /// </summary>
    /// <returns>user info (FIO, Email, Phone, Last Login) and url for logout</returns>
    [Authorize]
    [HttpGet]
    [Route("cabinet")]
    public IActionResult Cabinet()
    {
        var user = (User) HttpContext.Items["User"];
        var id = user.Id; 
        var cabinet = _manager.GetInfo(id);
        return View(cabinet);
    }

    [Authorize]
    [HttpGet]
    [Route("logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("access_token"); 
        return RedirectToAction("Login", "Account");
    }
}