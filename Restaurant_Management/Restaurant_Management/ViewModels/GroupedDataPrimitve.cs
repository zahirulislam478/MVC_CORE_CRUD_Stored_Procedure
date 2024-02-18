namespace Restaurant_Management.ViewModels
{
    public class GroupedDataPrimitve<T>
    {
        public string? Key { get; set; } = default!;
        public T Data { get; set; } = default!;
    }
}
