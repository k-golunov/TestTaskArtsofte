using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using Api.Attributes;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class AccountController : Controller
{
    private readonly IAccountManager _manager;
    private readonly HttpClient _httpClient;

    public AccountController(IAccountManager manager)
    {
        _manager = manager;
        _httpClient = new HttpClient();
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        // _manager.Add(model);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        var response = await _manager.Register(model);
        // HttpContext.User.AddIdentity(new ClaimsIdentity(response.AccessToken));
        // HttpContext.Request.Headers["Authorization"] = "Bearer " + response.AccessToken;
        // return RegisterSuccess(model.FIO);
        // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);
        /*var baseAddress = new Uri("https://localhost:44319/");

        using (var httpClient = new HttpClient {BaseAddress = baseAddress})
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.AccessToken);
        }*/
        
        return RedirectToAction("RegisterSuccess", "Account", model);
    }

    [HttpPost]
    public IActionResult Login(LoginRequestModel model)
    {
        var response = _manager.Authenticate(model);
        // var a = HttpContext.Request.Cookies.TryGetValue("access_token", out var login);
        HttpContext.Response.Cookies.Append("access_token", response.AccessToken);
        return RedirectToAction("Cabinet", "Account");
    }

    [HttpGet]
    public IActionResult Login()
    {
        var baseAddress = new Uri("https://localhost:7075/");
        using (var httpClient = new HttpClient {BaseAddress = baseAddress})
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer 1");
        }
        return View();
    }

    [HttpGet]
    public IActionResult RegisterSuccess(RegisterRequestModel model)
    {
        ViewData["Fio"] = model.FIO;
        return View();
    }

    [HttpGet]
    public IActionResult Test()
    {
        return Content("yes");
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult Test2()
    {
        return Content("no");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Cabinet()
    {
        return View();
    }
}