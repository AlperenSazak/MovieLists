using System;

namespace Core.Entities
{
    public class MovieLike
    {
        public int Id { get; set; }
        public bool IsLiked { get; set; } 
        public DateTime CreatedAt { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}