using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.DTOs
{
    // Genre DTO
    public class TmdbGenreDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    // Genres Response DTO
    public class TmdbGenresResultDto
    {
        [JsonPropertyName("genres")]
        public List<TmdbGenreDto> Genres { get; set; }
    }
}