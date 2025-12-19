namespace Application.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string SKU { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
