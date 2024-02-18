using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management.Models;

namespace Restaurant_Management.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RestaurantDbContext db;
        public OrdersController(RestaurantDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int id)
        {
            ViewBag.FoodId = id;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Order model)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlInterpolated($"EXEC InsertOrder {model.CustomerName}, {model.Phone}, {model.Email}, {model.Address}, {model.OrderDate}, {model.Quantity}, {model.TotalAmount}, {model.FoodId}");
                return RedirectToAction("Index", "Foods");

            }
            ViewBag.Foods = db.Foods.ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var data = db.Orders.FirstOrDefault(x => x.OrderId == id);
            if (data == null) { return NotFound(); }
            ViewBag.Foods = db.Foods.ToList();
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Order model)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlInterpolated($"EXEC UpdateOrder {model.OrderId}, {model.CustomerName}, {model.Phone}, {model.Email}, {model.Address}, {model.OrderDate}, {model.Quantity}, {model.TotalAmount}, {model.FoodId}");
                return RedirectToAction("Index", "Foods");

            }
            ViewBag.Devices = db.Foods.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteOrder {id}");
            return Json(new { success = true, id });
        }
    }
}
