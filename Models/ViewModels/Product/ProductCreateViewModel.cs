﻿using Models.ViewModels.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
 
namespace Models.ViewModels.Product;

public class ProductCreateViewModel : IImageViewModel
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string? Color { get; set; } 

    [Required]
    [Range(1, 100000)]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; } = 1;
    
    public string? ImageUrl { get; set; }
    public string FolderName { get; set; } = "product";

     
    public bool IsActive { get; set; } = true;   
 

    [Required(ErrorMessage = "Category required")]
    [DisplayName("Category")]
    public Guid CategoryId { get; set; }
    


}
