using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Data;
using RecipesApi.DTOs;
using RecipesApi.Models;

namespace RecipesApi.Controllers
{
    [ApiController]
    [Route("api/recipes/{recipeId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Guid recipeId, CreateCommentDto dto)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                RecipeId = recipeId,
                UserName = dto.UserName,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(Guid recipeId)
        {
            var comments = await _context.Comments
                .Where(c => c.RecipeId == recipeId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return Ok(comments);
        }
    }

}
