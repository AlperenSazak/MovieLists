using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreateMovieDto
    {
        public int TmdbId { get; set; }
        public string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double? VoteAverage { get; set; }
        public int? Runtime { get; set; }
        public DateTime WatchedDate { get; set; }
        public int? UserRating { get; set; }
        public string? UserNotes { get; set; }
        public string? Genres { get; set; }
        public int UserId { get; set; }
    }
}