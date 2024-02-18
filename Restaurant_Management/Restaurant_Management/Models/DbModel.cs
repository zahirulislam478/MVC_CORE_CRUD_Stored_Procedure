using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace Restaurant_Management.Models
{
    public enum FoodCategory
    {
        Appetizer = 1,
        MainCourse,
        Dessert,
        Beverage,
        Snack
    }
    public class Food
    {
        public int FoodId { get; set; }
        [Required, StringLength(100)]
        public string FoodName { get; set; } = default!;
        [EnumDataType(typeof(FoodCategory))]
        public FoodCategory? Category { get; set; } = default!;
        [Required, StringLength(100)]
        public string Description { get; set; } = default!;

        [Required, Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Required, StringLength(100)]
        public string? Picture { get; set; } = default!;
        public bool? IsAvailable { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
    public class Order
    {
        public int OrderId { get; set; }
        [Required, StringLength(100)]
        public string? CustomerName { get; set; } = default!;
        [Required]
        public int? Phone { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; } 
        [Required, StringLength(100)]
        public string? Address { get; set; } 

        [Required, DataType(DataType.DateTime)]
        public DateTime? OrderDate { get; set; }
        public int Quantity { get; set; }
        [Required, Column(TypeName=("money"))]
        public decimal? TotalAmount { get; set; }
        [Required, ForeignKey("Food")]
        public int FoodId { get; set; }
        public virtual Food? Food { get; set; } 
    }
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) { }
        public DbSet<Food> Foods { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Random random = new Random();

            for (int i = 1; i <= 5; i++)
            {
                modelBuilder.Entity<Food>().HasData(
                    new Food
                    {
                        FoodId = i,
                        FoodName = "Food " + i,
                        Category = (FoodCategory)random.Next(1, 5),
                        Description = "ABC" + i,
                        Price = random.Next(1000, 2000) * 1.00M,
                        IsAvailable = true,
                        Picture = i + ".jpg"
                    }
                );
            }

            for (int i = 1; i <= 8; i++)
            {
                modelBuilder.Entity<Order>().HasData(
                    new Order
                    {
                        OrderId = i,
                        CustomerName = "Customer " + i,  
                        Phone = random.Next(100000000, 999999999), // Assuming 9-digit phone number
                        Email = $"customer{i}@example.com",  // Assuming a simple email pattern
                        Address = "Address " + i,
                        OrderDate = DateTime.Now.AddDays(-1 * random.Next(200, 500)),
                        Quantity = random.Next(100, 300),
                        TotalAmount = random.Next(1000, 2000) * 1.00M,
                        FoodId = i % 5 == 0 ? 5 : i % 5
                    }
                );
            }
        }
    }
}
