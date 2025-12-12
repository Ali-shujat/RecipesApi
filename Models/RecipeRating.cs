namespace RecipesApi.Models
{
    public class RecipeRating
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
    }
}
