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
    public class MovieLikesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieLikesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("movie/{movieId}/stats")]
        [AllowAnonymous]
        public async Task<ActionResult<MovieLikeStatsDto>> GetMovieLikeStats(int movieId)
        {
            var userId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

            var likes = await _context.MovieLikes
                .Where(l => l.MovieId == movieId)
                .ToListAsync();

            var userLike = userId.HasValue
                ? await _context.MovieLikes.FirstOrDefaultAsync(l => l.MovieId == movieId && l.UserId == userId.Value)
                : null;

            var stats = new MovieLikeStatsDto
            {
                TotalLikes = likes.Count(l => l.IsLiked),
                TotalDislikes = likes.Count(l => !l.IsLiked),
                UserLiked = userLike?.IsLiked
            };

            return Ok(stats);
        }

        [HttpPost]
        public async Task<ActionResult> ToggleLike(CreateMovieLikeDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var existingLike = await _context.MovieLikes
                .FirstOrDefaultAsync(l => l.MovieId == dto.MovieId && l.UserId == userId);

            if (existingLike != null)
            {
                if (existingLike.IsLiked == dto.IsLiked)
                {
                    _context.MovieLikes.Remove(existingLike);
                }
                else
                {
                    existingLike.IsLiked = dto.IsLiked;
                    existingLike.CreatedAt = DateTime.UtcNow;
                }
            }
            else
            {
                var like = new MovieLike
                {
                    MovieId = dto.MovieId,
                    UserId = userId,
                    IsLiked = dto.IsLiked,
                    CreatedAt = DateTime.UtcNow
                };
                _context.MovieLikes.Add(like);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("movie/{movieId}")]
        public async Task<IActionResult> RemoveLike(int movieId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var like = await _context.MovieLikes
                .FirstOrDefaultAsync(l => l.MovieId == movieId && l.UserId == userId);

            if (like == null)
                return NotFound();

            _context.MovieLikes.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}