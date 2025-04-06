using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete;
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderItems = [];
            OrderCode = GenerateOrderCode();

        }

        [Required]
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!;



        [Required]
        public Guid ShopId { get; set; }
        [ForeignKey("ShopId")]
        public Shop Shop { get; set; } = null!;

        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }


        [Required]
        public string OrderCode { get; set; } = null!;

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;


        public string? ShippingTrackingNumber { get; set; } = string.Empty;

        private string GenerateOrderCode()
        {
        return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }
}

        public enum PaymentMethod
        {
            CreditCard = 1,   
            BankTransfer = 2,  
            CashOnDelivery = 3   
        }
        public enum OrderStatus
{
            Pending=1,
            Processing = 2,
            Shipped = 3,
            Delivered=4,
            Cancelled=5
        }
 
