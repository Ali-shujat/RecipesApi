using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models.Dtos
{
    public class RecipeDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int TimeInMins { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<CommentDto> Comments { get; set; } = new();
        public List<RatingDto> Ratings { get; set; } = new();
    }

    public class RecipeCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int TimeInMins { get; set; }
    }

    public class RecipeUpdateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int TimeInMins { get; set; }
    }
}