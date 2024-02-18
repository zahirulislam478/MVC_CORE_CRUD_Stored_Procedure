using Restaurant_Management.Models;

namespace Restaurant_Management.ViewModels
{
    public class GroupedData
    {      
            public string? Key { get; set; } = default!;
            public IEnumerable<Order> Data { get; set; } = default!;       
    }
}
