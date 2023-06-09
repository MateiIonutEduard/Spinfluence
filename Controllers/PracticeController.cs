﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spinfluence.Models;
using Spinfluence.Services;
#pragma warning disable

namespace Spinfluence.Controllers
{
    public class PracticeController : Controller
    {
        readonly IPracticeService practiceService;

        public PracticeController(IPracticeService practiceService)
        { this.practiceService = practiceService; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Notifications()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchPractice(PracticeEventSearchFilter practiceEventSearchFilter)
        {
            ViewData["filter"] = practiceEventSearchFilter;
            return View($"Views/Practice/Index.cshtml", ViewData["filter"]);
        }

        public async Task<IActionResult> Show(int id, string? type)
        {
            string path = await practiceService.GetPracticeAsync(id, type);
            int index = path.LastIndexOf('.');

            string ext = path.Substring(index + 1);
            byte[] buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, practiceService.GetContentType(path));
        }

        public async Task<IActionResult> AddPractice(PracticeModel practiceModel)
        {
            string? token = Request.Cookies["token"];
            int res = await practiceService.AddPracticeAsync(practiceModel, token);

            if (res >= 0) return Redirect("/Practice/");
            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<IActionResult> SearchNotifications(PracticeEventSearchFilter practiceEventSearchFilter)
        {
            ViewData["filter"] = practiceEventSearchFilter;
            return View($"Views/Practice/Notifications.cshtml", ViewData["filter"]);
        }

        [HttpPost]
        public async Task<IActionResult> ApprovePractice(int? id, bool IsApproved)
        {
            string? token = HttpContext.Request.Cookies["token"];

            if (id != null)
            {
                /* update students practices */
                await practiceService.ApprovePracticeAsync(token, id.Value, IsApproved);
                return Redirect("/Practice/Notifications");
            }
            else
                return Redirect("/Account/Login");
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> RemovePractice(int? id)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            if (id != null)
            {
                int res = await practiceService.CancelPracticeEventAsync(id.Value, token);
                if (res == 1) return Ok();
                else if (res < 0) return Unauthorized();
            }

            return NotFound();
        }
    }
}
