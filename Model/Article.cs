using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Model
{    public class Article
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Article(string title, string content, string author)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Content = content;
            Author = author;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}