using Models.ViewModels.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ProductViewModel: IImageViewModel
    {
       
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }
        public string? Description { get; set; }

        [Required]
        [DisplayName("List Price")]
        [Range(1, 10000)]
        public decimal ListPrice { get; set; }


        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; } = 1;

        [Required(ErrorMessage = "Category required")]
        [DisplayName("Category")]
        public Guid CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string FolderName { get; set; } = "product";
    }
}
