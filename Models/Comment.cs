using System.Text.Json.Serialization;

namespace RecipesApi.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid RecipeId { get; set; }
        public string UserName { get; set; } = string.Empty;

        // Prevent object-cycle during JSON serialization
        [JsonIgnore]
        public Recipe? Recipe { get; set; }
    }
}
