using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Concrete
{
    public class OrderItem : IBaseEntity
    {
        public OrderItem()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }  // 🟢 Order ile ilişki
 

        [Required]
        public string ProductName { get; set; }  // 🟢 Ürün adı sabitlenmeli

        [Required]
        public decimal ProductPrice { get; set; }  // 🟢 Ürünün sipariş anındaki fiyatı 

        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int Count { get; set; } = 1;  // 🟢 Ürün adedi
    }
}
