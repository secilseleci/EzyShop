using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Concrete
{
    public class ShoppingCartItem : IBaseEntity
    {
        public ShoppingCartItem()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CartId { get; set; }

        [ForeignKey("CartId")]
        public ShoppingCart Cart { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Range(1, 10, ErrorMessage = "Please enter a value between 1 and 10")]
        public int Count { get; set; } = 1;

        [Required]
        public decimal PriceAtPurchase { get; set; }  
    }
}
