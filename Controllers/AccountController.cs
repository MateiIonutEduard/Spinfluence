using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Spinfluence.Models;
using Spinfluence.Services;
using Spinfluence.Data;
#pragma warning disable

namespace Spinfluence.Controllers
{
    public class AccountController : Controller
    {
        readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        { this.accountService = accountService; }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Recover()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string address, string password)
        {
            string token = await accountService.Login(address, password);
            if (string.IsNullOrEmpty(token)) return Unauthorized(new { error = "Unauthorized access!" });
            Response.Cookies.Append("token", token);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(string username, string password, string address, IFormFile logo, [FromForm]int grantType)
        {
            string token = await accountService.Signup(username, password, address, logo, grantType);
            if (string.IsNullOrEmpty(token)) return Unauthorized(new { error = "User already exists!" });
            Response.Cookies.Append("token", token);
            return Ok();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> RefreshToken(string token)
        {
            string newToken = await accountService.RefreshToken(token);
            return Ok(new { token = newToken });
        }

        [Authorize]
        public IActionResult About(string token)
        {
            var data = accountService.About(token);
            return Json(data);
        }

        public async Task<IActionResult> Profile(int userId)
        {
            Account? account = await accountService.GetAccountProfileAsync(userId);
            int index = account.logo.LastIndexOf('.');

            string ext = account.logo.Substring(index + 1);
            byte[] buffer = System.IO.File.ReadAllBytes(account.logo);
            return File(buffer, $"image/{ext}");
        }

        public IActionResult Show()
        {
            var token = Request.Cookies["token"];
            var data = accountService.About(token);
            int index = data.path.LastIndexOf('.');

            string ext = data.path.Substring(index + 1);
            byte[] buffer = System.IO.File.ReadAllBytes(data.path);
            return File(buffer, $"image/{ext}");
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdatePassword(string key)
        {
            var header = HttpContext.Request.Headers["Authorization"].ToString();
            var token = header.Split(' ')[1];

            var done = await accountService.UpdatePassword(token, key);
            if (!done) return Unauthorized();

            return Ok();
        }

        public IActionResult Signout()
        {
            Response.Cookies.Delete("token");
            return Redirect("/Account/Login");
        }

        [HttpPost]
        public IActionResult Recover(string address)
        {
            bool ok = accountService.Recover(address);
            if (!ok) return NotFound(new { error = "There is no user with the specified email address." });
            return Ok(new { message = "Please check your email address!" });
        }
    }
}