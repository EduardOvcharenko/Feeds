using Feeds.Models;
using Microsoft.EntityFrameworkCore;

namespace Feeds.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CollectionFeeds> CollectionFeeds { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate(); 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CollectionFeeds>()
                .HasKey(cf => new { cf.FeedId, cf.CollectionId });
        }
    }
}
