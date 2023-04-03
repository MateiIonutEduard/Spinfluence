using Microsoft.AspNetCore.Mvc;

namespace Spinfluence.Controllers
{
    public class PracticeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /*[HttpPost]
        public IActionResult AddPractice()
        {
            return View();
        }*/
    }
}
