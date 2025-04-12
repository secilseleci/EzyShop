using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete
{
    public class Product:BaseEntity
    { 
      
        [Required]
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [JsonIgnore]
        public  Category Category { get; set; } = null!;


        [Required]
        public Guid ShopId { get; set; }
        [ForeignKey("ShopId")]
        [JsonIgnore]
        public  Shop Shop { get; set; } = null!;


        [JsonIgnore]
        public ICollection<ProductImage> ProductImages { get; set; } = [];

        [JsonIgnore]
        public ICollection<CartLine>  CartLines { get; set; } = [];
        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; } = [];



        [Required]
        public  string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Stock { get; set; } = 1;
        public string? Color { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public bool IsSoldOut => Stock <= 0;   
        public bool IsActive { get; set; } = true;   
 
        
    }
}
