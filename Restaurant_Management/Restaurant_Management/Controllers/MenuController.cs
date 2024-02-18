using Microsoft.AspNetCore.Mvc;

namespace Restaurant_Management.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
