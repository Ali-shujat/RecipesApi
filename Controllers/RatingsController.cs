using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Data;
using RecipesApi.DTOs;
using RecipesApi.Models;

namespace RecipesApi.Controllers
{
    [ApiController]
    [Route("api/recipes/{recipeId}/ratings")]
    public class RatingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RatingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddRating(Guid recipeId, CreateRatingDto dto)
        {
            if (dto.Value < 1 || dto.Value > 5)
                return BadRequest("Rating must be between 1 and 5");

            var rating = new Rating
            {
                Id = Guid.NewGuid(),
                RecipeId = recipeId,
                UserName = dto.UserName,
                Value = dto.Value,
                CreatedAt = DateTime.UtcNow
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return Ok(rating);
        }

        [HttpGet("average")]
        public async Task<IActionResult> GetAverageRating(Guid recipeId)
        {
            var avg = await _context.Ratings
                .Where(r => r.RecipeId == recipeId)
                .AverageAsync(r => (double?)r.Value) ?? 0;

            return Ok(new { averageRating = Math.Round(avg, 1) });
        }
    }
}