using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Travelogue.Models;
using Travelogue.Models.Blogs;

namespace Travelogue.Data
{
    public class StoryContext : IdentityDbContext<TravelUser>
    {
        private IConfigurationRoot _config;

        public StoryContext(DbContextOptions<StoryContext> options, IConfigurationRoot config) : base(options)
        {
            _config = config;
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<SubPost> SubPosts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Stop> Stops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<SubPost>().ToTable("SubPosts");
            modelBuilder.Entity<Trip>().ToTable("Trips");
            modelBuilder.Entity<Stop>().ToTable("Stops");

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.UseSqlServer("Server=EOINERD\\SQLEXPRESS;Database=StoryDb;Trusted_Connection=true;MultipleActiveResultSets=true");
        }
    }
}
