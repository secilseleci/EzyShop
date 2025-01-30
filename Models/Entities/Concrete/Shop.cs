using Models.Entities.Abstract;
using Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Concrete
{
    public class Shop:IBaseEntity
    {
        public Shop()
        {
            Id = Guid.NewGuid();
            Products = new List<Product>();
        }
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        public AppUser Seller { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; }

    }
}
