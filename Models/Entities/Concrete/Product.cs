using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete
{
    public class Product:IBaseEntity
    {
        public Product()
        {
            Id = Guid.NewGuid();
            ProductImages = new List<ProductImage>();

        }
        [Required]
        public Guid Id { get; set; }

        

      
        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [JsonIgnore]
        public Category Category { get; set; }
        
        
        [Required]
        public Guid ShopId { get; set; }
        [ForeignKey("ShopId")]
        [JsonIgnore]
        public Shop Shop { get; set; }

        [JsonIgnore]
        public List<ProductImage> ProductImages { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }
     
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Stock { get; set; } = 1;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        public bool IsSoldOut => Stock <= 0;   
        public bool IsActive { get; set; } = true;   
        public bool IsDeleted { get; set; } = false;  
        public bool IsVisible => IsActive && !IsDeleted;

       
       
     

      

    }
}
