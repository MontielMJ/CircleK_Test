namespace Web.Front.Models
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public List<SaleViewModel> ItemsSale { get; set; } = new();
        public decimal Subtotal => Items.Sum(i => i.Total);
        public decimal Tax => Math.Round(Subtotal * 0.16m, 2);
        public decimal Total => Subtotal + Tax;
    }

}
