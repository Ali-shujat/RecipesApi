using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models
{
    public class Recipe
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int TimeInMins { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Backing fields stored as JSON in the DB
        public List<string> CategoriesJson { get; set; } = new();
        public List<string> InstructionsJson { get; set; } = new();
        public List<int> RatingsJson { get; set; } = new();

        // Navigation collection for ingredients
        public List<Ingredient> Ingredients { get; set; } = new();

        // Helper properties for (de)serialization/layer mapping
        public List<string> Categories
        {
            get => CategoriesJson;
            set => CategoriesJson = value;
        }
        public List<string> Instructions
        {
            get => InstructionsJson;
            set => InstructionsJson = value;
        }
        public List<int> Ratings
        {
            get => RatingsJson;
            set => RatingsJson = value;
        }
    }
}
