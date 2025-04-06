using Models.ViewModels.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Category
{
    public class CategoryViewModel : IImageViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } 

        

        [DisplayName("Image")]
        public string? ImageUrl { get; set; }

        public string FolderName { get; set; } = "category";
    }
}
