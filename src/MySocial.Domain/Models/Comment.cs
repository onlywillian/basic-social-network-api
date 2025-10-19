﻿namespace MySocial.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Message { get; set; } = null!;

        public int PostId { get; set; }

        public Post Post { get; set; } = null!;

        public int AuthorId { get; set; }

        public User Author { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
