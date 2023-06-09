﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spinfluence.Data;
using Spinfluence.Models;
using Spinfluence.Services;
using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
#pragma warning disable

namespace Spinfluence.Controllers
{
    public class HomeController : Controller
    {
        readonly IAccountService accountService;
        readonly ICompanyService companyService;

        public HomeController(IAccountService accountService, ICompanyService companyService)
        {
            this.accountService = accountService;
            this.companyService = companyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public async Task<IActionResult> Profile(int id, bool isLogo)
        {
            CompanyDetailsModel? company = await companyService.GetCompanyAsync(id);
            string filePath = isLogo ? company!.LogoImage : company!.PosterImage;

            int index = filePath.LastIndexOf(".");
            byte[] buffer = System.IO.File.ReadAllBytes(filePath);
            return File(buffer, $"image/{filePath.Substring(index + 1)}");
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create(CompanyModel companyModel)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            if (!string.IsNullOrEmpty(token))
            {
                var account = accountService.About(token);

                if (account != null && account!.admin)
                {
                    int res = await companyService.CreateCompanyAsync(companyModel);
                    if (res >= 0) return Ok();
                    else return BadRequest();
                }

                return Forbid();
            }

            return Unauthorized();
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> RemoveCompany(int id)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];
            var account = accountService.About(token);

            if (account != null)
            {
                // admin account have the remove grant of company
                if (account!.admin)
                {
                    bool res = await companyService.RemoveCompanyAsync(id);
                    if (res) return Ok();
                    return NotFound();
                }

                // forbid access for normal account
                return Forbid();
            }

            return Unauthorized();
        }

        public IActionResult About(int id)
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}