using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant_Management.Models;
using Restaurant_Management.ViewModels;
using Restaurant_Management.ViewModels.Input;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using X.PagedList;

namespace Restaurant_Management.Controllers
{
    public class FoodsController : Controller
    {
        private readonly RestaurantDbContext db;
        private readonly IWebHostEnvironment env;
        public FoodsController(RestaurantDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            return View(await db.Foods.OrderBy(x => x.FoodId).Include(x => x.Orders).ToPagedListAsync(pg, 5));
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FoodInputModel model)
        {            
            if (ModelState.IsValid)
            {              
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertFood {model.FoodName}, {(int)model.Category}, {model.Description}, {model.Price}, {model.Picture}, {(model.IsAvailable ? 1 : 0)}");
                var id = await db.Foods.MaxAsync(x => x.FoodId);
               
                foreach (var order in model.Orders)
                {
                    await db.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertOrder {order.CustomerName}, {order.Phone}, {order.Email}, {order.Address}, {order.OrderDate}, {order.Quantity}, {order.TotalAmount}, {id}");
                }
                return Json(new { success = true });
            }            
            return Json(new { success = false});           
        }
        public IActionResult GetOrderForm()
        {
            return PartialView("_GetOrdersForm");
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            string ext = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
            string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
            FileStream fs = new FileStream(savePath, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return Json(new { name = fileName });
        }
        public async Task<IActionResult> Edit(int id)
        {
            var data = await db.Foods.FirstOrDefaultAsync(x => x.FoodId == id);
            if (data == null) return NotFound();

            return View(new FoodEditModel
            {
                FoodId = data.FoodId,
                FoodName = data.FoodName,
                Category = data.Category ?? FoodCategory.Appetizer,
                Description =data.Description,               
                Price = data.Price,
                IsAvailable = data.IsAvailable ?? false
            });           
        }
        [HttpPost]
        public async Task<IActionResult> Edit(FoodEditModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await db.Foods.FirstOrDefaultAsync(x => x.FoodId == model.FoodId);
                if (product == null) return NotFound();
                product.FoodId = model.FoodId;
                product.FoodName = model.FoodName;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Category = model.Category;
                product.IsAvailable = model.IsAvailable;

                if (model.Picture != null)
                {
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
                    FileStream fs = new FileStream(savePath, FileMode.Create);
                    await model.Picture.CopyToAsync(fs);
                    product.Picture = fileName;
                    fs.Close();
                }
                db.Database.ExecuteSqlInterpolated($@"EXEC UpdateFood 
                                        {product.FoodId}, 
                                        {product.FoodName}, 
                                        {(int)product.Category}, 
                                        {product.Description}, 
                                        {product.Price}, 
                                        {product.Picture}, 
                                        {(model.IsAvailable ? 1 : 0)}");

                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteFood {id}");
            return Json(new { success = true, id });
        }
        public async Task<IActionResult> Aggregates()
        {
            var data = await db.Orders.Include(x => x.Food)
                .ToListAsync();
            return View(data);
        }

        public IActionResult Grouping()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Grouping(string groupby)
        {

            if (groupby == "foodname")
            {
                var data = db.Orders.Include(x => x.Food)
               .ToList()
               .GroupBy(x => x.Food?.FoodName)
               .Select(g => new GroupedData { Key = g.Key, Data = g })
               .ToList();

                return View("GroupingResult", data); 
            }
            if (groupby == "year month")
            {
                var data = db.Orders.Include(x => x.Food)
                    .OrderByDescending(x => x.OrderDate)
               .ToList()
               .GroupBy(x => $"{x.OrderDate:MMM, yyyy}")
               .Select(g => new GroupedData { Key = g.Key, Data = g })
               .ToList();

                return View("GroupingResult", data);
            }
            if (groupby == "count")
            {
                var data = db.Orders.Include(x => x.Food)
                    .OrderByDescending(x => x.OrderDate)
               .ToList()
               .GroupBy(x => x.Food?.FoodName)
               .Select(g => new GroupedDataPrimitve<int> { Key = g.Key, Data = g.Count() })
               .ToList();

                return View("GroupingResultPrimitive", data);
            }

            return RedirectToAction("Grouping");
        }      

    }
}
