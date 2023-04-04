using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spinfluence.Data;
using Spinfluence.Models;
using Spinfluence.Services;
using System;
using System.Diagnostics;

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
            var account = await accountService.About(token);

            if (account != null && account!.admin)
            {
                bool ok = await companyService.CreateCompanyAsync(companyModel);
                if (ok) return RedirectToAction("/Index");
                else return BadRequest();
            }

            return Unauthorized();
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> RemoveCompany(int id)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];
            var account = await accountService.About(token);

            if (account != null && account!.admin)
            {
                bool res = await companyService.RemoveCompanyAsync(id);
                if (res) return Ok();
                return NotFound();
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