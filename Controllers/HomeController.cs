﻿using Microsoft.AspNetCore.Mvc;
using Spinfluence.Data;
using Spinfluence.Models;
using Spinfluence.Services;
using System;
using System.Diagnostics;

namespace Spinfluence.Controllers
{
    public class HomeController : Controller
    {
        readonly ICompanyService companyService;

        public HomeController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        public IActionResult Index()
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

        [HttpPost]
        public async Task<IActionResult> Create(CompanyModel companyModel)
        {
            bool ok = await companyService.CreateCompanyAsync(companyModel);
            if (ok) return RedirectToAction("/Index");
            else return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCompany(int id)
        {
            bool res = await companyService.RemoveCompanyAsync(id);
            if (res) return Ok();
            return NotFound();
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