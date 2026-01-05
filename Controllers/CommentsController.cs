using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Data;
using RecipesApi.Models;

namespace RecipesApi.Controllers
{
    // Route is explicit and will not overlap with other controllers:
    // GET  api/recipes/{recipeId}/comments
    // POST api/recipes/{recipeId}/comments
    [Route("api/recipes/{recipeId:guid}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/recipes/{recipeId}/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(Guid recipeId)
        {
            var recipeExists = await _context.Recipes.AnyAsync(r => r.Id == recipeId);
            if (!recipeExists)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Where(c => c.RecipeId == recipeId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            return Ok(comments);
        }

        // POST: api/recipes/{recipeId}/comments
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Guid recipeId, Comment comment)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                return NotFound();
            }

            // Ensure the comment is linked to the route recipe and set timestamps
            comment.RecipeId = recipeId;
            comment.CreatedAt = DateTime.UtcNow;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Return the comments collection location
            return CreatedAtAction(nameof(GetComments), new { recipeId }, comment);
        }
    }
}
