using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ProductFilterViewModel
    {
        public List<string> Colors { get; set; } = new(); // Mevcut renk seçenekleri
        public List<CategoryViewModel> Categories { get; set; } = new(); // Kategoriler

        [Display(Name = "Color")]
        public string? Color { get; set; } // Kullanıcının seçtiği renk

        [Display(Name = "Category")]
        public int? CategoryId { get; set; } // Kullanıcının seçtiği kategori

        [Display(Name = "Min Price")]
        public decimal? MinPrice { get; set; } // Minimum fiyat

        [Display(Name = "Max Price")]
        public decimal? MaxPrice { get; set; } // Maksimum fiyat

        [Display(Name = "Product Name")]
        public string? ProductName { get; set; } // Ürün adı ile arama
    }
}

