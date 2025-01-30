using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }
        public string? Description { get; set; }

        [Required]
        public decimal ListPrice { get; set; }


        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; } = 1;

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public string? ImageUrl { get; set; }

 
     

        public List<ProductImage> ProductImages { get; set; }
       
        [Required]
        public Guid ShopId { get; set; }  

        public Shop Shop { get; set; }
    }
}
