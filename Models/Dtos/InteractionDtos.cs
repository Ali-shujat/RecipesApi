using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class CommentCreateDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;
    }

    public class RatingDto
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class RatingCreateDto
    {
        [Range(1, 5)]
        public int Value { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;
    }
}