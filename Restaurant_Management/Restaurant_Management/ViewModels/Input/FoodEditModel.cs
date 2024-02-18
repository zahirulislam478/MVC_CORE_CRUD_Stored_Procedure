using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant_Management.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Restaurant_Management.ViewModels.Input
{   
    public class FoodEditModel
    {
        public int FoodId { get; set; }
        [Required, StringLength(100)]
        public string FoodName { get; set; } = default!;
        [EnumDataType(typeof(FoodCategory))]
        public FoodCategory Category { get; set; }
        [Required, StringLength(100)]
        public string Description { get; set; } = default!;

        [Required, Column(TypeName = "money")]
        public decimal Price { get; set; }
        public IFormFile? Picture { get; set; } = default!;
        public bool IsAvailable { get; set; }
        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
