using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
 
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
        public Order Order { get; set; }   
 

        [Required]
        public string ProductName { get; set; }  

        [Required]
        public decimal ProductPrice { get; set; }
        
        [Required]
        public string Color { get; set; }
        public string? ImageUrl { get; set; }  

        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int Count { get; set; } = 1;   
}
}