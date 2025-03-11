using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete
{
    public class ProductImage
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        [JsonIgnore] 
        public Product Product { get; set; }
    }
}
