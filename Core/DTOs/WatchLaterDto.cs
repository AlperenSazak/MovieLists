using System;

namespace Core.DTOs
{
    public class WatchLaterDto
    {
        public int Id { get; set; }
        public int TmdbId { get; set; }
        public string Title { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double? VoteAverage { get; set; }
        public string? Genres { get; set; }
        public DateTime AddedDate { get; set; }
        public int UserId { get; set; }
    }

    public class CreateWatchLaterDto
    {
        public int TmdbId { get; set; }
        public string Title { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double? VoteAverage { get; set; }
        public string? Genres { get; set; }
    }
}