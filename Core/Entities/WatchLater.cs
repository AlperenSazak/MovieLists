using System;

namespace Core.Entities
{
    public class WatchLater
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

        // Foreign Key
        public int UserId { get; set; }
        public User User { get; set; }
    }
}