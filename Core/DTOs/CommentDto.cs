using System;

namespace Core.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } 
    }

    public class CreateCommentDto
    {
        public int MovieId { get; set; }
        public string Content { get; set; }
    }

    public class UpdateCommentDto
    {
        public string Content { get; set; }
    }
}