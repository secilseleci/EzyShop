using Models.Entities.Abstract;
using Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Concrete
{
    public class Order : IBaseEntity
    {
        public Order()
        {
            Id = Guid.NewGuid();
            OrderItems = new List<OrderItem>();
            OrderCode = GenerateOrderCode();

        }

        [Required]
        public Guid Id { get; set; }


        [Required]
        public string OrderCode { get; set; }


        [Required]
        public Guid CustomerId { get; set; }
        public AppUser Customer { get; set; }

       

        [Required]
        public Guid? ShopId { get; set; }
        [ForeignKey("ShopId")]
        public Shop? Shop { get; set; }


        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Status Status { get; set; } =  Status.Pending;


        public string? ShippingTrackingNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }



        private string GenerateOrderCode()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }
    }
}
        public enum PaymentMethod
        {
            CreditCard = 1,   
            BankTransfer = 2,  
            CashOnDelivery = 3   
        }
        public enum Status
        {
            Pending,
            Shipped,
            Delivered,
            Cancelled   
        }
 
