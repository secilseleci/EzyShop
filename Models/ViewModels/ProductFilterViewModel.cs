using System.Collections.Generic;

namespace Models.ViewModels
{
    public class ProductFilterViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new();
        public string? SelectedCategory { get; set; }
        public string? SelectedColor { get; set; }
        public string? ProductName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
