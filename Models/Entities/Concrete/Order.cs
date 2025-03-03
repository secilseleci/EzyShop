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

        }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public AppUser Customer { get; set; }

        [Required]
        public Guid SellerId { get; set; }
        [ForeignKey("SellerId")]
        public AppUser Seller { get; set; }

        [Required]
        public Guid ShopId { get; set; }
        [ForeignKey("ShopId")]
        public Shop Shop { get; set; }


        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Status Status { get; set; } =  Status.Pending;


        public string? ShippingTrackingNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}
        public enum Status
        {
            Pending,
            Shipped,
            Delivered,
            Cancelled   
}
 
