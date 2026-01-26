using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Core.DTOs;
using Core.Mappings;
using Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MovieLists.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.User)
                .ToListAsync();

            return Ok(movies.Select(m => m.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(CreateMovieDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var existingMovie = await _context.Movies
                .FirstOrDefaultAsync(m => m.UserId == userId && m.TmdbId == dto.TmdbId);

            if (existingMovie != null)
            {
                return BadRequest("Bu film kütüphanenizde bulunmaktadır.");
            }

            var movie = dto.ToEntity();
            movie.UserId = userId;
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var createdMovie = await _context.Movies
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == movie.Id);

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, createdMovie.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("my-movies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMyMovies()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var movies = await _context.Movies
                .Include(m => m.User)
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.WatchedDate)
                .ToListAsync();

            return Ok(movies.Select(m => m.ToDto()));
        }
    }
}