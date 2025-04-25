namespace Models.DTOs;

public class ProductListDto
{
    public Guid CategoryId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string? ImageUrl { get; set; }

}