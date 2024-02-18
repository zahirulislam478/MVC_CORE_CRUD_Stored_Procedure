using Microsoft.AspNetCore.Mvc;

namespace Restaurant_Management.Controllers
{
    public class BookTablesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
