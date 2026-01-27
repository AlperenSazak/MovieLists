namespace Core.Entities
{
    public class CommentLike
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public bool IsLike { get; set; } 
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public Comment Comment { get; set; }
        public User User { get; set; }
    }
}