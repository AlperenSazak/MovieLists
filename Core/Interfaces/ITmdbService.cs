using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITmdbService
    {
        Task<TmdbSearchResultDto> SearchMoviesAsync(string query);
        Task<TmdbMovieDto> GetMovieDetailsAsync(int tmdbId);
        Task<List<string>> GetMovieBackdropsAsync(int tmdbId);
        Task<TmdbSearchResultDto> GetPopularMoviesAsync(int page = 1);
        Task<TmdbGenresResultDto> GetGenresAsync();
        Task<TmdbSearchResultDto> DiscoverMoviesAsync(string category, int page);
    }
}