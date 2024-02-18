using Microsoft.AspNetCore.Mvc;

namespace Restaurant_Management.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
