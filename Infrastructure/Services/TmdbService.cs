using Core.DTOs;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public TmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TmdbApi:ApiKey"];
            _baseUrl = configuration["TmdbApi:BaseUrl"];
        }

        public async Task<TmdbSearchResultDto> SearchMoviesAsync(string query)
        {
            var url = $"{_baseUrl}/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&language=tr-TR";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TmdbSearchResultDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        public async Task<TmdbMovieDto> GetMovieDetailsAsync(int tmdbId)
        {
            var url = $"{_baseUrl}/movie/{tmdbId}?api_key={_apiKey}&language=tr-TR&append_to_response=videos";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TmdbMovieDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        public async Task<List<string>> GetMovieBackdropsAsync(int tmdbId)
        {
            var url = $"{_baseUrl}/movie/{tmdbId}/images?api_key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            var backdrops = new List<string>();
            if (json.RootElement.TryGetProperty("backdrops", out var backdropsArray))
            {
                foreach (var backdrop in backdropsArray.EnumerateArray())
                {
                    if (backdrop.TryGetProperty("file_path", out var filePath))
                    {
                        backdrops.Add($"https://image.tmdb.org/t/p/original{filePath.GetString()}");
                    }
                }
            }

            return backdrops;
        }

        public async Task<TmdbSearchResultDto> GetPopularMoviesAsync(int page = 1)
        {
            var url = $"{_baseUrl}/movie/popular?api_key={_apiKey}&language=tr-TR&page={page}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TmdbSearchResultDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        public async Task<TmdbGenresResultDto> GetGenresAsync()
        {
            var url = $"{_baseUrl}/genre/movie/list?api_key={_apiKey}&language=tr-TR";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TmdbGenresResultDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        public async Task<TmdbSearchResultDto> DiscoverMoviesAsync(string category, int page)
        {
            var url = $"{_baseUrl}/movie/{category}?api_key={_apiKey}&language=tr-TR&page={page}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TmdbSearchResultDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }
    }
}