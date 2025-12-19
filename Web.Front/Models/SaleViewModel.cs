namespace Web.Front.Models
{
    public class SaleViewModel
    {

        public int Id { get; set; }
        public string Folio { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
