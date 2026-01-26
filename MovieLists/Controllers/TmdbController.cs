using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MovieLists.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TmdbController : ControllerBase
    {
        private readonly ITmdbService _tmdbService;

        public TmdbController(ITmdbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            var result = await _tmdbService.SearchMoviesAsync(query);
            return Ok(result);
        }

        [HttpGet("{tmdbId}")]
        public async Task<IActionResult> GetMovieDetails(int tmdbId)
        {
            var result = await _tmdbService.GetMovieDetailsAsync(tmdbId);
            return Ok(result);
        }

        [HttpGet("{tmdbId}/backdrops")]
        public async Task<IActionResult> GetMovieBackdrops(int tmdbId)
        {
            var backdrops = await _tmdbService.GetMovieBackdropsAsync(tmdbId);
            return Ok(backdrops);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularMovies([FromQuery] int page = 1)
        {
            var result = await _tmdbService.GetPopularMoviesAsync(page);
            return Ok(result);
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            var result = await _tmdbService.GetGenresAsync();
            return Ok(result);
        }

        [HttpGet("discover")]
        public async Task<IActionResult> DiscoverMovies([FromQuery] string category = "popular", [FromQuery] int page = 1)
        {
            try
            {
                var result = await _tmdbService.DiscoverMoviesAsync(category, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}