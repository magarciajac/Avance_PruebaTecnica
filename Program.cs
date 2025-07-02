using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SentimentApi.Data;
using SentimentApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Configurar EF Core para usar siempre SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CommentsDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar migraciones pendientes
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CommentsDbContext>();
    db.Database.Migrate();
    
}

// ** Registrar Swagger sin condición **
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SentimentApi v1");
    c.RoutePrefix = "swagger";
});

//app.UseHttpsRedirection();

// Clasificación de sentimiento
var positiveWords = new[] { "excelente", "genial", "fantástico", "bueno", "increíble" };
var negativeWords = new[] { "malo", "terrible", "problema", "defecto", "horrible" };

string ClassifySentiment(string text)
{
    var lower = text.ToLower();
    if (positiveWords.Any(w => lower.Contains(w))) return "positivo";
    if (negativeWords.Any(w => lower.Contains(w))) return "negativo";
    return "neutral";
}

// ------------------------------------------------------------
// NOTE:
// Para mantener la prueba simple, todos los endpoints se
// definen aquí mismo en Program.cs usando Minimal APIs.
//
// En un proyecto de producción se moverían a métodos de
// extensión (e.g. app.MapCommentsEndpoints()) o a controladores
// dedicados para mejorar la mantenibilidad, pruebas y
// separación de responsabilidades.
// ------------------------------------------------------------


// POST /api/comments
app.MapPost("/api/comments", async (CommentsDbContext db, Comment comment) =>
{
    comment.Sentiment = ClassifySentiment(comment.CommentText);
    comment.CreatedAt = DateTime.UtcNow;
    db.Comments.Add(comment);
    await db.SaveChangesAsync();
    return Results.Created($"/api/comments/{comment.Id}", comment);
})
.WithName("PostComment")
.WithOpenApi();

// GET /api/comments
app.MapGet("/api/comments", async (CommentsDbContext db, [FromQuery] string? productId, [FromQuery] string? sentiment) =>
{
    var query = db.Comments.AsQueryable();
    if (!string.IsNullOrEmpty(productId))
        query = query.Where(c => c.ProductId == productId);
    if (!string.IsNullOrEmpty(sentiment))
        query = query.Where(c => c.Sentiment == sentiment);
    var list = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    return Results.Ok(list);
})
.WithName("GetComments")
.WithOpenApi();

// GET /api/comments/sentiment-summary
app.MapGet("/api/comments/sentiment-summary", async (CommentsDbContext db) =>
{
    var total = await db.Comments.CountAsync();
    var counts = await db.Comments.GroupBy(c => c.Sentiment)
        .Select(g => new { Sentiment = g.Key, Count = g.Count() })
        .ToListAsync();
    var summary = new
    {
        total_comments = total,
        sentiment_counts = counts.ToDictionary(x => x.Sentiment, x => x.Count)
    };
    return Results.Ok(summary);
})
.WithName("GetSentimentSummary")
.WithOpenApi();



app.Run();