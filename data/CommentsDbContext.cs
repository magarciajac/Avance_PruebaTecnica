using Microsoft.EntityFrameworkCore;
using SentimentApi.Models;

namespace SentimentApi.Data
{
    public class CommentsDbContext : DbContext
    {
        public CommentsDbContext(DbContextOptions<CommentsDbContext> options)
            : base(options) { }

        public DbSet<Comment> Comments { get; set; } = null!;
    }
}