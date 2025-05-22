# Blog API Project

A RESTful API for managing blog articles built with ASP.NET Core 9.0. This API provides basic CRUD operations for blog articles with JSON file-based persistence.

## Features

- Get all articles
- Get article by ID
- Create new articles
- Update existing articles
- Delete articles
- Swagger UI for API documentation and testing
- JSON file-based storage

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /Blog/GetArticles | Retrieve all articles |
| GET | /Blog/GetArticleById/{id} | Retrieve a specific article by ID |
| POST | /Blog/CreateArticle | Create a new article |
| PUT | /Blog/UpdateArticle/{id} | Update an existing article |
| DELETE | /Blog/DeleteArticle/{id} | Delete an article |

## Prerequisites

- .NET 9.0 SDK
- Visual Studio 2025 or Visual Studio Code

## Getting Started

1. Clone the repository
2. Navigate to the project directory:
```powershell
cd BlogProject
```
3. Run the application:
```powershell
dotnet run
```
4. Access the Swagger UI at: `https://localhost:5001/swagger`

## Project Structure

```
BlogProject/
├── Controllers/
│   └── BlogController.cs    # API endpoints implementation
├── Model/
│   └── Article.cs          # Article data model
├── Program.cs              # Application entry point and configuration
├── articles.json          # Data storage file
└── appsettings.json      # Application configuration
```

## API Request Examples

### Create Article
```http
POST /Blog/CreateArticle
Content-Type: application/json

{
    "title": "My First Article",
    "content": "This is the content of my first article"
}
```

### Update Article
```http
PUT /Blog/UpdateArticle/{id}
Content-Type: application/json

{
    "title": "Updated Article Title",
    "content": "This is the updated content"
}
```

## Data Storage

The application uses a JSON file (`articles.json`) for data persistence. Each article contains:
- Unique ID (auto-generated)
- Title
- Content
- Creation Date
- Last Updated Date

## Development

The project uses:
- ASP.NET Core 9.0
- Swagger/OpenAPI for API documentation
- System.Text.Json for JSON handling

## Contributing

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
