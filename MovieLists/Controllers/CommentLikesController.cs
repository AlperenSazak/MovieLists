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
    public class CommentLikesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentLikesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("comment/{commentId}/stats")]
        public async Task<ActionResult<CommentLikeStatsDto>> GetCommentLikeStats(int commentId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var likes = await _context.CommentLikes
                .Where(cl => cl.CommentId == commentId)
                .ToListAsync();

            var userLike = likes.FirstOrDefault(cl => cl.UserId == userId);

            var stats = new CommentLikeStatsDto
            {
                TotalLikes = likes.Count(l => l.IsLike),
                TotalDislikes = likes.Count(l => !l.IsLike),
                UserLiked = userLike?.IsLike
            };

            return Ok(stats);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleCommentLike(CreateCommentLikeDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var existingLike = await _context.CommentLikes
                .FirstOrDefaultAsync(cl => cl.CommentId == dto.CommentId && cl.UserId == userId);

            if (existingLike != null)
            {
                if (existingLike.IsLike == dto.IsLike)
                {
                    _context.CommentLikes.Remove(existingLike);
                }
                else
                {
                    existingLike.IsLike = dto.IsLike;
                }
            }
            else
            {
                var commentLike = new CommentLike
                {
                    CommentId = dto.CommentId,
                    UserId = userId,
                    IsLike = dto.IsLike,
                    CreatedAt = DateTime.UtcNow
                };
                _context.CommentLikes.Add(commentLike);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}