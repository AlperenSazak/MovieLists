namespace Core.DTOs
{
    public class CreateCommentLikeDto
    {
        public int CommentId { get; set; }
        public bool IsLike { get; set; }
    }

    public class CommentLikeStatsDto
    {
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public bool? UserLiked { get; set; } 
    }
}