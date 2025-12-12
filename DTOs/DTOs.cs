namespace RecipesApi.DTOs
{


    public record RecipeDto(Guid Id, string Title, string Description, string? ImageUrl, int TimeInMins, List<string> Categories, List<string> Instructions);
    public record CreateRecipeDto(string Title, string Description, string? ImageUrl, int TimeInMins, List<string> Categories, List<string> Instructions);
    public record UpdateRecipeDto(string Title, string Description, string? ImageUrl, int TimeInMins, List<string> Categories, List<string> Instructions);

}
