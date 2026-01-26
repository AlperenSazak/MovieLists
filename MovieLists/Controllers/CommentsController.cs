using Core.DTOs;
using Core.Entities;
using Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MovieLists.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments/movie/5
        [HttpGet("movie/{movieId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetMovieComments(int movieId)
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.MovieId == movieId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    MovieId = c.MovieId,
                    UserId = c.UserId,
                    Username = c.User.Username
                })
                .ToListAsync();

            return Ok(comments);
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var comment = new Comment
            {
                Content = dto.Content,
                MovieId = dto.MovieId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var createdComment = await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == comment.Id);

            var commentDto = new CommentDto
            {
                Id = createdComment.Id,
                Content = createdComment.Content,
                CreatedAt = createdComment.CreatedAt,
                UpdatedAt = createdComment.UpdatedAt,
                MovieId = createdComment.MovieId,
                UserId = createdComment.UserId,
                Username = createdComment.User.Username
            };

            return CreatedAtAction(nameof(GetMovieComments), new { movieId = comment.MovieId }, commentDto);
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, UpdateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return NotFound();

            if (comment.UserId != userId)
                return Forbid();

            comment.Content = dto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return NotFound();

            if (comment.UserId != userId)
                return Forbid();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}