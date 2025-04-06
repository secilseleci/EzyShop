using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete;

public class Category :BaseEntity
{ 

    [Required]
    public  string Name { get; set; } = null!;


    public string? ImageUrl { get; set; }= string.Empty;

    [JsonIgnore]
    public ICollection<Product> Products { get; set; }=[];

}
