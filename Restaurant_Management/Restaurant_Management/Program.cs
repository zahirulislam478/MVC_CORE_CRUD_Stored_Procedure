using Microsoft.EntityFrameworkCore;
using Restaurant_Management.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RestaurantDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();