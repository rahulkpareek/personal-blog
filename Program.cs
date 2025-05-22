using BlogProject.Middleware;
using BlogProject.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BlogProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Blog API", 
                    Version = "v1",
                    Description = "A simple Blog API for managing articles"
                });
            });

            var app = builder.Build();            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1");
                });
            }            app.UseHttpsRedirection();
            app.UseAuthorization(); 
            
            // Add Basic Authentication middleware
            app.UseBasicAuthentication();
            
            app.MapControllers();

            app.MapGet("/", () => "Welcome to the Blog API!");

            app.Run();
        }
    }
}

