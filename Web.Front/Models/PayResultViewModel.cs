using Microsoft.AspNetCore.Components.Web;

namespace Web.Front.Models
{
    public class PayResultViewModel
    {
        public string Status { get; set; } = null!;
        public string? AuthCode { get; set; }
        public string Message { get; set; } = null!;
        public decimal? Change { get; set; }
        public bool Success { get; set; }
    }
}
