using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipesApi.Models;
using System.Text.Json;

namespace RecipesApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<User> Users => Set<User>();
        public DbSet<RecipeRating> RecipeRatings => Set<RecipeRating>();

        public DbSet<RecipeComment> RecipeComments => Set<RecipeComment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Store List<string> as JSON text for simple collections (categories, instructions, ratings)
            var stringListConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

            var intListConverter = new ValueConverter<List<int>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>());

            modelBuilder.Entity<Recipe>(eb =>
            {
                eb.HasKey(r => r.Id);
                eb.Property(r => r.CategoriesJson).HasColumnName("Categories");
                eb.Property(r => r.InstructionsJson).HasColumnName("Instructions");
                eb.Property(r => r.RatingsJson).HasColumnName("Ratings");
                // eb.OwnsMany(r => r.Ingredients);

                // Map JSON-backed properties to converters
                eb.Property(r => r.CategoriesJson).HasConversion(stringListConverter);
                eb.Property(r => r.InstructionsJson).HasConversion(stringListConverter);
                eb.Property(r => r.RatingsJson).HasConversion(intListConverter);
            });

            modelBuilder.Entity<Ingredient>(ib =>
            {
                ib.HasKey(i => i.Id);
                // Ingredients will be stored in a separate table as owned or normal entity
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
