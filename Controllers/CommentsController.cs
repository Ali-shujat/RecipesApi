using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Data;
using RecipesApi.Models;
using RecipesApi.Models.Dtos;

namespace RecipesApi.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/recipes/{recipeId}/comments
        [HttpGet("api/recipes/{recipeId:guid}/comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(Guid recipeId)
        {
            var recipeExists = await _context.Recipes.AnyAsync(r => r.Id == recipeId);
            if (!recipeExists) return NotFound();

            var comments = await _context.Comments
                .Where(c => c.RecipeId == recipeId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserName = c.UserName
                })
                .ToListAsync();

            return Ok(comments);
        }

        // POST: api/recipes/{recipeId}/comments
        [HttpPost("api/recipes/{recipeId:guid}/comments")]
        public async Task<ActionResult<CommentDto>> PostComment(Guid recipeId, [FromBody] CommentCreateDto createDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null) return NotFound();

            var comment = new Comment
            {
                Content = createDto.Content,
                UserName = createDto.UserName,
                RecipeId = recipeId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var dto = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserName = comment.UserName
            };

            return CreatedAtAction(nameof(GetComments), new { recipeId }, dto);
        }

        // PUT: api/comments/{id}
        [HttpPut("api/comments/{id:guid}")]
        public async Task<IActionResult> PutComment(Guid id, [FromBody] CommentCreateDto updateDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            comment.Content = updateDto.Content;
            comment.UserName = updateDto.UserName;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/comments/{id}
        [HttpDelete("api/comments/{id:guid}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}