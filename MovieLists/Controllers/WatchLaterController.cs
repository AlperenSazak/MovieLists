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
    public class WatchLaterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WatchLaterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchLaterDto>>> GetWatchLaterMovies()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var movies = await _context.WatchLaterMovies
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.AddedDate)
                .Select(w => new WatchLaterDto
                {
                    Id = w.Id,
                    TmdbId = w.TmdbId,
                    Title = w.Title,
                    PosterPath = w.PosterPath,
                    BackdropPath = w.BackdropPath,
                    Overview = w.Overview,
                    ReleaseDate = w.ReleaseDate,
                    VoteAverage = w.VoteAverage,
                    Genres = w.Genres,
                    AddedDate = w.AddedDate,
                    UserId = w.UserId
                })
                .ToListAsync();

            return Ok(movies);
        }

        [HttpPost]
        public async Task<ActionResult<WatchLaterDto>> AddToWatchLater(CreateWatchLaterDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var exists = await _context.WatchLaterMovies
                .AnyAsync(w => w.UserId == userId && w.TmdbId == dto.TmdbId);

            if (exists)
            {
                return BadRequest("Bu film zaten 'Daha Sonra İzle' listenizde.");
            }

            var watchLater = new WatchLater
            {
                TmdbId = dto.TmdbId,
                Title = dto.Title,
                PosterPath = dto.PosterPath,
                BackdropPath = dto.BackdropPath,
                Overview = dto.Overview,
                ReleaseDate = dto.ReleaseDate,
                VoteAverage = dto.VoteAverage,
                Genres = dto.Genres,
                UserId = userId,
                AddedDate = DateTime.UtcNow
            };

            _context.WatchLaterMovies.Add(watchLater);
            await _context.SaveChangesAsync();

            var result = new WatchLaterDto
            {
                Id = watchLater.Id,
                TmdbId = watchLater.TmdbId,
                Title = watchLater.Title,
                PosterPath = watchLater.PosterPath,
                BackdropPath = watchLater.BackdropPath,
                Overview = watchLater.Overview,
                ReleaseDate = watchLater.ReleaseDate,
                VoteAverage = watchLater.VoteAverage,
                Genres = watchLater.Genres,
                AddedDate = watchLater.AddedDate,
                UserId = watchLater.UserId
            };

            return CreatedAtAction(nameof(GetWatchLaterMovies), result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFromWatchLater(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var movie = await _context.WatchLaterMovies.FindAsync(id);

            if (movie == null)
                return NotFound();

            if (movie.UserId != userId)
                return Forbid();

            _context.WatchLaterMovies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("tmdb/{tmdbId}")]
        public async Task<IActionResult> DeleteByTmdbId(int tmdbId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var movie = await _context.WatchLaterMovies
                .FirstOrDefaultAsync(w => w.UserId == userId && w.TmdbId == tmdbId);

            if (movie == null)
                return NotFound();

            _context.WatchLaterMovies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("check/{tmdbId}")]
        public async Task<ActionResult<bool>> CheckIfInWatchLater(int tmdbId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var exists = await _context.WatchLaterMovies
                .AnyAsync(w => w.UserId == userId && w.TmdbId == tmdbId);

            return Ok(exists);
        }
    }
}