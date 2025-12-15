namespace RecipesApi.Models
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string UserName { get; set; } = "";
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }

        public Recipe? Recipe { get; set; }
    }
}
