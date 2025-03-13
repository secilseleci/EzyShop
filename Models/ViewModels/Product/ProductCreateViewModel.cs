using Models.ViewModels.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
 
namespace Models.ViewModels.Product
{
    public class ProductCreateViewModel : IImageViewModel
    {

    

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }
          
        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; } = 1;
        
        public string? Description { get; set; }
         public string? ImageUrl { get; set; }
        public string FolderName { get; set; } = "product";

        public DateTime CreatedDate { get; set; }

        public bool IsSoldOut { get; set; }   
        public bool IsActive { get; set; } = true;   
        public bool IsDeleted { get; set; } = false;  
        public bool IsVisible => IsActive && !IsDeleted;   


        [Required(ErrorMessage = "Category required")]
        [DisplayName("Category")]
        public Guid CategoryId { get; set; }
        

        
        [Required(ErrorMessage = "Shop required")]
        [DisplayName("Shop")]
        public Guid ShopId { get; set; }

    }
}
