using Api.Attributes;
using Dal.Entities;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("")]
public class CabinetController : Controller
{
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
}