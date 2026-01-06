using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Data;
using RecipesApi.Models;
using RecipesApi.Models.Dtos;

namespace RecipesApi.Controllers
{
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RatingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/recipes/{recipeId}/ratings
        [HttpGet("api/recipes/{recipeId:guid}/ratings")]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatings(Guid recipeId)
        {
            var recipeExists = await _context.Recipes.AnyAsync(r => r.Id == recipeId);
            if (!recipeExists) return NotFound();

            var ratings = await _context.Ratings
                .Where(r => r.RecipeId == recipeId)
                .OrderBy(r => r.CreatedAt)
                .Select(r => new RatingDto
                {
                    Id = r.Id,
                    Value = r.Value,
                    CreatedAt = r.CreatedAt,
                    UserName = r.UserName
                })
                .ToListAsync();

            return Ok(ratings);
        }

        // POST: api/recipes/{recipeId}/ratings
        [HttpPost("api/recipes/{recipeId:guid}/ratings")]
        public async Task<ActionResult<RatingDto>> PostRating(Guid recipeId, [FromBody] RatingCreateDto createDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null) return NotFound();

            var rating = new Rating
            {
                Value = createDto.Value,
                UserName = createDto.UserName,
                RecipeId = recipeId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            var dto = new RatingDto
            {
                Id = rating.Id,
                Value = rating.Value,
                CreatedAt = rating.CreatedAt,
                UserName = rating.UserName
            };

            return CreatedAtAction(nameof(GetRatings), new { recipeId }, dto);
        }

        // PUT: api/ratings/{id}
        [HttpPut("api/ratings/{id:guid}")]
        public async Task<IActionResult> PutRating(Guid id, [FromBody] RatingCreateDto updateDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null) return NotFound();

            rating.Value = updateDto.Value;
            rating.UserName = updateDto.UserName;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ratings/{id}
        [HttpDelete("api/ratings/{id:guid}")]
        public async Task<IActionResult> DeleteRating(Guid id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null) return NotFound();

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}