using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models
{
    public class Ingredient
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;

        // Foreign key to Recipe
        public Guid RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}