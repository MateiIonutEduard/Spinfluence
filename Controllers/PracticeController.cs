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
            bool ok = await practiceService.AddPracticeAsync(practiceModel, token);

            if (ok) return RedirectToAction("/Index");
            return Redirect("/Account/Login");
        }

        [HttpDelete]
        public async Task<IActionResult> RemovePractice(int? id)
        {
            string? token = Request.Cookies["token"];

            if(id != null)
            {
                int res = await practiceService.CancelPracticeEventAsync(id.Value, token);
                if (res == 1) return RedirectToAction("/Index");
                else if (res < 0) return Redirect("/Account/Login");
            }

            return RedirectToAction("/Index");
        }
    }
}
