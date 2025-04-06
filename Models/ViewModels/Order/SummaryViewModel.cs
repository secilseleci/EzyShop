using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Order
{
    public class SummaryViewModel
    {
        public Guid CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Address { get; set; }

        public List<ShopOrderViewModel> ShopOrders { get; set; }

        //[Required]
        //public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalAmount => ShopOrders?.Sum(s => s.TotalAmount) ?? 0;
    }
}
