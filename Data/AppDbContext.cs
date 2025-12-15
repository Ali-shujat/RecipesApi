using Microsoft.EntityFrameworkCore;
using RecipesApi.Models;

namespace RecipesApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options) { }

        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Rating> Ratings => Set<Rating>();

    }
}
