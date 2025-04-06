using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders;

public static class CategorySeeder
{
    public static async Task SeedCategoriesAsync(ApplicationDbContext dbContext)
    {
        if (!dbContext.Categories.Any())
        {
            dbContext.Categories.AddRange(new List<Category>
            {
                new Category {
                    Id=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19"),
                    Name = "Electronics" ,
                    ImageUrl= "images/category/electronics.jpg"},
                new Category {
                    Id=Guid.Parse("279ac61d-0691-4d5a-aab0-caca11ed28c2"),
                    Name = "Books", 
                    ImageUrl="images/category/books.jpg" },
                new Category { 
                    Id=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959"),
                    Name = "Fashion",
                    ImageUrl = "images/category/fashion.jpg" },
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
