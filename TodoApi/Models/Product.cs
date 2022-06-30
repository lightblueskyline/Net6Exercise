namespace TodoApi.Models
{
    public class Product
    {
        public int ID { get; set; }
        // ! = from Nullable to Non-Nullable
        // ? = from Non-Nullable to Nullable
        public string Name { get; set; } = null!; // 不允许为空值
        public decimal Price { get; set; } = 0;
        public bool IsOnSale { get; set; } = false;
    }
}
