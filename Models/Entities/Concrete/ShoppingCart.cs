using Models.Entities.Abstract;
using Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models.Entities.Concrete
{
    public class ShoppingCart : IBaseEntity
    {
        public ShoppingCart()
        {
            Id = Guid.NewGuid();
            CartItems = new List<ShoppingCartItem>();

        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        public ICollection<ShoppingCartItem> CartItems { get; set; }

    }
}
