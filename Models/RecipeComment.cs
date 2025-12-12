namespace RecipesApi.Models
{
    public class RecipeComment
    {
        public Guid Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
    }
}
