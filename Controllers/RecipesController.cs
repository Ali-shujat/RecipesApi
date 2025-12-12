using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Data;
using RecipesApi.Models;

namespace RecipesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public RecipesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/recipes
        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> GetAll()
        {
            var recipes = await _db.Recipes
                .Include(r => r.Ingredients)
                .ToListAsync();
            return Ok(recipes);
        }

        // GET api/recipes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> Get(Guid id)
        {
            var recipe = await _db.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null) return NotFound();
            return Ok(recipe);
        }

        // POST api/recipes
        [HttpPost]
        public async Task<ActionResult<Recipe>> Create([FromBody] Recipe input)
        {
            input.Id = Guid.NewGuid();
            input.CreatedAt = DateTime.UtcNow;
            input.UpdatedAt = DateTime.UtcNow;

            if (input.Ingredients != null)
            {
                foreach (var ing in input.Ingredients)
                {
                    ing.Id = Guid.NewGuid();
                    ing.RecipeId = input.Id;
                }
            }

            _db.Recipes.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = input.Id }, input);
        }

        // PUT api/recipes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Recipe input)
        {
            var recipe = await _db.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null) return NotFound();

            // Update scalar properties
            recipe.Title = input.Title;
            recipe.Description = input.Description;
            recipe.ImageUrl = input.ImageUrl;
            recipe.TimeInMins = input.TimeInMins;
            recipe.UpdatedAt = DateTime.UtcNow;

            recipe.Categories = input.Categories ?? new List<string>();
            recipe.Instructions = input.Instructions ?? new List<string>();
            recipe.Ratings = input.Ratings ?? new List<int>();

            // Replace ingredients: simple approach - remove existing and add incoming
            _db.Ingredients.RemoveRange(recipe.Ingredients);
            recipe.Ingredients.Clear();

            if (input.Ingredients != null)
            {
                foreach (var ing in input.Ingredients)
                {
                    ing.Id = Guid.NewGuid();
                    ing.RecipeId = recipe.Id;
                    recipe.Ingredients.Add(ing);
                }
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/recipes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var recipe = await _db.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null) return NotFound();

            _db.Ingredients.RemoveRange(recipe.Ingredients);
            _db.Recipes.Remove(recipe);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
