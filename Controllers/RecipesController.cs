using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Data;
using RecipesApi.Models;
using RecipesApi.Models.Dtos;

namespace RecipesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes()
        {
            var recipes = await _context.Recipes
                .Select(r => new RecipeDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    ImageUrl = r.ImageUrl,
                    TimeInMins = r.TimeInMins,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    Comments = r.Comments
                        .OrderBy(c => c.CreatedAt)
                        .Select(c => new CommentDto
                        {
                            Id = c.Id,
                            Content = c.Content,
                            CreatedAt = c.CreatedAt,
                            UserName = c.UserName
                        })
                        .ToList(),
                    Ratings = r.Ratings
                        .Select(x => new RatingDto
                        {
                            Id = x.Id,
                            Value = x.Value,
                            CreatedAt = x.CreatedAt,
                            UserName = x.UserName
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(recipes);
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(Guid id)
        {
            var recipe = await _context.Recipes
                .Where(r => r.Id == id)
                .Select(r => new RecipeDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    ImageUrl = r.ImageUrl,
                    TimeInMins = r.TimeInMins,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    Comments = r.Comments
                        .OrderBy(c => c.CreatedAt)
                        .Select(c => new CommentDto
                        {
                            Id = c.Id,
                            Content = c.Content,
                            CreatedAt = c.CreatedAt,
                            UserName = c.UserName
                        })
                        .ToList(),
                    Ratings = r.Ratings
                        .Select(x => new RatingDto
                        {
                            Id = x.Id,
                            Value = x.Value,
                            CreatedAt = x.CreatedAt,
                            UserName = x.UserName
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(Guid id, [FromBody] RecipeUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipe.Title = updateDto.Title;
            recipe.Description = updateDto.Description;
            recipe.ImageUrl = updateDto.ImageUrl;
            recipe.TimeInMins = updateDto.TimeInMins;
            recipe.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/Recipes
        [HttpPost]
        public async Task<ActionResult<RecipeDto>> PostRecipe([FromBody] RecipeCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var recipe = new Recipe
            {
                Title = createDto.Title,
                Description = createDto.Description,
                ImageUrl = createDto.ImageUrl,
                TimeInMins = createDto.TimeInMins,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            var resultDto = new RecipeDto
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl,
                TimeInMins = recipe.TimeInMins,
                CreatedAt = recipe.CreatedAt,
                UpdatedAt = recipe.UpdatedAt
            };

            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, resultDto);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(Guid id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}