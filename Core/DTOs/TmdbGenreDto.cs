using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.DTOs
{
    public class TmdbGenreDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class TmdbGenresResultDto
    {
        [JsonPropertyName("genres")]
        public List<TmdbGenreDto> Genres { get; set; }
    }
}