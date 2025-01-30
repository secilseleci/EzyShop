using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class CategorySeeder
    {
        public static async Task SeedCategoriesAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.AddRange(new List<Category>
                {
                    new Category { Name = "Electronics" ,DisplayOrder=2, ImageUrl= "images/category/electronics.webp"},
                    new Category { Name = "Books", DisplayOrder=1 ,ImageUrl="images/category/books.webp" },
                    new Category { Name = "Fashion",DisplayOrder=3,ImageUrl = "images/category/fashion.webp" },
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
