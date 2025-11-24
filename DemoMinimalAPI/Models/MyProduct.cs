namespace DemoMinimalAPI.Models
{
    public class MyProduct
    {
        public int Id { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public string AlgorithmName { get; set; } = "";
    }
}
