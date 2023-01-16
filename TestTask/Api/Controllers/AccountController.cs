using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using Api.Attributes;
using Dal.Entities;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Serilog.Context;

namespace Api.Controllers;

public class AccountController : Controller
{
    private readonly IAccountManager _manager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountManager manager, ILogger<AccountController> logger)
    {
        _manager = manager;
        _logger = logger;
        LogContext.PushProperty("Source", "AccountController");
    }
    
    /// <summary>
    /// Get view for register user
    /// </summary>
    /// <returns>view with register page</returns>
    [HttpGet]
    public IActionResult Register()
    {
        // _manager.Add(model);
        return View();
    }

    /// <summary>
    /// Accepts RegisterRequestmodel for register user
    /// </summary>
    /// <param name="model">model with data for register (FIO, Phone, Email, Password)</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("one or more field is invalid");
            return View(model);
        }
        var response = await _manager.Register(model);
        if (response == null)
        {
            _logger.LogInformation("phone or email already used");
            return View(model);
        }
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

    /// <summary>
    /// Accepts LoginRequestModel for login user
    /// also add access token in cookies for authorization session
    /// </summary>
    /// <param name="model">model for login user (Phone, Password)</param>
    /// <returns>Redirect to cabinet page</returns>
    [HttpPost]
    public IActionResult Login(LoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("one or more field is invalid");
            return View(model);
        }
        var response = _manager.Authenticate(model);
        // var a = HttpContext.Request.Cookies.TryGetValue("access_token", out var login);
        HttpContext.Response.Cookies.Append("access_token", response.AccessToken);
        return RedirectToAction("Cabinet", "Cabinet");
    }

    /// <summary>
    /// Get view for login user
    /// </summary>
    /// <returns>view login page</returns>
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

    /// <summary>
    /// View page register success with fio and URL to open the login page
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult RegisterSuccess(RegisterRequestModel model)
    {
        ViewData["Fio"] = model.FIO;
        return View();
    }
}