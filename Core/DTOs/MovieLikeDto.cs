using System;

namespace Core.DTOs
{
    public class MovieLikeDto
    {
        public int Id { get; set; }
        public bool IsLiked { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }

    public class CreateMovieLikeDto
    {
        public int MovieId { get; set; }
        public bool IsLiked { get; set; } 
    }

    public class MovieLikeStatsDto
    {
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public bool? UserLiked { get; set; } 
    }
}