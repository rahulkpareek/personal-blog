using BlogProject.Model;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BlogProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly string _jsonFilePath;
        private List<Article>? _articles;

        public BlogController()
        {
            _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "articles.json");
            try
            {
                _articles = ReadJsonFile(_jsonFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                _articles = new List<Article>();
            }
        }

        private string GetAuthenticatedUsername(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
                return string.Empty;

            var credentialBytes = Convert.FromBase64String(authHeader.Replace("Basic ", ""));
            var credentials = System.Text.Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            return credentials[0];
        }

        [HttpGet]
        [Route("GetArticles")]
        public IActionResult GetArticles()
        {
            var username = GetAuthenticatedUsername(HttpContext);
            var userArticles = _articles?.Where(a => a.Author == username).ToList();

            if (userArticles == null || userArticles.Count == 0)
            {
                return NotFound("No articles found for the current user.");
            }
            return Ok(userArticles);
        }

        [HttpGet]
        [Route("GetArticleById/{id}")]
        public IActionResult GetArticleById(string id)
        {
            if (_articles == null || _articles.Count == 0)
            {
                return NotFound("No articles found.");
            }

            var username = GetAuthenticatedUsername(HttpContext);
            var article = _articles.FirstOrDefault(a => a.Id == id && a.Author == username);
            if (article == null)
            {
                return NotFound($"Article with ID {id} not found or you don't have access to it.");
            }
            return Ok(article);
        }

        [HttpPost]
        [Route("CreateArticle")]
        public IActionResult CreateArticle(string title, string content)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                return BadRequest("Title and content cannot be empty.");
            }

            var username = GetAuthenticatedUsername(HttpContext);
            var newArticle = new Article(title, content, username);
            _articles?.Add(newArticle);

            SaveArticlesToFile();

            return CreatedAtAction(nameof(GetArticleById), new { id = newArticle.Id }, newArticle);
        }

        [HttpPut]
        [Route("UpdateArticle/{id}")]
        public IActionResult UpdateArticle(string id, string title, string content)
        {
            if (_articles == null || _articles.Count == 0)
            {
                return NotFound("No articles found.");
            }

            var username = GetAuthenticatedUsername(HttpContext);
            var article = _articles.FirstOrDefault(a => a.Id == id && a.Author == username);
            if (article == null)
            {
                return NotFound($"Article with ID {id} not found or you don't have access to it.");
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                return BadRequest("Title and content cannot be empty.");
            }

            article.Title = title;
            article.Content = content;
            article.UpdatedAt = DateTime.Now;

            SaveArticlesToFile();

            return Ok(article);
        }

        [HttpDelete]
        [Route("DeleteArticle/{id}")]
        public IActionResult DeleteArticle(string id)
        {
            if (_articles == null || _articles.Count == 0)
            {
                return NotFound("No articles found.");
            }

            var username = GetAuthenticatedUsername(HttpContext);
            var article = _articles.FirstOrDefault(a => a.Id == id && a.Author == username);
            if (article == null)
            {
                return NotFound($"Article with ID {id} not found or you don't have access to it.");
            }

            _articles.Remove(article);
            SaveArticlesToFile();

            return Ok($"Article with ID {id} deleted successfully.");
        }

        private List<Article>? ReadJsonFile(string jsonFilePath)
        {
            if (!System.IO.File.Exists(jsonFilePath))
            {
                System.IO.File.Create(jsonFilePath).Close();
                return new List<Article>();
            }

            using (var stream = System.IO.File.OpenRead(jsonFilePath))
            {
                var reader = new StreamReader(stream);
                string jsonContent = reader.ReadToEnd();
                var articles = System.Text.Json.JsonSerializer.Deserialize<List<Article>>(jsonContent);
                reader.Close();
                return articles;
            }
        }

        private void SaveArticlesToFile()
        {
            System.IO.File.WriteAllText(_jsonFilePath, System.Text.Json.JsonSerializer.Serialize(_articles));
        }
    }
}
