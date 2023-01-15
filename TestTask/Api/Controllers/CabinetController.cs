using Api.Attributes;
using Dal.Entities;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("")]
public class CabinetController : Controller
{
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
        var cabinet = new CabinetModel
        {
            FIO = user.FIO,
            Email = user.Email,
            LastLogin = user.LastLogin,
            Phone = user.Phone
        };
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