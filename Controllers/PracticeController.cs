using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> AddPractice(PracticeModel practiceModel)
        {
            string? token = Request.Cookies["token"];
            int res = await practiceService.AddPracticeAsync(practiceModel, token);

            if (res >= 0) return Redirect("/Practice/");
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
